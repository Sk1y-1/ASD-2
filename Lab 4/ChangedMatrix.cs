using System;
using System.Drawing;

namespace Lab4
{
    public class ChangedMatrix : GraphLogic
    {
        public new void GenerateMatrix(int seed)
        {
            const int n3 = 1;
            const int n4 = 2;
            double k = 1.0 - n3 * 0.005 - n4 * 0.005 - 0.27;

            Random rand = new Random(seed);
            AdjMatrixDir = new int[n, n];
            AdjMatrixUnDir = new int[n, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    double value = rand.NextDouble() * 2;
                    AdjMatrixDir[i, j] = (int)Math.Floor(value * k);
                }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (AdjMatrixDir[i, j] == 1)
                    {
                        AdjMatrixUnDir[i, j] = 1;
                        AdjMatrixUnDir[j, i] = 1;
                    }
        }
    }
}
    
