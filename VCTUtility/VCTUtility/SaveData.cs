﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCTUtility {
    [Serializable]
    public class SaveData {
        public List<VoiceData> voiceDataList;

        public SaveData(List<VoiceData> voiceList) {
            voiceDataList = voiceList;
        }
    }
}
