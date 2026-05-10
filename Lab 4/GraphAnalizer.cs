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
                degrees[i] += _graph.AdjMatrixUnDir[i, j];
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
}
    


    
