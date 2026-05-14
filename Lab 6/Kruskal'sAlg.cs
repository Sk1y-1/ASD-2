using System;
using System.Collections.Generic;
using System.Linq;
 
namespace Lab6
{
    public class KruskalMST
    {
        private int _n;
        private int[] _parent;
        private int[] _rank;
 
        public List<(int U, int V, int Weight)> SortedEdges { get; private set; }
        public List<(int U, int V, int Weight)> MSTEdges { get; private set; }
        public List<(int U, int V, int Weight)> RejectedEdges { get; private set; }
        public int CurrentEdgeIndex { get; private set; }
        public bool Done { get; private set; }
        public int TotalWeight => MSTEdges.Count > 0
            ? MSTEdges.ConvertAll(e => e.Weight).Aggregate((a, b) => a + b)
            : 0;
 
        public KruskalMST(int[,] weightMatrix, int n)
        {
            _n = n;
            _parent = new int[n];
            _rank = new int[n];
            for (int i = 0; i < n; i++) { _parent[i] = i; _rank[i] = 0; }
 
            MSTEdges = new List<(int, int, int)>();
            RejectedEdges = new List<(int, int, int)>();
            CurrentEdgeIndex = 0;
            Done = false;
 
            SortedEdges = new List<(int, int, int)>();
            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++)
                    if (weightMatrix[i, j] > 0)
                        SortedEdges.Add((i, j, weightMatrix[i, j]));
 
            SortedEdges.Sort((a, b) => a.Weight.CompareTo(b.Weight));
        }
 
        private int Find(int x)
        {
            if (_parent[x] != x)
                _parent[x] = Find(_parent[x]);
            return _parent[x];
        }
 
        private bool Union(int x, int y)
        {
            int px = Find(x), py = Find(y);
            if (px == py) return false;
            if (_rank[px] < _rank[py]) { int tmp = px; px = py; py = tmp; }
            _parent[py] = px;
            if (_rank[px] == _rank[py]) _rank[px]++;
            return true;
        }
 
        public bool Step()
        {
            if (Done) return false;
 
            if (MSTEdges.Count == _n - 1)
            {
                Done = true;
                return false;
            }
 
            if (CurrentEdgeIndex >= SortedEdges.Count)
            {
                Done = true;
                return false;
            }
 
            var (u, v, w) = SortedEdges[CurrentEdgeIndex];
            CurrentEdgeIndex++;
 
            if (Union(u, v))
                MSTEdges.Add((u, v, w));
            else
                RejectedEdges.Add((u, v, w));
 
            if (MSTEdges.Count == _n - 1 || CurrentEdgeIndex >= SortedEdges.Count)
                Done = true;
 
            return true;
        }
 
        public string GetStatusText()
        {
            if (Done)
                return $"Done! MST has {MSTEdges.Count} edges, total weight = {TotalWeight}";
 
            if (CurrentEdgeIndex == 0)
                return "Click \"Next Step\" to start Kruskal's alg";
 
            if (CurrentEdgeIndex > 0 && CurrentEdgeIndex <= SortedEdges.Count)
            {
                var (u, v, w) = SortedEdges[CurrentEdgeIndex - 1];
                bool accepted = MSTEdges.Count > 0 && MSTEdges[MSTEdges.Count - 1] == (u, v, w);
                string action = accepted ? "Added" : " Rejected (cycle)";
                return $"Step {CurrentEdgeIndex}:  ({u + 1} - {v + 1}, w={w}) - {action}  |  MST edges: {MSTEdges.Count}/{_n - 1}";
            }
 
            return "";
        }
    }
}