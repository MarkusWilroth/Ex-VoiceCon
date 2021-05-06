﻿using System;
using Vosk;
using System.IO;

namespace VoskEngine
{
    public class VCEngine
    {

        private static Model model;
        private MFCC mfcc;
        public static void Main(string[] args)
        {
            Vosk.Vosk.SetLogLevel(0);


            var projecDir = Directory.GetParent(Directory.GetParent(@"../").Parent.Parent.FullName);
            var filePath = @"C:\Users\jasmi\source\repos\Ex-VoiceCon\VoskEngine\VoskEngine\model\model-en\"; //This might have to be added as a resx path and then reffed that way.
            var path2 = @"C:\Users\jasmi\OneDrive\Desktop\model-en\";
            var path = Path.Combine(projecDir.FullName, filePath);

            model = new Model(path2);
            
            
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

        public void ValidateKeyphrase(MemoryStream stream, int sampleRate, int numbOfChannels)
        {
            VoskRecognizer rec = new VoskRecognizer(model, sampleRate);
        

            byte[] buffer = new byte[4096];
            int bytesRead;

            string detectedText;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (rec.AcceptWaveform(buffer, bytesRead)) //check to see if basic detection goes through
                {
                    detectedText = rec.Result();
                }
                else
                {
                    detectedText = rec.PartialResult();
                }
            }
            NeuralNetwork agent = new NeuralNetwork(buffer);

            //minFreq
            mfcc = new MFCC(sampleRate, 512, 20, true, 20.0, 16000.0, 40); //windwowsize 32msek = 512 samples 16msek = 256 samples. numberOfCoefficients should be between 20-40 since thats where human speech mostly exists
            
            //If validation is true, then train AI 
        }

        public void TrainSet()
        {
            
        }

    }
}
