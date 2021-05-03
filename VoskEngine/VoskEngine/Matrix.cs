using System;
using System.Collections.Generic;
using System.Text;

namespace VoskEngine
{
    class Matrix
    {
        public int Rows { get; private set; }
        public int Colums { get; private set; }

        double[][] matrixValues; //prolly rename
        Matrix subMatrix;

        public Matrix(int r, int c)//Construct an "r" by "c" matrix filled with 0.0
        {
            matrixValues = new double[r][];

            for (int i = 0; i < matrixValues.GetLength(0); i++) {
                matrixValues[i] = new double[c];
            }
        }

        public Matrix(double[] values, int rows)
        {
            if (values.Length != rows) { throw new ArgumentException("Vector and rows must be atleast 1"); }
            matrixValues = new double[rows][];


            for (int i = 0; i < rows; i++) {
                matrixValues[i] = new double[rows];
                matrixValues[i] = values;
            }        
        }

        public Matrix (double[] [] matrixArray, int r, int c) // r = rows, c = colums
        {
            if (matrixArray.Length > r * c) {
                throw new ArgumentException("Invalid matrix array length"); //if we can't match up the rows and colums, break.
            }

            matrixValues = new double[r][];

            for (int i = 0; i < r; i++) //set sonnematrix to the size of the rows and colums (these will be added /removed depending on what operation is done)
            {
                matrixValues[i] = new double[c];
            }

            // looping through matrix 1 rows  
            for (int i = 0; i < matrixArray.GetLength(0); i++) {
                // for each matrix 1 row, loop through matrix 2 columns  
                for (int j = 0; j < matrixValues.GetLength(1); j++) {
                    // loop through matrix 1 columns to calculate the dot product  
                    for (int k = 0; k < matrixArray.GetLength(1); k++) {
                        matrixValues[i][j] += matrixArray[i][k] * matrixArray[k][i];
                    }
                }
            }
        }



        public Matrix GetMatrix(int r0, int r1, int c0, int c1) //r0 = row begning, r1 = row en, c0 colum begining, c1 colum end
        {
            //retrun submatrix from sent in index postiotions
            double[][] resultMatrix = new double[r1][];

            for (int i = 0; i < r1; i++) {
                Array.Copy(matrixValues[r0 + i], c0, resultMatrix[i], 0, c1);
            }

            subMatrix = new Matrix(resultMatrix, r1, c1);

            return subMatrix;
        }

		public Matrix Subtraction(Matrix matrix)
        {
            return null; //fix or remove
        }

        public void Set(int r, int c, double value) {
           matrixValues[r][c] = value;
        }
    }
}
