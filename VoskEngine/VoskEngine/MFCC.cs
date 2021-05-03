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

        //General fields
        private int windowSize;
        private int hopSize;
        private int sampleRate;
        private double baseFreq;

        //Fields concerning mel filter banks
        private double minFreq;
        private double maxFreq; 
        private int numberOfFilters;

        //Fields concerning MFCC settings
        private int numberOfCoefficients;
        private bool useFirstCoefficient;

        //implementation details
        private double[] inputData;
        private double[] buffer;
        public Matrix dctMatrix;
        public Matrix melFilterBanks;
        private Complex[] samples;
        


        int[] filterPoints = { 6,18,31,46,63,82,103,127,154,184,218,
                              257,299,348,402,463,531,608,695,792,901,1023}; //array of anchor points for filtering the frame spectrum

        double[,] H = new double[20, 1024]; //array of 20 filters for each MFCC

        int numSamples = 1000; //remove
        


        //sampleRate - float sampes per second, must be greater than zero; not wohle-numbered values get rounded
        //windowSize - int size of window; must be 2^n and at least 32
        //numberCoefficients - int must be grate or equal to 1 and smaller than the number of filters
        //useFirstCoefficient - boolean indicates whether the first coefficient of the dct process should be used in the mfcc feature vector or not
            //  The zero-order coefficient indicates the average power of the input signal.
            //  The first-order coefficient represents the distribution
            //  spectral energy between low and high frequencies.
        //minFreq - double start of the interval to place the mel-filters in
        //maxFreq - double end of the interval to place the mel-filters in
        //numberFilters - int number of mel-filters to place in the interval

        public MFCC(int _sampleRate, int _windowSize, int _numberOfCoefficients, bool _useFirstCoefficient, double _minFreq, double _maxFreq, int _numberFilters)
        {
            //Assign the values to the local mfcc obj
            if (_sampleRate < 1) {
                throw new ArgumentException("Sample rate must be greater than 1");
            }
            else if (_windowSize == null) {
                throw new ArgumentException("window size has to be assigned");
            }
            else if (_numberOfCoefficients < 1 || _numberOfCoefficients >= _numberFilters) {
                throw new ArgumentException("The number of coefficients has to be greater or equal to 1, and be smaller than the number of filters");
            }
            else if (_minFreq >= 0 || _minFreq > _maxFreq || _maxFreq > 88200.0f) {
                throw new ArgumentException("The " + minFreq.ToString() + " hasn't been assigned");
            }         
            else if (_numberFilters < 2 || _numberFilters > (windowSize / 2) + 1) {
                throw new ArgumentException("Number filters must be at least 2 and smaller than the nyquist frequency");
            }

            this.sampleRate = _sampleRate;
            this.windowSize = _windowSize;
            this.numberOfCoefficients = _numberOfCoefficients;
            this.useFirstCoefficient = _useFirstCoefficient;
            this.minFreq = _minFreq;
            this.maxFreq = _maxFreq;
            this.numberOfFilters = _numberFilters;

            this.hopSize = windowSize / 2; //50% Overleap

            //create buffers
            inputData = new double[windowSize];
            buffer = new double[windowSize];

            melFilterBanks = GetFilterBanks();

            dctMatrix = GetDCTMatrix();

            Fourier.Forward(buffer, null, FourierOptions.NoScaling);

            //create power fft object
            samples = new Matrix(buffer, windowSize);  //FFT.FFT_NORMALIZED_POWER, windowSize, FFT.WND_HANNING


            //Fourier.Forward(samples, FourierOptions.NoScaling); //turn the samples from a time domain to a frequence domain

            //double[] input = Window.Hann(windowSize);

            for (int i = 0; i < samples.Length; i++)
            {
                //MathNet.Numerics.Window.Hann(/*window with*/);
                double norm = samples[i].NormOfDifference(samples[i].Magnitude);          
            }        
        }


        private double[] GetFilterBankBounderies(double _minFreq, double _maxFreq, int _numberOfFilters)
        {
            //create return array
            double[] centers = new double[_numberOfFilters + 2];
            double maxFreqMel, minFreqMel, deltaFreqMel, nextCenterMel;
            double maxFreq2; //Check to see which conversion that yields most accurate data 
                             
            //Convert the samples of hz to mel scale                
            //mel= 1127.01048 * log(f/700 +1) //ver:1                 Hz -> mel
            //mel= (1000/log(2))(log(f/1000+1)) //ver:2
            //f = 700 * (exp(mel / 1127.01048) - 1)  mel -> Hz

            maxFreqMel = 1127.01048 * Math.Log(_maxFreq / 700 + 1);
            maxFreq2 = (1000 / Math.Log(2)) * (Math.Log(_maxFreq / 1000 + 1));

            minFreqMel = 1127.01048 * Math.Log(_minFreq / 700 + 1);

            deltaFreqMel = (maxFreqMel - minFreqMel) / (_numberOfFilters + 1);

            //create (numberFilters + 2) equidistant points for the triangles
            nextCenterMel = minFreqMel;

            for (int i = 0; i < centers.Length; i++) {
                //transform the points back to linear scale
                centers[i] = 700 * (Math.Exp(nextCenterMel / 1127.01048) - 1);
                nextCenterMel += deltaFreqMel;
            }
            //ajust boundaries to exactly fit the given min./max. frequency
            centers[0] = minFreq;
            centers[_numberOfFilters + 1] = maxFreq;

            return centers;
        }

        private Matrix GetFilterBanks()
        {
            double[] boundaries = GetFilterBankBounderies(minFreq, maxFreq, numberOfFilters);


            //ignore filters outside of spectrum
            for (int i = 1; i < boundaries.Length - 1; i++) {
                if (boundaries[i] > sampleRate / 2) {
                    numberOfFilters = i - 1;
                    break;
                }
            }

            //create the filter bank matrix
            double[][] matrix = new double[numberOfFilters][];

            //fill each row of the filter bank matrix with one triangular mel filter
            for (int i = 1; i <= numberOfFilters; i++) {
                double[] filter = new double[(windowSize / 2) + 1];

                //for each frequency of the fft
                for (int j = 0; j < filter.Length; j++) {
                    //compute the filter weight of the current triangular mel filter
                    double freq = baseFreq * j;
                    filter[j] = GetMelFilterWeight(i, freq, boundaries);
                }

                //add the computed mel filter to the filter bank
                matrix[i - 1] = filter;
            }           
            //return the filter bank
            return new Matrix(matrix, numberOfFilters, (windowSize / 2) + 1); //bygg matrix classen som kan hantera att man skickar in martrisvektorerna och storleken
        }


        /// <summary>
		/// Returns the filter weight of a given mel filter at a given freqency.
		/// Mel-filters are triangular filters on the linear scale with an integral
		/// (area) of 1. However they are placed equidistantly on the mel scale, which
		/// is non-linear rather logarithmic.
		/// Consequently there are lots of high, thin filters at start of the linear
		/// scale and rather few and flat filters at the end of the linear scale.
		/// Since the start-, center- and end-points of the triangular mel-filters on
		/// the linear scale are known, the weigths are computed using linear
		/// interpolation.
		/// </summary>
		/// <param name="filterBank">int the number of the mel-filter, used to exract the
		///                       boundaries of the filter from the array</param>
		/// <param name="freq">double the frequency, at which the filter weight should be
		///                       returned</param>
		/// <param name="boundaries">double[] an array containing all the boundaries</param>
		/// <returns>double the filter weight</returns>
        private double GetMelFilterWeight(int filterbank, double frequency, double[] boundaries)
        {
            //for most frequencies the filter weight is 0
            double result = 0;

            //compute start- , center- and endpoint as well as the height of the filter
            double start = boundaries[filterbank - 1];
            double center = boundaries[filterbank];
            double end = boundaries[filterbank + 1];
            double height = 2.0d / (end - start);

            //is the frequency within the triangular part of the filter
            if (frequency >= start && frequency <= end) {
                //depending on frequencys position within the triangle
                if (frequency < center) {
                    //...use a ascending linear function
                    result = (frequency - start) * (height / (center - start));
                }
                else {
                    //..use a descending linear function
                    result = height + ((frequency - center) * (-height / (end - center)));
                }
            }
            return result;
        }

        private void Process(/*audio file*/)              //Add MFCC coefficent value as an atrribute to the *VoiceData
        {
            if (/*audio file is null*/true)
            {
                //Throw exeption 
            }

            //Else add to list?
        }

        //input - double[] input data is an array of samples in db SoundPreassureLevel (backgrund noise),
        //must be a multiple of the hop size, must not be a null value.
        //Hop siize = number of samples between each successive FFT window
        //double[][] an array of arrays contains a double array of Sone value for each window.
        //AKA the aucustion value or percieved loudness
        public double[][] Process(double[] input)
        {
            //check for null
            if (input == null)
                throw new Exception("input data must not be a null value");

            //check for correct array length
            if ((input.Length % hopSize) != 0) {
                double arrayLength = (double)input.Length / hopSize;
                arrayLength = Math.Round(arrayLength);
                int lenNew = (int)arrayLength * hopSize;
                Array.Resize<double>(ref input, lenNew);
                //throw new Exception("Input data must be multiple of hop size (windowSize/2).");
            }

            //create return array with appropriate size
            int len = (input.Length / hopSize) - 1;
            double[][] mfcc = new double[len][];

            for (int i = 0; i < len; i++) {
                mfcc[i] = new double[numberOfCoefficients];
            }

            //process each window of this audio segment
            for (int i = 0, pos = 0; pos < input.Length - hopSize; i++, pos += hopSize) {
                mfcc[i] = ProcessWindow(input, pos);
            }
            return mfcc;
        }

        /// <summary>
        /// Returns an int which represents the window size in [samples]
        /// </summary>
        /// <returns></returns>
        public int GetWindowSize()
        {
            return samples.Length; //
        }

        public Matrix GetDCTMatrix()
        {
            //compute constants
            double k = Math.PI / numberOfFilters;
            double w1 = 1.0 / (Math.Sqrt(numberOfFilters));
            double w2 = Math.Sqrt(2.0 / numberOfFilters);

            //create new matrix
            Matrix matrix = new Matrix(numberOfCoefficients, numberOfFilters);

            //generate dct matrix
            for (int i = 0; i < numberOfCoefficients; i++)
            {
                for (int j = 0; j < numberOfFilters; j++)
                {
                    if (i == 0)
                        matrix.Set(i, j, w1 * Math.Cos(k * i * (j + 0.5d)));
                    else
                        matrix.Set(i, j, w2 * Math.Cos(k * i * (j + 0.5d)));
                }
            }

            //adjust index if we are using first coefficient
            if (!useFirstCoefficient)
                matrix = matrix.GetMatrix(1, numberOfCoefficients - 1, 0, numberOfFilters - 1);

            return matrix;
        }

        /*Note from StackOverflow
        Many common (but not all) FFT libraries scale the FFT result of a unit amplitude sinusoid by the length of the FFT. 
        This maintains Parsevals equality since a longer sinusoid represents more total energy than a shorter one of the same amplitude.
        If you don't want to scale by the FFT length when using one of these libraries, then divide by the length before computing the magnitude in dB.
         */

        /// <summary>
		/// Transforms one window of MFCCs. The following steps are
		/// performed:
		/// (1) normalized power fft with hanning window function
		/// (2) convert to Mel scale by applying a mel filter bank
		/// (3) convertion to db<br>
		/// (4) finally a DCT is performed to get the mfcc<br>
        /// 
		/// This process is mathematical identical with the process described in [1].
		/// </summary>
		/// <param name="_window">double[] data to be converted, must contain enough data for
		///                        one window</param>
		/// <param name="start">int start index of the window data</param>
		/// <returns>double[] the window representation in Sone</returns>
        public double[] ProcessWindow(double[] _window, int start)
        {
            //number of unique coefficients, and the rest are symmetrically redundant
            int fftSize = (windowSize / 2) + 1;

            //check start
            if (start < 0)
                throw new Exception("start must be a positive value");

            //check window size
            if (_window == null || _window.Length - start < windowSize)
                throw new Exception("the given data array must not be a null value and must contain data for one window");

            //just copy to buffer
            for (int j = 0; j < windowSize; j++)
                buffer[j] = _window[j + start];

            //perform power fft
            //samples.Transform(buffer, null); old
            Fourier.Forward(buffer, null,FourierOptions.NoScaling);

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
