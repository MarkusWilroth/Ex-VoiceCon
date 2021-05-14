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


        private static Model model;
        private MFCC mfcc;



        public static void Main()
        {
            Vosk.Vosk.SetLogLevel(0);


            var projecDir = Directory.GetParent(Directory.GetParent(@"../").Parent.Parent.FullName);
            var filePath = @"C:\Users\jasmi\source\repos\Ex-VoiceCon\VoskEngine\VoskEngine\model\model-en\"; //This might have to be added as a resx path and then reffed that way.
            var path2 = @"C:\Users\jasmi\OneDrive\Desktop\model-en\";
            var path = Path.Combine(projecDir.FullName, filePath);

            model = new Model(path2); //fix path so that it gets it from the device


            //DemoBytes(model); //to test Vosk
        }

        /// <summary>
        /// To test that the engine works
        /// </summary>
        /// <param name="model"></param>
        private static void DemoBytes(Model model)
        {
            // Demo byte buffer
            VoskRecognizer rec = new VoskRecognizer(model, 16000.0f);
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


        private void DetectKW() //called before MFCC to get Vosk Speech To Text
        {
            VoskRecognizer rec = new VoskRecognizer(model, 16000.0f); //16000hz = 16bit sample rate


        }

        public void ValidateKeyphrase(MemoryStream stream, int sampleRate, int numbOfChannels, VoiceData[] keyWords) //need to send in vocieDAtalist
        {
            VoskRecognizer rec = new VoskRecognizer(model, sampleRate);
            double[][] coefficientMatrix;
            List<ModelKeyWord> SpeechModelWords = new List<ModelKeyWord>();


            byte[] byteBuffer = new byte[4096];
            double[] buffer = new double[stream.Length];
            int bytesRead;
            int count = 0;


            bool[] validatedIndexes; //will get size of the detectedWordsInText and then each index will represent the success or not for that word
            string recognizedResultData = string.Empty;
            string[] detectedWordsInText;
            string[][] detectedWordsInValidationData = new string[keyWords.Length][]; ; //holds the whole phrases as arrays of arrays since each individual word is its own array


            while ((bytesRead = stream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
            {//Checks to see if the STT conversion gets any detected words out
                if (rec.AcceptWaveform(byteBuffer, bytesRead)) {
                    recognizedResultData = rec.Result(); //use final result and activate it when the user presses the add button again, during the game use result
                    SpeechModelWords = LoadJsonResult(recognizedResultData); 
                }
                else {
                    recognizedResultData = rec.PartialResult();
                }

                buffer[count] = BitConverter.ToInt16(byteBuffer, 0);
            }



            for (int i = 0; i < SpeechModelWords.Count; i++)
            {
                //check incoming words agains the speechmodels words
                //if they match, then use the confidence from the speechmodels words and have them in the output
            }



            //if the detected words dont exits in the validation set (fully),
            //add and save


            //minFreq
            mfcc = new MFCC(sampleRate, 512, 20, true, 20.0, 16000.0, 40); //windwowsize 32msek = 512 samples 16msek = 256 samples. numberOfCoefficients should be between 20-40 since thats where human speech mostly exists
            coefficientMatrix = mfcc.Process(buffer);

            if (coefficientMatrix.Length == 0)
                throw new ArgumentException("the input stream is to short to process");

            Matrix mfccs = new Matrix(coefficientMatrix, coefficientMatrix.GetLength(0), coefficientMatrix.GetLength(1));


            //coefficientMatrix = mfcc.dctMatrix.GetArray(); //Coefficients as it is right now showes the total amount of coefficients found in the detected soundstream

            //If validation is true, then train AI
            NeuralNetwork agent = new NeuralNetwork(coefficientMatrix);//Send in how many layers

            foreach (var coefficiets in coefficientMatrix) //this needs fixing. since it shoulw be the training function calling the FeedForward
            {
                agent.FeedForward(coefficiets);  //this is done when the words have been "accepted" as detected. We feed the agent that then uses it to train
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
    double confidence;
    double start;
    double end;
    string word;
}