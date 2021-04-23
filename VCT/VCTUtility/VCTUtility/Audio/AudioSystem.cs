using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace VCTUtility {
    public static class AudioSystem {
        public static AudioManager audioManager = new AudioManager();

        public static void StartRecording() {
            audioManager.StartRecording();
        }

        public static MemoryStream StopRecording() {
            return audioManager.StopRecording();
        }
    }
}
