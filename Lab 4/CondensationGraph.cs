using System;
using System.Windows.Forms;

namespace Lab4
{
public class CondensationGraph : GraphLogic
{
    private readonly int _n;

    public override int n => _n;
    public CondensationGraph(int [,] condensationMatrix, int componentCount)
    {
        AdjMatrixDir = condensationMatrix;
        AdjMatrixUnDir = new int[componentCount, componentCount];
            _n = componentCount;


        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                if (AdjMatrixDir[i, j] == 1 && AdjMatrixDir[j, i] == 1)
                {
                    AdjMatrixUnDir[i, j] = 1;
                    AdjMatrixUnDir[j, i] = 1;
            }
        }
    }
}






