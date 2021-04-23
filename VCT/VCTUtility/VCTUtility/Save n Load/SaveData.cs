using System;
using System.Collections.Generic;
using System.Text;

namespace VCTUtility {
    [Serializable]
    public class SaveData {
        public List<VoiceData> voiceDataList;

        public SaveData(List<VoiceData> voiceList) {
            voiceDataList = voiceList;
        }
    }
}
