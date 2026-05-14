using System;
using System.Drawing;
 
namespace Lab6
{
    public class GraphLogic
    {
        public int n { get; } = 11;
        public int[,] AdjMatrixDir { get; set; }
        public int[,] AdjMatrixUnDir { get; set; }
        public int[,] WeightMatrix { get; set; }
        public PointF[] vertices { get; set; }
 
        private int n3 = 1;
        private int n4 = 2;
 
        public void GenerateMatrix(int seed)
        {
            double k = 1.0 - n3 * 0.01 - n4 * 0.005 - 0.05;
 
            Random rand = new Random(seed);
            AdjMatrixDir = new int[n, n];
            AdjMatrixUnDir = new int[n, n];
 
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    double value = rand.NextDouble() * 2.0;
                    AdjMatrixDir[i, j] = (value * k >= 1.0) ? 1 : 0;
                }
 
            for (int i = 0; i < n; i++)
                AdjMatrixDir[i, i] = 0;
 
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (AdjMatrixDir[i, j] == 1 || AdjMatrixDir[j, i] == 1)
                        AdjMatrixUnDir[i, j] = 1;
 
            for (int i = 0; i < n; i++)
                AdjMatrixUnDir[i, i] = 0;
        }
 
        public void GenerateWeightMatrix(int seed)
        {
            Random rand = new Random(seed);
 
            double[,] B = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    B[i, j] = rand.NextDouble() * 2.0;
 
            int[,] C = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    C[i, j] = (int)Math.Ceiling(B[i, j] * 100.0 * AdjMatrixUnDir[i, j]);
 
            int[,] D = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    D[i, j] = C[i, j] > 0 ? 1 : 0;
 
            int[,] H = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    H[i, j] = (D[i, j] != D[j, i]) ? 1 : 0;
 
            WeightMatrix = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    int tr = (i < j) ? 1 : 0;
                    WeightMatrix[i, j] = (D[i, j] + H[i, j] * tr) * C[i, j];
                }
 
            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++)
                {
                    int w = Math.Max(WeightMatrix[i, j], WeightMatrix[j, i]);
                    WeightMatrix[i, j] = w;
                    WeightMatrix[j, i] = w;
                }
 
            for (int i = 0; i < n; i++)
                WeightMatrix[i, i] = 0;
        }
 
        public void CalculateLayout(float x, float y, float w, float h)
        {
            vertices = new PointF[n];
            for (int i = 0; i < n; i++)
            {
                if (i < 3)      vertices[i] = new PointF(x + i * (w / 3), y);
                else if (i < 6) vertices[i] = new PointF(x + w, y + (i - 3) * (h / 3));
                else if (i < 9) vertices[i] = new PointF(x + w - (i - 6) * (w / 3), y + h);
                else            vertices[i] = new PointF(x, y + h - (i - 9 + 1) * (h / 3));
            }
        }
    }
}