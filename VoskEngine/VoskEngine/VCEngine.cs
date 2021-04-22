using System;
using Vosk;
using System.IO;

namespace VoskEngine
{
    class VCEngine
    {
        

        public static void Main(string[] args)
        {
            Vosk.Vosk.SetLogLevel(0);


            var projecDir = Directory.GetParent(Directory.GetParent(@"../").Parent.Parent.FullName);
            var filePath = @"C:\Users\jasmi\source\repos\VoskEngine\VoskEngine\model\model-en\"; //This might have to be added as a resx path and then reffed that way.
            var path = Path.Combine(projecDir.FullName, filePath);

            Model model = new Model(path);


            DemoBytes(model);
        }

        /// <summary>
        /// To test that the engine works
        /// </summary>
        /// <param name="model"></param>
        public static void DemoBytes(Model model)
        {
            // Demo byte buffer
            VoskRecognizer rec = new VoskRecognizer(model, 16000.0f);
            using (Stream source = File.OpenRead("C:/Users/jasmi/source/repos/VoskEngine/VoskEngine/test.wav"))
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

        public void DetectKW(Model model)
        {
            VoskRecognizer rec = new VoskRecognizer(model, 16000.0f);

            //Capture recording, send it from the tool to the engine
            //using (Stream source = File.OpenRead(Au))
            //{

            //}
            
            
        }

    }
}
