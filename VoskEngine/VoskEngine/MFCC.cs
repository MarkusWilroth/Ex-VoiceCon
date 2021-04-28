using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;




namespace VoskEngine
{
    class MFCC
    {


        int winowSize;

        double[] frame; //one frame
        double[] frameMass; //an array of all frames of 2048 samples or 128 ms
        double[] frameMassFFT; //array of FFT results for all frames

        int[] filterPoints = { 6,18,31,46,63,82,103,127,154,184,218,
                              257,299,348,402,463,531,608,695,792,901,1023}; //array of anchor points for filtering the frame spectrum

        double[,] H = new double[20, 1024]; //array of 20 filters for each MFCC
        double[] mfcc = new double[20]; //MFCC array for a given speech sample

        int numSamples = 1000;
        int sampleRate = 2000;

        int RATE = 48000;
        int BufferSize = (int)Math.Pow(2, 13);

        Complex[] samples;

        //sampleRate - float sampes per second, must be greater than zero; not wohle-numbered values get rounded
        //windowSize - int size of window; must be 2^n and at least 32
        //numberCoefficients - int must be grate or equal to 1 and smaller than the number of filters
        //useFirstCoefficient - boolean indicates whether the first coefficient of the dct process should be used in the mfcc feature vector or not
        //minFreq - double start of the interval to place the mel-filters in
        //maxFreq - double end of the interval to place the mel-filters in
        //numberFilters - int number of mel-filters to place in the interval

        public MFCC(float sampleRate, int windowSize, int numberCoefficients, bool useFirstCoefficient, double minFreq, double maxFreq, int numberFilters)
        {
            //Assign the values to the local mfcc obj
            if (sampleRate == null)
            {
                throw new ArgumentException("The " + sampleRate.ToString() + " hasn't been assigned");
            }
            else if (windowSize == null)
            {
                throw new ArgumentException("The " + windowSize.ToString() + " hasn't been assigned");
            }
            else if (numberCoefficients == null)
            {
                throw new ArgumentException("The " + numberCoefficients.ToString() + " hasn't been assigned");
            }
            else if (useFirstCoefficient == null)
            {
                throw new ArgumentException("The " + useFirstCoefficient.ToString() + " hasn't been assigned");
            }
            else if (minFreq == null)
            {
                throw new ArgumentException("The " + minFreq.ToString() + " hasn't been assigned");
            }
            else if (maxFreq == null)
            {
                throw new ArgumentException("The " + maxFreq.ToString() + " hasn't been assigned");
            }
            else if (numberFilters == null) //or lesser than one etc, same for the rest
            {
                throw new ArgumentException("The " + numberFilters.ToString() + " hasn't been assigned");
            }

            samples = new Complex[numSamples];


            Fourier.Forward(samples, FourierOptions.NoScaling); //turn the samples from a time domain to a frequence domain

            for (int i = 0; i < samples.Length; i++)
            {
                //MathNet.Numerics.Window.Hann(/*window with*/);
                double norm = samples[i].NormOfDifference(samples[i].Magnitude);

                
                winowSize = 25; //ms
                double[] input = Window.Hann(winowSize); //replace dummy imput with the soundstream array of hz or bytes?
                //Convert the samples of hz to mel scale
                //mel= 1127.01048 * log(f/700 +1) //ver:1
                //mel= (1000/log(2))(log(f/1000+1)) //ver:2
            }


        }

        private void Process(/*audio file*/)              
        {
            if (/*audio file is null*/true)
            {
                //Throw exeption 
            }

            //Else add to list?
        }

        //input - double[] input data is an array of samples in db SoundPreassureLevel (backgrund noise), must be a multiple of the hop size, must not be a null value. Hop siize = number of samples between each successive FFT window
        //double[][] an array of arrays contains a double array of Sone value for each window. AKA the aucustion value or percieved loudness
        public double[][] Process(double[] input)
        {
            double[][] result = null; //fix

            return result;
        }

        /// <summary>
        /// Returns an int which represents the window size in [samples]
        /// </summary>
        /// <returns></returns>
        public int GetWindowSize()
        {
            return winowSize;
        }
        /*
         * Note from StackOverflow
        Many common (but not all) FFT libraries scale the FFT result of a unit amplitude sinusoid by the length of the FFT. 
        This maintains Parsevals equality since a longer sinusoid represents more total energy than a shorter one of the same amplitude.
        If you don't want to scale by the FFT length when using one of these libraries, then divide by the length before computing the magnitude in dB.

         */
        public double[] ProcessWindow(double[] _window, int start)
        {
            if (_window == null)
            {
                throw new ArgumentException("The " + _window.ToString() + " hasn't been assigned");
            }
            else if (start == null)
            {
                throw new ArgumentException("The " + start.ToString() + " hasn't been assigned");
            }


            return null;
        }

        /*
        Using a 2khz sampeling rate and making the signal 1000 samples in lenght.
        This means that the signal will last for 0,5seconds (1000/2000) and show 100 cycels

        NOTE: the FFT "bidirectional bandwith" is the same as the sampling frequency or, 2kHz. 
              Since there are 1000 samples, each sample represents 2kHz / 1kHz aka 2Hz. 

        The max frequency we can detect is 1/2 of the sampleing rate (1kHz), and that appears in
        the middle of the chart/ spectrum of those 1000 samples. Therefor the top 500 samples '
        are useless.

         */
        private void PlotWaveform(int secondHarm, int thirdHarm, double secondPH, double thiridPH)
        {

            double[] fundamental = Generate.Sinusoidal(numSamples, sampleRate, 60, 10.0);
            double[] second = Generate.Sinusoidal(numSamples, sampleRate, 120, secondHarm, 0.0, secondPH);
            double[] third = Generate.Sinusoidal(numSamples, sampleRate, 180, thirdHarm, 0.0, thiridPH);


            //Add waveforms together to create the composite waveform
            for (int i = 0; i < numSamples; i++)
            {
                samples[i] = new Complex(fundamental[i] + second[i] + third[i], 0);
            }
        }
    }
}
