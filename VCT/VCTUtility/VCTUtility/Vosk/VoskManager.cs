using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Vosk;

namespace VCTUtility {
    public static class VoskManager { //Denna klass är vad VCT Går igenom

        private static float sampleRate = 16000.0f;

        public static void SetUp() {
            Vosk.Vosk.SetLogLevel(0);

            string projecDir = Environment.CurrentDirectory;
            string path = Path.Combine(Directory.GetParent(projecDir).Parent.FullName, "VCT-CommandBox.png");

            Model model = new Model(path);
        }

        //Vad gör denna klass? Är detta en engångs grej? Ska den bara hända när man startar?
        public static string DemoBytes(Model model, MemoryStream mStream) { 
            VoskRecognizer rec = new VoskRecognizer(model, sampleRate); //rec kan enkelt misstolkas som recording, annan förkortning?

            using (Stream stream = mStream) {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0) {
                    if (rec.AcceptWaveform(buffer, bytesRead)) {
                        Console.WriteLine(rec.Result());
                    } else {
                        Console.WriteLine(rec.PartialResult());
                    }
                }
                //Console.WriteLine(rec.FinalResult()); //Är detta för test? DLL kan inte riktigt skria ut saker? Return string?
                return rec.FinalResult();            
            }
        }
        
        public static void DetectKW(Model model, MemoryStream mStream) { //mStream är en rec från tool... ska vi använda den?
            VoskRecognizer rec = new VoskRecognizer(model, sampleRate);

            //Capture recording, send it from the tool to the engine
        }
    }
}
