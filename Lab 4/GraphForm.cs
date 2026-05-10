using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Lab4
{        
    public class GraphForm : Form
        {
        public GraphForm(GraphLogic logic)
    {
            this.Size = new Size(1100, 700);
            this.Text = "GraphTypes";
            SplitContainer split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.SplitterDistance = 700;

            TabControl tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;

            TabPage dirTab = new TabPage("Directed");
            GraphPanel dirPanel = new GraphPanel(logic, true);
            dirPanel.Dock = DockStyle.Fill;
            dirTab.Controls.Add(dirPanel);

            TabPage undirTab = new TabPage("Undirected");
            GraphPanel undirPanel = new GraphPanel(logic, false);
            undirPanel.Dock = DockStyle.Fill;
            undirTab.Controls.Add(undirPanel);

            tabs.TabPages.Add(dirTab);
            tabs.TabPages.Add(undirTab);
            split.Panel1.Controls.Add(tabs);

            GraphAnalyzer analyzer = new GraphAnalyzer(logic);
            TextBox results = new TextBox();
            results.Dock = DockStyle.Fill;
            results.Multiline = true;
            results.ScrollBars = ScrollBars.Vertical;
            results.ReadOnly = true;
            results.Font = new Font("Consolas", 10);
            results.Text = BuildResultsText(logic, analyzer);
            split.Panel2.Controls.Add(results);

            this.Controls.Add(split);
        }
        private string BuildResultsText(GraphLogic logic, GraphAnalyzer analyzer)
        {
            StringBuilder sb = new();
            int [] degreesUnDir = analyzer.UndirectedDegrees();
            sb.AppendLine("Undirected graph:");
            for (int i = 0; i < logic.UndirectedGraph.VertexCount; i++)
            {
                sb.AppendLine($"Vertex {i}: deg = {degreesUnDir[i]}");
            }
            var (degIn, degOut, deg) = analyzer.DirectedDegrees();  
            sb.AppendLine("\nDirected graph:");
            for (int i = 0; i < logic.n; i++)
            {
                sb.AppendLine($"Vertex {i+1, -5}: indeg = {degIn[i] -2 }, outdeg = {degOut[i] -3}, deg = {deg[i]}");
            }
            var (reg, regDeg) = analyzer.Regularity();
            sb.AppendLine($"\nRegularity:");
            sb.AppendLine( reg ? $"Regular graph, degree: = {regDeg}" : "Undirected graph");

            var (hanging, isolated) = analyzer.HangingAndIsolatedVertices();
            sb.AppendLine($"\n Additional vertices:");
            sb.AppendLine("Hanging : " + (hanging.Count > 0 ? string.Join(", ", hanging) : "None"));
            sb.AppendLine("Isolated: " + (isolated.Count > 0 ? string.Join(", ", isolated) : "None"));
            return sb.ToString();
            
        }
    }
}
        
