using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

namespace VCTUtility {
    public static class SaveSystem {
        public static void Save(string path, List<VoiceData> voiceList) {
            SaveData saveData = new SaveData(voiceList);

            if (path != null) {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Create);
                formatter.Serialize(stream, saveData);
                stream.Close();
            }
        }

        public static SaveData Load(string path) {
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
