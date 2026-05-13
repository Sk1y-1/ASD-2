using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Lab3
{
    public class GraphForm : Form
    {
        private GraphLogic _logic;

        public GraphForm(GraphLogic logic)
        {
            this.Size = new Size(1100, 700);
            this.Text = "GraphTypes";
            TabControl tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;

            TabPage dirTab = new TabPage("Directed Graph");
            GraphPanel dirPanel = new GraphPanel(logic, true);
            dirPanel.Dock = DockStyle.Fill;
            dirTab.Controls.Add(dirPanel);

            TabPage undirTab = new TabPage("Undirected Graph");
            GraphPanel undirPanel = new GraphPanel(logic, false);
            undirPanel.Dock = DockStyle.Fill;
            undirTab.Controls.Add(undirPanel);

            tabs.TabPages.Add(dirTab);
            tabs.TabPages.Add(undirTab);
            this.Controls.Add(tabs);
        }

        public class GraphPanel : Panel
        {
            private GraphLogic _logic;
            private bool _Directed;

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
            Pen p = new Pen(Color.Black, 1.2f);
            if (_Directed) 
            p.CustomEndCap = new AdjustableArrowCap(5, 5);
        else 
            p.EndCap = LineCap.Flat;

            int [,] matrix = _Directed ? _logic.AdjMatrixDir : _logic.AdjMatrixUnDir;

            for (int i = 0; i < _logic.n; i++)
            {
                for (int j = 0; j < _logic.n; j++)
                {
                    if (matrix [i, j] == 1)
                    {
                        DrawEdge(g, p, _logic.vertices[i], _logic.vertices[j], i == j);
                    }
                }
            }

            float r = 25f;
            for (int i = 0; i < _logic.n; i++)
            {
                var pt = _logic.vertices[i];
                g.FillEllipse(Brushes.White, pt.X - r, pt.Y - r, r * 2, r * 2);
                g.DrawEllipse(Pens.Black, pt.X - r, pt.Y - r, r * 2, r * 2);
                g.DrawString((i + 1).ToString(), this.Font, Brushes.Black, pt.X - 7, pt.Y - 7);
            }
        }
        

        

        private void DrawEdge(Graphics g, Pen p, PointF p1, PointF p2, bool loop)
        {
            float r = 25f;
            if (loop) g.DrawArc(p, p1.X - r, p1.Y - r * 1.5f, r, r, 0, 320); 
            else
            {
        float dx = p2.X - p1.X;
        float dy = p2.Y - p1.Y;
        
        float dist = (float)Math.Sqrt(dx * dx + dy * dy);
        
        float startX = p1.X + (dx / dist) * r;
        float startY = p1.Y + (dy / dist) * r;

        float endX = p2.X - (dx / dist) * r;
        float endY = p2.Y - (dy / dist) * r;
        g.DrawLine(p, new PointF(startX, startY), new PointF(endX, endY));
                } 
            }
        }
    }
}

