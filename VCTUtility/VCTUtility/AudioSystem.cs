using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCTUtility {
    public static class AudioSystem {

        public static AudioManager audioManager = new AudioManager();

        public static void StartRecording() {
            audioManager.StartRecording();
        } 
        
        public static MemoryStream StopRecording() {
            return audioManager.StopRecording();
        }

        public static int GetSampleRate() {
            return audioManager.GetSampleRate();
        }

        public static int GetChannels() {
            return audioManager.GetChannels();
        }
    }
}
