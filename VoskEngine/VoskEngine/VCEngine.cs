using System;
using Vosk;
using System.IO;
using System.Collections.Generic;
using VCTUtility;
using Newtonsoft.Json;


namespace VoskEngine
{
    public class VCEngine
    {


        private static Model spkModel;
        private MFCC mfcc;



        public static void Main()
        {
            Vosk.Vosk.SetLogLevel(0);


            var projecDir = Directory.GetParent(Directory.GetParent(@"../").Parent.Parent.FullName);
            var filePath = @"C:\Users\jasmi\source\repos\Ex-VoiceCon\VoskEngine\VoskEngine\model\model-en\"; //This might have to be added as a resx path and then reffed that way.
            var path2 = @"C:\Users\jasmi\OneDrive\Desktop\model-en\";
            var path = Path.Combine(projecDir.FullName, filePath);

            spkModel = new Model(path2); //fix path so that it gets it from the device


            //DemoBytes(model); //to test Vosk
        }

        /// <summary>
        /// To test that the engine works
        /// </summary>
        /// <param name="model"></param>
        private static void DemoBytes(Model model)
        {
            // Demo byte buffer
            VoskRecognizer rec = new VoskRecognizer(model, 16000.0f); //speechmodel in Vosk is 8000 hz in samplerate maybe change?
            using (Stream source = File.OpenRead(@"C:\Users\jasmi\source\repos\Ex-VoiceCon\VoskEngine\VoskEngine\test.wav"))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (rec.AcceptWaveform(buffer, bytesRead))
                    {
                        Console.WriteLine(rec.Result());
                    }
                    else
                    {
                        Console.WriteLine(rec.PartialResult());
                    }
                }
            }
            Console.WriteLine(rec.FinalResult());
        }


        private void DetectKW() //called before MFCC to get Vosk Speech To Text - DepricaTED?
        {
            VoskRecognizer rec = new VoskRecognizer(spkModel, 16000.0f); //16000hz = 16bit sample rate
        }

        public void ValidateKeyphrase(MemoryStream stream, string IncomingPhrase,int sampleRate, int numbOfChannels, VoiceData[] keyWords) //need to send in vocieDAtalist
        {
            Console.Write(spkModel.ToString());//test but will probably just print trash
            VoskRecognizer rec = new VoskRecognizer(spkModel, sampleRate);
            double[][] inputMfccCoefficents;
            List<ModelKeyWord> SpeechModelWords = new List<ModelKeyWord>();
            string[] phraseAsWords = IncomingPhrase.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            byte[] byteBuffer = new byte[4096];
            double[] buffer = new double[stream.Length];
            int bytesRead;
            int count = 0;
            double detectionQiality = 1.0; //this is 1 if the entire phrase is detected. Value will decrease with each word wrong 


            bool[] validatedIndexes; //will get size of the detectedWordsInText and then each index will represent the success or not for that word
            string recognizedResultData = string.Empty;
            string[] detectedWordsInText;
            

            while ((bytesRead = stream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
            {//Checks to see if the STT conversion gets any detected words out
                if (rec.AcceptWaveform(byteBuffer, bytesRead)) {
                    recognizedResultData = rec.Result(); //use final result and activate it when the user presses the add button again, during the game use result
                    SpeechModelWords = LoadJsonResult(recognizedResultData);
            
                }
                else {
                    recognizedResultData = rec.PartialResult();
                    //Check if its power value is close enught to the vosk-speechmodel and then save or trash
                }
                buffer[count] = BitConverter.ToInt16(byteBuffer, 0);
            }



           
            if (phraseAsWords.Length != SpeechModelWords.Count)
            {
                throw new ArgumentException("the incoming phrase and the detected speech did not match." +
                    "Incoming Phrase: " + IncomingPhrase + "  Detected Wrods from model: " + SpeechModelWords); //need to iterate trhough the wrods detected instead of printeing the whole obj
            }
            else
            {
                validatedIndexes = new bool[SpeechModelWords.Count];
            }

            for (int i = 0; i < SpeechModelWords.Count; i++)
            {
                //check incoming words agains the speechmodels words
                //if they match, then use the confidence from the speechmodels words and have them in the output

                if (phraseAsWords[i] == SpeechModelWords.ToArray()[i].word)
                {
                    validatedIndexes[i] = true;
                }
                else
                {
                    validatedIndexes[i] = false;
                }
            }

            //set the detection value so that we can consider if we want to test train the word or nah
            detectionQiality = detectionQiality / validatedIndexes.Length;
            int iteratorForPositiveValues = 0;
            foreach (var result in validatedIndexes)
            {
               if(result == true)
                {
                    iteratorForPositiveValues++;
                }
            }
            detectionQiality = detectionQiality * iteratorForPositiveValues;

            if (detectionQiality > 0.8) //tinker with this value
            {
                //if the detected words dont exits in the validation set (fully),
                //add and save


                //minFreq
                mfcc = new MFCC(sampleRate, 512, 20, true, 20.0, 8000.0, 40); //windwowsize 32msek = 512 samples 16msek = 256 samples. numberOfCoefficients should be between 20-40 since thats where human speech mostly exists: MaxFreq 8000/16000
                inputMfccCoefficents = mfcc.Process(buffer);

                if (inputMfccCoefficents.Length == 0)
                    throw new ArgumentException("the input stream is to short to process");

                //Matrix mfccs = new Matrix(inputMfccCoefficents, inputMfccCoefficents.GetLength(0), inputMfccCoefficents.GetLength(1));
                //compare this incoming "mfccs" with the speechmodels validation data mfcc-matrix

              

                //If validation is true, then train AI
                NeuralNetwork agent = new NeuralNetwork(inputMfccCoefficents);//Send in how many layers  //inputMfccCoefficents
         
                agent.
                
            }

        }


        private List<ModelKeyWord> LoadJsonResult(string s)
        {
            List<ModelKeyWord> SpeechModelWords = JsonConvert.DeserializeObject<List<ModelKeyWord>>(s);
            return SpeechModelWords;
        }



    }

    
      
}
public class ModelKeyWord
{
   public double confidence;
   public double start;
   public double end;
   public string word;
}