using System;
using System.Collections.Generic;
using System.Text;

namespace VoskEngine
{
    class Matrix
    {
        public int Rows { get; private set; }
        public int Colums { get; private set; }


        public Matrix (double[] [] matrixArray, int r, int c) // r = rows, c = colums
        {
            if (matrixArray.Length > r * c)
            {
                throw new ArgumentException("Invalid matrix array length");
            }


        }
    }
}
