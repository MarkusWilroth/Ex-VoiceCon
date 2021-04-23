using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using System.IO;

namespace VCTUtility {
    class MFCC {

        double[] frame; //one frame
        double[] frameMass; //an array of all frames of 2048 samples or 128 ms
        double[] frameMassFFT; //array of FFT results for all frames

        int[] filterPoints = { 6,18,31,46,63,82,103,127,154,184,218,
                              257,299,348,402,463,531,608,695,792,901,1023}; //array of anchor points for filtering the frame spectrum

        int numSamples;
        float sampleRate;

        int RATE;
        int bufferSize = (int)Math.Pow(2, 13);

        Complex[] samples;

        public MFCC(float sampleRate, int numberCoefficients, bool useFirstCoefficient, double minFreq, double maxFreq, int numberFilters) {
            this.sampleRate = sampleRate;
            samples = new Complex[numSamples];

            Fourier.Forward(samples, FourierOptions.NoScaling);

            for (int i = 0; i < samples.Length; i++) {
                double norm = samples[i].NormOfDifference(samples[i].Magnitude);
            }
        }

        private void Process(MemoryStream mStream) {
           
        }

        public double[][] Process(double[] input) { //Ska input vara i formen av MemoryStream?
            double[][] result = null; //fix
            return result;
        }

        private void PlotWaveForm(int secondHarm, int thirdHarm, double secondPH, double thirdPH) {
            double[] fundamental = Generate.Sinusoidal(numSamples, sampleRate, 60, 10.0);
            double[] second = Generate.Sinusoidal(numSamples, sampleRate, 120, secondHarm, 0.0, secondPH);
            double[] third = Generate.Sinusoidal(numSamples, sampleRate, 180, thirdHarm, 0.0, thirdPH);


            //Add waveforms together to create the composite waveform
            for (int i = 0; i < numSamples; i++) {
                samples[i] = new Complex(fundamental[i] + second[i] + third[i], 0);
            }
        }
    }
}
