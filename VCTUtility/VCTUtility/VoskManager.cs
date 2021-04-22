using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vosk;

namespace VCTUtility {
    public class VoskManager {

        private static float sampleRate = 16000.0f;
        private static int bufferType = 4096;
        
        public static void ByteBuffer(Model model) {
            VoskRecognizer rec = new VoskRecognizer(model, sampleRate);
            using(Stream source = File.OpenRead("test.wav")) { //Måste göras annorlunda då vår test inte kommer vara i test.wav
                byte[] buffer = new byte[bufferType];
                int bytesRead;

                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0) {
                    if (rec.AcceptWaveform(buffer, bytesRead)) {
                        Console.WriteLine(rec.Result()); //Måste göras annorlunda
                    } else {
                        Console.WriteLine(rec.PartialResult());
                    }
                }
            }
            Console.WriteLine(rec.FinalResult());
        }

        public static void FloatArray(Model model) {
            VoskRecognizer rec = new VoskRecognizer(model, sampleRate);
            using (Stream source = File.OpenRead("test.wav")) {
                byte[] buffer = new byte[bufferType];
                int bytesRead;

                while((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0) {
                    float[] fbuffer = new float[bytesRead / 2];
                    for (int i = 0, n = 0; i < fbuffer.Length; i++, n+=2) {
                        fbuffer[i] = (short)(buffer[n] | buffer[n + 1] << 8);
                    }

                    if (rec.AcceptWaveform(fbuffer, fbuffer.Length)) {
                        Console.WriteLine(rec.Result());
                    } else {
                        Console.WriteLine(rec.PartialResult());
                    }
                } 
            }
            Console.WriteLine(rec.FinalResult());
        }

        public static void DemoSpeaker(Model model) {
            SpkModel spkModel = new SpkModel("model-spk");
            VoskRecognizer rec = new VoskRecognizer(model, spkModel, sampleRate);

            using (Stream source = File.OpenRead("test.wav")) {
                byte[] buffer = new byte[bufferType];
                int bytesRead;

                while((bytesRead = source.Read(buffer, 0 , buffer.Length)) > 0) {
                    if (rec.AcceptWaveform(buffer, bytesRead)) {
                        Console.WriteLine(rec.Result());
                    } else {
                        Console.WriteLine(rec.PartialResult());
                    }
                }
            }
            Console.WriteLine(rec.FinalResult());
        }

        public static void Test() {
            Model model = new Model("model");
            ByteBuffer(model);
            Console.WriteLine("-----");
            FloatArray(model);
            Console.WriteLine("-----");
            DemoSpeaker(model);
        }
        public bool Recogniser() {
            return true;
        }
    }
}
