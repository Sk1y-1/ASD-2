using System;
using System.Collections.Generic;

namespace Lab5
{
public class GraphTraversal
{
    private int[,] _matrix;
    public bool[] Visited;
    public List<int> QueueOrStack = new List<int>(); 
    public List<(int, int)> TreeEdges = new List<(int, int)>(); 
    public List<int> Order = new List<int>(); 

    public GraphTraversal(int[,] matrix, int n)
    {
        _matrix = matrix;
        Visited = new bool[n];
    }
    public bool StepBFS(int n)
    {

        if (QueueOrStack.Count == 0)
        {
            for (int i = 0; i < n; i++)
            {
                if (!Visited[i] && HasOutEdges(i, n))
                {
                    QueueOrStack.Add(i);
                    Visited[i] = true;
                    Order.Add(i + 1);
                    return true; 
                }
            }
            return false; 
        }

        int curr = QueueOrStack[0];
        QueueOrStack.RemoveAt(0);

        for (int v = 0; v < n; v++)
        {
            if (_matrix[curr, v] == 1 && !Visited[v])
            {
                Visited[v] = true;
                TreeEdges.Add((curr, v));
                Order.Add(v + 1);
                QueueOrStack.Add(v);
            }
        }
        return true;
    }

    public bool StepDFS(int n)
{
    if (QueueOrStack.Count == 0)
    {
        for (int i = 0; i < n; i++)
        {
            if (!Visited[i])
            {
                QueueOrStack.Add(i);
                Visited[i] = true;
                Order.Add(i + 1);
                return true;
            }
        }
        return false;
    }

    int curr = QueueOrStack[QueueOrStack.Count - 1];
    for (int v = 0; v < n; v++)
    {
        if (_matrix[curr, v] == 1 && !Visited[v])
        {
            Visited[v] = true;
            TreeEdges.Add((curr, v));
            Order.Add(v + 1);
            QueueOrStack.Add(v);
            return true;
        }
    }

    QueueOrStack.RemoveAt(QueueOrStack.Count - 1);
    
    if (QueueOrStack.Count > 0 || HasUnvisited(n)) 
    {
        return StepDFS(n); 
    }

    return false;
}

private bool HasUnvisited(int n)
{
    for (int i = 0; i < n; i++)
        if (!Visited[i]) 
        return true;
    return false;
}

    private bool HasOutEdges(int u, int n)
    {
        for (int v = 0; v < n; v++) if (_matrix[u, v] == 1) return true;
        return false;
    }
}
}