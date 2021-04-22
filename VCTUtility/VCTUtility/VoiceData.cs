using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCTUtility {
    [Serializable]
    public class VoiceData {
        public string voiceTitle, voiceText, voiceTag; //VoiceTitle är vad man ska söka på, Voice Text är vad den innehåller och VoiceTag är vad utvecklaren kommer använda (t.ex. forward)
        public MemoryStream rec; //Själva orginalljudfilen, avänds för Vosk och att spela upp (Om man skulle vilja det)

        public VoiceData(string voiceTitle, string voiceText, string voiceTag, MemoryStream rec) {
            this.voiceTitle = voiceTitle;
            this.voiceText = voiceText;
            this.voiceTag = voiceTag;
            this.rec = rec;
        }
    }
    /* ToDo:
     * - Fylla denna med relevant data som behövs
     */ 
}
