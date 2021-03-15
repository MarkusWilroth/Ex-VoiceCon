using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace VoiceControllerTool
{
    class MicrophoneHandler
    {
        public enum SampelingRateEnum
        {
            [Description("8K")]
            EightK = 8000,
            [Description("11K")]
            ElevenK = 11025,
            [Description("16K")]
            SixteenK = 16000,
            [Description("22K")]
            TwentyTwoK = 22050,
            [Description("32K")]
            ThirtyTwoK = 32000,
            [Description("44K")]
            FortyFourK = 44100,
            [Description("48K")]
            FortyEightK = 48000,
        }

        public delegate void RecordingFinishedHandler(WaveIn sourceAud);
        public event RecordingFinishedHandler RecordingFinished;


        public string Name { get; } //Microphone name

        public int SelectedDeviceIndex { get; set; }

        public SampelingRateEnum SampelingRate { get; }
        public int ClipTime { get; }

        private Pocketsphinx.Decoder decoder; //both ms and ps have a definition for decoder thats why its specefied

        //BlockAlignReductionStream stream;
        DirectSoundOut output;
        WaveFileReader sourceFile;
        WaveFileWriter waveFileWriter;

        WaveIn sourceStream;


        public List<WaveInCapabilities> deviceSources { get; private set; }

        public MicrophoneHandler(string _name, SampelingRateEnum _sampelingRate, int _clipTime)
        {
            Name = _name;
            SampelingRate = _sampelingRate;
            ClipTime = _clipTime;

            DetectAudioSources();
        }

        public MicrophoneHandler()
        {
            DetectAudioSources();
        }


        //Find audio sources conncected to the current device
        private void DetectAudioSources()
        {
            deviceSources = new List<WaveInCapabilities>();

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                deviceSources.Add(WaveIn.GetCapabilities(i));
            }
        }


        public void StartRecording(int index, string deviceName)
        {
            if (deviceSources.Count == 0) return;

            SaveFileDialog save = new SaveFileDialog();  //Check if there's a setpath to a savefile, if nah, let the user navigate to it
            save.Filter = "Wave File (*.wav)|*.wav"; //switch this out to the dll save


            save.ShowDialog();  //Switch this out with a list of all the objects

            int selectedDevice = index;

            sourceStream = new WaveIn();
            sourceStream.DeviceNumber = selectedDevice;

            sourceStream.WaveFormat = new WaveFormat((int)SampelingRate, WaveIn.GetCapabilities(selectedDevice).Channels);

            sourceStream.DataAvailable += new EventHandler<WaveInEventArgs>(SourceStream_DataAvailable);
            waveFileWriter = new WaveFileWriter(save.FileName, sourceStream.WaveFormat); //same here dll save instead of wav save

            sourceStream.StartRecording();
        }


        private void SourceStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);

            waveFileWriter.Flush(); //clear the ram so that it doesn't hold the old recording
        }


        public void StopRecording()
        {
            int position = sourceStream.DeviceNumber;
            float[] soundData = new float[sourceStream.WaveFormat.SampleRate * sourceStream.WaveFormat.Channels];

            float[] newData = new float[position * sourceStream.WaveFormat.Channels];

            for (int i = 0; i < newData.Length; i++)
            {
                newData[i] = soundData[i];
            }

            byte[] byteData = ConvertToBytes(newData, sourceStream.WaveFormat.Channels);
            waveFileWriter.Write(byteData, 0, 0);

            if (RecordingFinished != null)
                RecordingFinished.Invoke(sourceStream);

            if (sourceStream != null)
            {
                sourceStream.StopRecording();

                sourceStream.Dispose();
                sourceStream = null;
            }

            if (waveFileWriter != null)
            {
                waveFileWriter.Dispose();
                waveFileWriter = null;
            }

        }

        private void SourceStream_DataAvailable1(object sender, WaveInEventArgs e)
        {
            throw new NotImplementedException();
        }

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
