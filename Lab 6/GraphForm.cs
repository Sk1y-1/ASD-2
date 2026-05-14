using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace Lab6
{
    public class GraphForm : Form
    {
        private GraphLogic _logic;
        private KruskalMST _kruskal;

        public GraphForm(GraphLogic logic)
        {
            _logic = logic;
            _kruskal = new KruskalMST(logic.WeightMatrix, logic.n);

            this.Size = new Size(1200, 820);
            this.Text = " Minimum Spanning Tree (Kruskal's Algorithm)";
            this.BackColor = Color.FromArgb(245, 245, 248);

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                Padding = new Padding(8)
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110f));

            GroupBox leftBox = new GroupBox
            {
                Text = "Weighted Graph",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Margin = new Padding(4)
            };
            GraphPanel graphPanel = new GraphPanel(logic, _kruskal, false) { Dock = DockStyle.Fill };
            leftBox.Controls.Add(graphPanel);

            GroupBox rightBox = new GroupBox
            {
                Text = "Minimum Spanning Tree (MST)",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Margin = new Padding(4)
            };
            GraphPanel mstPanel = new GraphPanel(logic, _kruskal, true) { Dock = DockStyle.Fill };
            rightBox.Controls.Add(mstPanel);

            Panel controlPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(230, 232, 240),
                Margin = new Padding(4)
            };
            mainLayout.SetColumnSpan(controlPanel, 2);

            Button btnNext = new Button
            {
                Text = "Next Step",
                Left = 16, Top = 16,
                Width = 200, Height = 50,
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(60, 120, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnNext.FlatAppearance.BorderSize = 0;

            Button btnReset = new Button
            {
                Text = " Reset",
                Left = 230, Top = 16,
                Width = 140, Height = 50,
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(180, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnReset.FlatAppearance.BorderSize = 0;

            Label lblStatus = new Label
            {
                Text = "Press \"Next Step\" to start Kruskal's algorithm",
                Left = 390, Top = 20,
                Width = 760, Height = 40,
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(40, 40, 80)
            };

            Label lblEdgeList = new Label
            {
                Text = "",
                Left = 390, Top = 60,
                Width = 760, Height = 40,
                Font = new Font("Arial", 9),
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            btnNext.Click += (s, e) =>
            {
                if (_kruskal.Done)
                {
                    MessageBox.Show(
                        $"Kruskal's Algorithm Complete!\n\nMST Edges:\n{GetMSTEdgesText()}\n\nTotal Weight: {_kruskal.TotalWeight}",
                        "MST Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _kruskal.Step();
                lblStatus.Text = _kruskal.GetStatusText();
                lblEdgeList.Text = GetMSTEdgesShort();

                graphPanel.Invalidate();
                mstPanel.Invalidate();

                if (_kruskal.Done)
                {
                    btnNext.Text = "Done! View MST";
                    btnNext.BackColor = Color.FromArgb(40, 160, 80);
                }
            };

            btnReset.Click += (s, e) =>
            {
                _kruskal = new KruskalMST(_logic.WeightMatrix, _logic.n);
                graphPanel.UpdateKruskal(_kruskal);
                mstPanel.UpdateKruskal(_kruskal);
                lblStatus.Text = "Reset. Press \"Next Step\" to start Kruskal's alg";
                lblEdgeList.Text = "";
                btnNext.Text = " Next Step";
                btnNext.BackColor = Color.FromArgb(60, 120, 200);
                graphPanel.Invalidate();
                mstPanel.Invalidate();
            };

            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Right)
                    btnNext.PerformClick();
            };

            controlPanel.Controls.Add(btnNext);
            controlPanel.Controls.Add(btnReset);
            controlPanel.Controls.Add(lblStatus);
            controlPanel.Controls.Add(lblEdgeList);

            mainLayout.Controls.Add(leftBox, 0, 0);
            mainLayout.Controls.Add(rightBox, 1, 0);
            mainLayout.Controls.Add(controlPanel, 0, 1);

            this.Controls.Add(mainLayout);
        }

        private string GetMSTEdgesText()
        {
            var lines = new List<string>();
            foreach (var (u, v, w) in _kruskal.MSTEdges)
                lines.Add($"  ({u + 1} — {v + 1})  weight ={w}");
            return string.Join("\n", lines);
        }

        private string GetMSTEdgesShort()
        {
            if (_kruskal.MSTEdges.Count == 0) return "";
            var parts = new List<string>();
            foreach (var (u, v, w) in _kruskal.MSTEdges)
                parts.Add($"{u + 1}-{v + 1}({w})");
            return "MST edges: " + string.Join("  ", parts);
        }

        public class GraphPanel : Panel
        {
            private GraphLogic _logic;
            private KruskalMST _kruskal;
            private bool _mstOnly;

            public GraphPanel(GraphLogic logic, KruskalMST kruskal, bool mstOnly)
            {
                _logic = logic;
                _kruskal = kruskal;
                _mstOnly = mstOnly;
                this.DoubleBuffered = true;
            }

            public void UpdateKruskal(KruskalMST k) => _kruskal = k;

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                float margin = 60f;
                _logic.CalculateLayout(margin, margin, this.Width - margin * 2, this.Height - margin * 2);

                if (_mstOnly)
                    DrawMSTGraph(g);
                else
                    DrawFullGraph(g);
            }

            private void DrawFullGraph(Graphics g)
            {
                var mstSet = new HashSet<(int, int)>();
                var rejSet = new HashSet<(int, int)>();
                foreach (var (u, v, _) in _kruskal.MSTEdges) { mstSet.Add((u, v)); mstSet.Add((v, u)); }
                foreach (var (u, v, _) in _kruskal.RejectedEdges) { rejSet.Add((u, v)); rejSet.Add((v, u)); }

                (int, int) lastEdge = (-1, -1);
                if (_kruskal.CurrentEdgeIndex > 0 && _kruskal.CurrentEdgeIndex <= _kruskal.SortedEdges.Count)
                {
                    var (lu, lv, _) = _kruskal.SortedEdges[_kruskal.CurrentEdgeIndex - 1];
                    lastEdge = (lu, lv);
                }

                Font weightFont = new Font("Arial", 8, FontStyle.Bold);

                for (int i = 0; i < _logic.n; i++)
                    for (int j = i + 1; j < _logic.n; j++)
                    {
                        if (_logic.WeightMatrix[i, j] == 0) continue;

                        bool isMST = mstSet.Contains((i, j));
                        bool isRej = rejSet.Contains((i, j));
                        bool isCurrent = (lastEdge == (i, j) || lastEdge == (j, i));

                        Color edgeColor;
                        float edgeWidth;
                        if (isCurrent && !_kruskal.Done)
                        { edgeColor = Color.Orange; edgeWidth = 3f; }
                        else if (isMST)
                        { edgeColor = Color.FromArgb(30, 160, 60); edgeWidth = 2.5f; }
                        else if (isRej)
                        { edgeColor = Color.FromArgb(180, 180, 180); edgeWidth = 1f; }
                        else
                        { edgeColor = Color.FromArgb(100, 100, 120); edgeWidth = 1.2f; }

                        using var pen = new Pen(edgeColor, edgeWidth);
                        DrawEdgeWithWeight(g, pen, weightFont, _logic.vertices[i], _logic.vertices[j],
                            _logic.WeightMatrix[i, j], edgeColor);
                    }

                DrawVertices(g);
                weightFont.Dispose();
            }

            private void DrawMSTGraph(Graphics g)
            {
                Font weightFont = new Font("Arial", 8, FontStyle.Bold);

                foreach (var (u, v, w) in _kruskal.MSTEdges)
                {
                    using var pen = new Pen(Color.FromArgb(30, 160, 60), 2.8f);
                    DrawEdgeWithWeight(g, pen, weightFont, _logic.vertices[u], _logic.vertices[v], w,
                        Color.FromArgb(30, 160, 60));
                }

                var mstVerts = new HashSet<int>();
                foreach (var (u, v, _) in _kruskal.MSTEdges) { mstVerts.Add(u); mstVerts.Add(v); }
                DrawVertices(g, mstVerts);

                weightFont.Dispose();
            }

            private void DrawEdgeWithWeight(Graphics g, Pen pen, Font font,
                PointF p1, PointF p2, int weight, Color textColor)
            {
                float r = 22f;
                float dx = p2.X - p1.X, dy = p2.Y - p1.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                if (dist < 1) return;

                float ox = dx / dist * r, oy = dy / dist * r;
                g.DrawLine(pen, p1.X + ox, p1.Y + oy, p2.X - ox, p2.Y - oy);

                PointF mid = new PointF((p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);
                string wStr = weight.ToString();
                SizeF sz = g.MeasureString(wStr, font);
                g.FillEllipse(Brushes.White, mid.X - sz.Width / 2 - 2, mid.Y - sz.Height / 2 - 1,
                    sz.Width + 4, sz.Height + 2);
                using var brush = new SolidBrush(textColor);
                g.DrawString(wStr, font, brush, mid.X - sz.Width / 2, mid.Y - sz.Height / 2);
            }

            private void DrawVertices(Graphics g, HashSet<int> highlighted = null)
            {
                float r = 22f;
                Font numFont = new Font("Arial", 10, FontStyle.Bold);

                for (int i = 0; i < _logic.n; i++)
                {
                    var pt = _logic.vertices[i];
                    bool hi = highlighted != null && highlighted.Contains(i);

                    using var shadow = new SolidBrush(Color.FromArgb(30, 0, 0, 0));
                    g.FillEllipse(shadow, pt.X - r + 2, pt.Y - r + 2, r * 2, r * 2);

                    Color fill = hi ? Color.FromArgb(180, 230, 180) : Color.FromArgb(240, 240, 255);
                    using var fillBrush = new SolidBrush(fill);
                    g.FillEllipse(fillBrush, pt.X - r, pt.Y - r, r * 2, r * 2);

                    using var borderPen = new Pen(hi ? Color.FromArgb(30, 160, 60) : Color.FromArgb(80, 80, 140), 2f);
                    g.DrawEllipse(borderPen, pt.X - r, pt.Y - r, r * 2, r * 2);

                    string label = (i + 1).ToString();
                    SizeF sz = g.MeasureString(label, numFont);
                    g.DrawString(label, numFont, Brushes.Black, pt.X - sz.Width / 2, pt.Y - sz.Height / 2);
                }

                numFont.Dispose();
            }
        }
    }
}