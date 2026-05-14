using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System;
 
namespace Lab5
{
    public class GraphForm : Form
    {
        private GraphLogic _logic;
 
        public GraphForm(GraphLogic logic)
        {
            _logic = logic;
            this.Size = new Size(1100, 750);
            this.Text = "Graphs Traversal Visualization";
 
            TabControl tabs = new TabControl { Dock = DockStyle.Fill };
 
            // ── BFS Tab ──────────────────────────────────────────────
            var bfs = new GraphTraversal(logic.AdjMatrixDir, logic.n);
 
            TabPage dirTab = new TabPage("Directed BFS");
            GraphPanel dirPanel = new GraphPanel(logic, true) { Dock = DockStyle.Fill };
 
            Panel bfsControlPanel = new Panel { Dock = DockStyle.Bottom, Height = 100, BackColor = Color.LightGray };
            Button btnBfsNext = new Button {
                Text = "Next step (BFS)",
                Left = 20, Top = 20,
                Width = 180, Height = 45,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            Label lblBfsResult = new Label {
                Text = "BFS order: ",
                Left = 220, Top = 30,
                Width = 800,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
 
            bool bfsDone = false;
            btnBfsNext.Click += (s, e) => {
                if (bfsDone) return;
                if (bfs.StepBFS(logic.n))
                {
                    dirPanel.CurrentTraversal = bfs;
                    lblBfsResult.Text = "BFS order: " + string.Join(" -> ", bfs.Order);
                    dirPanel.Invalidate();
                }
                else
                {
                    bfsDone = true;
                    btnBfsNext.Enabled = false;
                    MessageBox.Show("BFS traversal completed!");
                }
            };
 
            bfsControlPanel.Controls.Add(btnBfsNext);
            bfsControlPanel.Controls.Add(lblBfsResult);
 
            dirTab.Controls.Add(dirPanel);
            dirTab.Controls.Add(bfsControlPanel);
            tabs.TabPages.Add(dirTab);
 
            // ── DFS Tab ──────────────────────────────────────────────
            var dfs = new GraphTraversal(logic.AdjMatrixDir, logic.n);
 
            TabPage dfsTab = new TabPage("Directed DFS");
            GraphPanel dfsPanel = new GraphPanel(logic, true) { Dock = DockStyle.Fill };
 
            Panel dfsControlPanel = new Panel { Dock = DockStyle.Bottom, Height = 100, BackColor = Color.LightGray };
            Button btnDfsNext = new Button {
                Text = "Next step (DFS)",
                Left = 20, Top = 20,
                Width = 180, Height = 45,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            Label lblDfsResult = new Label {
                Text = "DFS order: ",
                Left = 220, Top = 30,
                Width = 800,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
 
            bool dfsDone = false;
            btnDfsNext.Click += (s, e) => {
                if (dfsDone) return;
                if (dfs.StepDFS(logic.n))
                {
                    dfsPanel.CurrentTraversal = dfs;
                    lblDfsResult.Text = "DFS order: " + string.Join(" -> ", dfs.Order);
                    dfsPanel.Invalidate();
                }
                else
                {
                    dfsDone = true;
                    btnDfsNext.Enabled = false;
                    MessageBox.Show("DFS traversal completed!");
                }
            };
 
            dfsControlPanel.Controls.Add(btnDfsNext);
            dfsControlPanel.Controls.Add(lblDfsResult);
 
            dfsTab.Controls.Add(dfsPanel);
            dfsTab.Controls.Add(dfsControlPanel);
            tabs.TabPages.Add(dfsTab);
 
            this.Controls.Add(tabs);
        }
 
        public class GraphPanel : Panel
        {
            private GraphLogic _logic;
            private bool _Directed;
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public GraphTraversal CurrentTraversal { get; set; }
 
            public GraphPanel(GraphLogic logic, bool directed)
            {
                _logic = logic;
                _Directed = directed;
                this.DoubleBuffered = true;
            }
 
            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
 
                int[,] matrix = _Directed ? _logic.AdjMatrixDir! : _logic.AdjMatrixUnDir!;
 
                Pen edgePen = new Pen(Color.Black, 1.2f);
                if (_Directed) edgePen.CustomEndCap = new AdjustableArrowCap(5, 5);
 
                for (int i = 0; i < _logic.n; i++)
                {
                    for (int j = 0; j < _logic.n; j++)
                    {
                        if (matrix[i, j] == 1)
                        {
                            DrawEdge(g, edgePen, _logic.vertices![i], _logic.vertices![j], i == j);
                        }
                    }
                }
 
                if (CurrentTraversal != null)
                {
                    Pen treePen = new Pen(Color.Red, 2.8f);
                    treePen.CustomEndCap = new AdjustableArrowCap(6, 6);
                    foreach (var edge in CurrentTraversal.TreeEdges)
                    {
                        DrawEdge(g, treePen, _logic.vertices![edge.Item1], _logic.vertices![edge.Item2], false);
                    }
                }
 
                float r = 25f;
                for (int i = 0; i < _logic.n; i++)
                {
                    var pt = _logic.vertices![i];
                    Brush b = (CurrentTraversal != null && CurrentTraversal.Visited[i])
                        ? Brushes.LightGreen
                        : Brushes.White;
 
                    g.FillEllipse(b, pt.X - r, pt.Y - r, r * 2, r * 2);
                    g.DrawEllipse(Pens.Black, pt.X - r, pt.Y - r, r * 2, r * 2);
                    g.DrawString((i + 1).ToString(), this.Font, Brushes.Black, pt.X - 7, pt.Y - 7);
                }
            }
 
            private void DrawEdge(Graphics g, Pen p, PointF p1, PointF p2, bool loop)
            {
                float r = 25f;
                if (loop)
                {
                    g.DrawArc(p, p1.X - r, p1.Y - r * 1.5f, r, r, 0, 320);
                }
                else
                {
                    float dx = p2.X - p1.X;
                    float dy = p2.Y - p1.Y;
                    float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                    if (dist == 0) return;

                    float ox = (dx / dist) * r;
                    float oy = (dy / dist) * r;

                    g.DrawLine(p, p1.X + ox, p1.Y + oy, p2.X - ox, p2.Y - oy);
                }
            }
        }
    }
}

