using NAudio.Wave;
using Pocketsphinx;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace VoiceControllerTool
{
    /// <summary>
    /// Interaction logic for AddPhraseWindow.xaml
    /// </summary>
    public partial class AddPhraseWindow : Window
    {

        MainWindow main;
        MicrophoneHandler micHandler;
        Pocketsphinx.Decoder decoder;

        int maxRecTime = 20; 
        bool recording;

        DispatcherTimer timer;
        TimeSpan time;



        public delegate void SpeechRecognizedHandler(string phrase);
        public event SpeechRecognizedHandler OnSpeechRecognized;


        public AddPhraseWindow(MainWindow _main)
        {
            InitializeComponent();

            SetupMicrophone();
            LoadDevices();
            SetupKPDecoder(); //doesn't work yet it complains about not finding the dll for config

            main = _main;
            recording = false;

            time = TimeSpan.FromSeconds(10);
            timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                if (time == TimeSpan.Zero) timer.Stop();
                time = time.Add(TimeSpan.FromSeconds(-1));

            }, Application.Current.Dispatcher);
        }

        private void SetupMicrophone()
        {
            micHandler = new MicrophoneHandler(WaveIn.GetCapabilities(0).ProductName, MicrophoneHandler.SampelingRateEnum.SixteenK, maxRecTime);
            micHandler.RecordingFinished += ProcessAudio;
        }


        private void AddPhraseBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Recording button has three outputs that it can yield,
        /// 
        /// Green: Means that the voice input could be mathced to the key-phrase and 
        ///        that it will be sent of to validate if it already exists or not.
        ///         
        /// Yellow: Means that voice input couldn't be connected to the key-phrase. 
        ///         This could either mean that the voice command utterd wasn't close
        ///         enough or that the key-phrase enterd might be "too" long. 
        ///         
        /// Red:    Means that the keyphrase was invalidated, this could be because 
        ///         it already exists.
        ///         
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!recording)
            {
                recording = true;
                ErrorLbl.Visibility = Visibility.Hidden;
                RecBtn.Background = new SolidColorBrush(Color.FromArgb(133, 30, 30, 0));
                RecBtn.Content = "Recording";


                SetRecBtn();

                //Ask the user if what they have entered is correct an then start the recording
                MessageBoxResult result = MessageBox.Show("Is the keyphrase correct?\n " + AddPhraseBox.Text, "Confirm Keyphrase", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result != MessageBoxResult.OK) return;


                micHandler.StartRecording(DevicesComboBox.SelectedIndex, DevicesComboBox.Text);

            }
            else if (AddPhraseBox.Text == "Enter keyphrase/ phrases" || AddPhraseBox.Text == string.Empty)
            {
                ErrorLbl.Visibility = Visibility.Visible;
                ErrorLbl.Content = "Cannot leave the field empty \n" +
                                    "or \n" +
                                    "enter the helper text";
                RecBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA0A0A0"));
                RecBtn.Content = "Record";
                recording = false;

                SetRecBtn();
                //Stop recording get the channels and sample rate from waveform

                micHandler.StopRecording();
            }
            else if (recording && AddPhraseBox.Text != "Enter keyphrase/ phrases" && AddPhraseBox.Text != string.Empty)
            {

                main.AddNewKeyphrase(AddPhraseBox.Text);


                //Use the timer to reset the indicator and to hold the window before sending the new keyphrase to the main, allowing the user to understand
                //what has happend.
                timer.Start();
                this.Close();
            }

            /*Check the 3 states and*/
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Turns the indicator button off and on, whenever it is capturing
        /// </summary>
        private void SetRecBtn()
        {
            if (recording == true)
            {
                indicatorBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5895FF"));
                indicatorBtn.Content = "REC";
            }
            else
            {
                indicatorBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA0A0A0"));
                indicatorBtn.Content = "";
            }


            /*
              indicatorBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCC1E")); // yelllow 
              indicatorBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCC1E1E")); // red
              indicatorBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1ECC1E")); // green
             */
        }

        private void UpdateButtons()
        {

        }


        /// <summary>
        /// Loads the devices (microphones) found on the current system.
        /// </summary>
        private void LoadDevices()
        {
            foreach (var d in micHandler.deviceSources)
            {
                DevicesComboBox.Items.Add(d.ProductName);
            }
            DevicesComboBox.SelectedIndex = 0;
            micHandler.SelectedDeviceIndex = DevicesComboBox.SelectedIndex;
        }

        private void SetupKPDecoder()
        {
            Config c = Pocketsphinx.Decoder.DefaultConfig();

            string speechDataPath = "C:/Users/jasmi/source/repos/WpfPocketsphinxTest/WpfPocketsphinxTest/StreamingAssets/en-us";

            string dictPath = "C:/Users/jasmi/source/repos/WpfPocketsphinxTest/WpfPocketsphinxTest/StreamingAssets/en-us/dictionary";

            string logPath = "C:/Users/jasmi/source/repos/WpfPocketsphinxTest/WpfPocketsphinxTest/StreamingAssets/en-us/en-us.lm.bin";

            string keywordsPath = "C:/Users/jasmi/source/repos/WpfPocketsphinxTest/WpfPocketsphinxTest/StreamingAssets/keywords.txt";

            c.SetString("-hmm", speechDataPath);

            c.SetString("-dict", dictPath);

            c.SetString("-kws", keywordsPath);

            /* How accurate our decoder will be. For shorter keyphrases you can use smaller thresholds like 1e-1, 
             * for longer keyphrases the threshold must be bigger, up to 1e-50. 
             * If your keyphrase is very long – larger than 10 syllables – it is recommended to split it 
             * and spot for parts separately. 
             */
            c.SetFloat("-kws_threshold", 1e-15);

            //// These two lines enable and save raw data to a log for debugging. 
            //c.SetString("-logfn", logPath);
            //c.SetString("-rawlogdir", "adress here"); //output the debug to where we want the data. 

            //Create a decoder with out configuration setup
            decoder = new Pocketsphinx.Decoder(c);
            //Start the decoder
            decoder.StartUtt();
        }

        private void ProcessAudio(WaveIn sourceStream)
        {
            // Create a new array for our audio data. 1
            var newData = new float[sourceStream.WaveFormat.SampleRate * sourceStream.WaveFormat.Channels];
            // Get our data from our sourceStream. 2

            // Convert audio data into byte data. 3
            byte[] byteData = ConvertToBytes(newData, sourceStream.WaveFormat.Channels);
            // Process the raw byte data with our decoder. 4
            decoder.ProcessRaw(byteData, byteData.Length, false, false);
            // Checks if we recognize a keyphrase. 5
            if (decoder.Hyp() != null)
            {
                // Fire our event. 5.1
                if (OnSpeechRecognized != null)
                    OnSpeechRecognized.Invoke(decoder.Hyp().Hypstr);
                // Stop the decoder. 5.2
                decoder.EndUtt();
                // Start the decoder again. 5.3
                decoder.StartUtt();
            }
        }


        //Converts audio input from float to byte based on the used microphone
        private static byte[] ConvertToBytes(float[] data, int channels)
        {
            float tot = 0;
            byte[] byteData = new byte[data.Length / channels * 2];
            for (int i = 0; i < data.Length / channels; i++)
            {
                float sum = 0;
                for (int j = 0; j < channels; j++)
                {
                    sum += data[i * channels + j];
                }
                tot += sum * sum;
                short val = (short)(sum / channels * 20000); // volume
                byteData[2 * i] = (byte)(val & 0xff);
                byteData[2 * i + 1] = (byte)(val >> 8);
            }
            return byteData;
        }

    }
}
