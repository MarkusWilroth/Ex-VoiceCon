using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace VCTUtility {
    public class AudioManager {
        private MemoryStream rec, lastRec;
        private WaveInEvent waveIn;
        private WaveOut waveOut;

        public void StartRecording() {
            waveIn = new WaveInEvent() { //Skapar WaveIn och sätter in rätt inställningar
                WaveFormat = new WaveFormat(44100, 16, 1)
            };

            rec = new MemoryStream();
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(WaveDataAvailable);
            waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(WaveRecordStopped);

            waveIn.StartRecording();
        }

        private void WaveDataAvailable(object sender, WaveInEventArgs e) {
            if (rec != null) {
                rec.Write(e.Buffer, 0, e.BytesRecorded);
                rec.Flush();
            }
        }

        private void WaveRecordStopped(object sender, StoppedEventArgs e) {
            if (waveIn != null) {

            }

            if (rec != null) {
                rec.Dispose();
                rec = null;
            }
        }

        public MemoryStream StopRecording() {
            lastRec = rec;
            waveIn.StopRecording();
            return lastRec;
        }

        public void PlayInput(MemoryStream rec) {
            rec.Dispose();
            waveOut = new WaveOut();
            RawSourceWaveStream RSS = new RawSourceWaveStream(rec, waveIn.WaveFormat);
            RSS.Position = 0;

            OffsetSampleProvider OSP = new OffsetSampleProvider(RSS.ToSampleProvider());
            OSP.SkipOver = TimeSpan.FromMilliseconds(0);
            OSP.Take = TimeSpan.FromMilliseconds(3000);

            waveOut.Init(OSP);
            waveOut.Play();
        }
    }
}
