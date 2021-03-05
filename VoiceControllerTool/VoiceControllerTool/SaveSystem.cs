using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace VoiceControllerTool {
    static class SaveSystem {

        //private SaveData saveData;
        //string path;

        public static void SaveVoice(List<VoiceData> voiceList, string path) { //Sparar voiceData
            SaveData saveData = new SaveData(voiceList);

            if (path != null) {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Create);
                formatter.Serialize(stream, saveData);
                stream.Close();
            }
        }

        public static SaveData Load(string path) { //Öppnar en fil
            SaveData saveData = null;

            if (File.Exists(path)) {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                if (stream.Length != 0) {
                    saveData = formatter.Deserialize(stream) as SaveData;
                }

                stream.Close();
                return saveData;
            } else {
                Console.WriteLine("Voice file not found in " + path);
                return null;
            }
        }
    }
}
