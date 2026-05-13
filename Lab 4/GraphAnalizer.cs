using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4 {

public class GraphAnalyzer
{
    private GraphLogic _graph;
    public GraphAnalyzer(GraphLogic graph)
    {
        _graph = graph;
    }
public int[] CalculateDegreesUnDir()
    {
        
        int[] degrees = new int [_graph.n];
        for (int i = 0; i < _graph.n; i++)
        {
            for (int j = 0; j < _graph.n; j++)
            {
                degrees[i] += _graph.AdjMatrixUnDir![i, j];
            }
        }
        return degrees;
    }

    public (int[] degIn, int[] degOut, int[] deg) CalculateDegreesDir()
    {
        int[] degIn = new int[_graph.n];
        int[] degOut = new int[_graph.n];
        int[] deg = new int[_graph.n];
        for (int i = 0; i < _graph.n; i++)
        {
            for (int j = 0; j < _graph.n; j++)
            {
                degOut[i] += _graph.AdjMatrixDir![i, j];
                degIn[j] += _graph.AdjMatrixDir![i, j];
            }
        }
        for (int i = 0; i < _graph.n; i++)
        {
            deg[i] = degIn[i] + degOut[i];
        }
        return (degIn, degOut, deg);
    }
    public (bool isRegular, int degree) IsRegular(int[] degrees)
    {
        int first = degrees[0];
        foreach (int d in degrees)
            if (d != first) return (false, 0);
        return (true, first);
    }

    public (List<int> hanging, List<int> isolated) GetSpecialVertices(int[] degrees)
    {
        var hanging  = new List<int>();
        var isolated = new List<int>();

        for (int i = 0; i < _graph.n; i++)
        {
            if (degrees[i] == 1) hanging.Add(i + 1);
            if (degrees[i] == 0) isolated.Add(i + 1);
        }
    

        return (hanging, isolated);
        }
        private int[,] MultiplyMatrices(int[,] A, int[,] B)
{
            int[,] result = new int[_graph.n, _graph.n];
    for (int i = 0; i < _graph.n; i++)
        for (int j = 0; j < _graph.n; j++)
            for (int k = 0; k < _graph.n; k++) // k - Matrix multiplication index
                result[i, j] += A[i, k] * B[k, j];
    return result;
}
public int[,] GetPaths2()
{
    return MultiplyMatrices(_graph.AdjMatrixDir!, _graph.AdjMatrixDir!);
}
public int[,] GetPaths3()
{
    return MultiplyMatrices(GetPaths2(), _graph.AdjMatrixDir!);
}

public int[,] GetReachabilityMatrix()
{
    int[,] D = (int[,])_graph.AdjMatrixDir!.Clone(); // D - Distance 
    for (int k = 0; k < _graph.n; k++)
        for (int i = 0; i < _graph.n; i++)
            for (int j = 0; j < _graph.n; j++)
            if ( D[i, k] == 1 && D[k, j] == 1 )
                D[i, j] = 1;
    return D;
}

public int[,] GetStrongConnectivityMatrix()
{
    int[,] D = GetReachabilityMatrix();
    int[,] S = new int[_graph.n, _graph.n];

    for (int i = 0; i < _graph.n; i++)
        for (int j = 0; j < _graph.n; j++)
            S[i, j] = D[i, j] & D[j, i];
    return S;
}
public List<List<int>> GetStrongComponents()
        {
            bool[] visited = new bool[_graph.n];
            var components = new List<List<int>>();

    for (int i = 0; i < _graph.n; i++)
    {
        if (visited[i]) continue; 

        var component = new List<int>();
        for (int j = 0; j < _graph.n; j++)
        {
                    int[,] S = GetStrongConnectivityMatrix();
                    if (S[i, j] == 1)
                    {
                        component.Add(j + 1);
                        visited[j] = true;
                    }
                }
        components.Add(component);
    }
    return components;
}
public int[,] GetCondensationMatrix()
{
    var components = GetStrongComponents();
    int c = components.Count; 
    int[,] condensation = new int[c, c];

    for (int ci = 0; ci < c; ci++)
        for (int cj = 0; cj < c; cj++)
        {
            if (ci == cj) continue; 
            foreach (int u in components[ci])
                foreach (int v in components[cj])
                    if (_graph.AdjMatrixDir![u-1, v-1] == 1)
                        condensation[ci, cj] = 1;
        }
    return condensation;
            }
        }
    }



    


    
