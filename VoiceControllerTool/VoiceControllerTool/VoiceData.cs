using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceControllerTool {
    [Serializable]
    class VoiceData {

        public string voiceTitle, voiceText;
        
        public VoiceData(string voiceTitle, string voiceText) {
            this.voiceTitle = voiceTitle;
            this.voiceText = voiceText;
        }
    }
}
