using System;
using System.Windows.Forms;
using System.Drawing;
namespace Lab4 {

public class GraphLogic
{
        public virtual int n {get;} = 11;
        public int[,]? AdjMatrixDir {get; protected set;}
        public int[,]? AdjMatrixUnDir {get; protected set;}
        public PointF[]? vertices {get; private set;}

        public void GenerateMatrix(int seed)
        {
            const int n3 = 1;
            const int n4 = 2;

               double k = 1.0 - n3 * 0.01 - n4 * 0.01 - 0.3;

                
                Random rand = new Random(seed);
                AdjMatrixDir = new int[n, n];
                AdjMatrixUnDir = new int[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                      double value = rand.NextDouble() * 2;
                      AdjMatrixDir[i, j] = (int)Math.Floor(value * k);
                    
                        }
                    }

                    for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (AdjMatrixDir[i, j] == 1)
                        {
                            AdjMatrixUnDir[i, j] = 1;
                            AdjMatrixUnDir[j, i] = 1;
                        }
                    } 
                }
            }
        public void CalculateLayout(float x, float y, float w, float h)
        {
            vertices = new PointF[n];
            for (int i = 0; i < n; i++)
            {
                if (i < 3) vertices[i] = new PointF(x + i * (w / 3), y);
                else if (i < 6) vertices[i] = new PointF(x + w, y + (i - 3) * (h / 3));
                else if (i < 9) vertices[i] = new PointF(x + w - (i - 6) * (w / 3), y + h);
                else vertices[i] = new PointF(x, y + h - (i - 9 + 1) * (h / 3));
            }
        }
    }
}
