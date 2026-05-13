using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Drawing.Drawing2D;

namespace Lab4
{        
    public class GraphForm : Form
        {
        public GraphForm(GraphLogic original, GraphLogic changed)
    {

            this.Size = new Size(1100, 700);
            this.Text = "GraphTypes";
            SplitContainer split = new SplitContainer();
            split.Dock = DockStyle.Fill;

            TabControl tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;

            TabPage dirTab = new TabPage("Directed");
            GraphPanel dirPanel = new GraphPanel(original, true);
            dirPanel.Dock = DockStyle.Fill;
            dirTab.Controls.Add(dirPanel);

            TabPage undirTab = new TabPage("Undirected");
            GraphPanel undirPanel = new GraphPanel(original, false);
            undirPanel.Dock = DockStyle.Fill;
            undirTab.Controls.Add(undirPanel);

            tabs.TabPages.Add(dirTab);
            tabs.TabPages.Add(undirTab);
            split.Panel1.Controls.Add(tabs);

            TabPage changedDirTab = new TabPage("Changed Directed");
            GraphPanel changedDirPanel = new GraphPanel(changed, true);
            changedDirPanel.Dock = DockStyle.Fill;
            changedDirTab.Controls.Add(changedDirPanel);

            TabPage changedUndirTab = new TabPage("Changed Undirected");
            GraphPanel changedUndirPanel = new GraphPanel(changed, false);
            changedUndirPanel.Dock = DockStyle.Fill;
            changedUndirTab.Controls.Add(changedUndirPanel);

            GraphAnalyzer graphAnalyzer2 = new GraphAnalyzer(changed);
            var components = graphAnalyzer2.GetStrongComponents();
            int [,] condensationMatrix = graphAnalyzer2.GetCondensationMatrix();
            CondensationGraph condensationGraph = new CondensationGraph(condensationMatrix, components.Count);
            condensationGraph.CalculateLayout(150, 100, 450, 350);



            TabPage CondensationTab = new TabPage("Condensation");
            GraphPanel CondensationPanel = new GraphPanel(condensationGraph, true, true);
            CondensationPanel.Dock = DockStyle.Fill;
            CondensationTab.Controls.Add(CondensationPanel);

            tabs.TabPages.Add(changedDirTab);
            tabs.TabPages.Add(changedUndirTab);
            tabs.TabPages.Add(CondensationTab);

            GraphAnalyzer analyzer = new GraphAnalyzer(original);
            TextBox results = new TextBox();
            results.Dock = DockStyle.Fill;
            results.Multiline = true;
            results.ScrollBars = ScrollBars.Both;
            results.ReadOnly = true;
            results.WordWrap = false;
            results.Font = new Font("Consolas", 10);
            results.Text = BuildResultsText(original, analyzer, changed);
            split.Panel2.Controls.Add(results);

            this.Controls.Add(split);
            split.SplitterDistance = 700;
        }
    private string BuildResultsText(GraphLogic logic, GraphAnalyzer analyzer, GraphLogic changed)
{
    StringBuilder sb = new();

    int[] degreesUnDir = analyzer.CalculateDegreesUnDir();
    sb.AppendLine("Undirected graph:");
    for (int i = 0; i < logic.n; i++)
        sb.AppendLine($"  v{i + 1}: deg = {degreesUnDir[i]}");

    var (degIn, degOut, deg) = analyzer.CalculateDegreesDir();
    sb.AppendLine("\nDirected graph:");
    for (int i = 0; i < logic.n; i++)
        sb.AppendLine($" v{i+1,-5}: in={degIn[i],-3} out={degOut[i],-3} deg={deg[i]}");

    var (isReg, regDeg) = analyzer.IsRegular(degreesUnDir);
    sb.AppendLine("\nRegularity:");
    sb.AppendLine(isReg
        ? $"Regular, degree: {regDeg}"
        : "Irregular:");

    var (hanging, isolated) = analyzer.GetSpecialVertices(degreesUnDir);
    sb.AppendLine("\nSpecial vertices:");
    sb.AppendLine("Hanging:  " + (hanging.Count > 0
        ? string.Join(", ", hanging) : "None"));
    sb.AppendLine("Isolated: " + (isolated.Count > 0
        ? string.Join(", ", isolated) : "None"));

        sb.AppendLine("\n Changed graph:");
        GraphAnalyzer changedAnalyzer = new GraphAnalyzer(changed);

        var (degIn2, degOut2, deg2) = changedAnalyzer.CalculateDegreesDir();
sb.AppendLine("Semi-degrees:");
for (int i = 0; i < changed.n; i++)
    sb.AppendLine($"  v{i+1,-5}: in={degIn2[i],-3} out={degOut2[i],-3} deg={deg2[i]}");

sb.AppendLine("\nReachability matrix:");
int[,] D = changedAnalyzer.GetReachabilityMatrix();
for (int i = 0; i < changed.n; i++)
{
    sb.Append("  ");
    for (int j = 0; j < changed.n; j++)
        sb.Append(D[i, j] + " ");
    sb.AppendLine();
}

sb.AppendLine("\nPaths of length 2:");
var paths2 = changedAnalyzer.GetPathsOfLength2();
foreach (var path in paths2)
    sb.AppendLine("  " + path);

sb.AppendLine("\nPaths of length 3:");
var paths3 = changedAnalyzer.GetPathsOfLength3();
foreach (var path in paths3)
    sb.AppendLine("  " + path);

sb.AppendLine("\nStrong connectivity matrix:");
int[,] S = changedAnalyzer.GetStrongConnectivityMatrix();
for (int i = 0; i < changed.n; i++)
{
    sb.Append("  ");
    for (int j = 0; j < changed.n; j++)
        sb.Append(S[i, j] + " ");
    sb.AppendLine();
}


sb.AppendLine("\nStrong components:");
var components = changedAnalyzer.GetStrongComponents();
for (int i = 0; i < components.Count; i++)
    sb.AppendLine($"  C{i+1}: {string.Join(", ", components[i])}");


sb.AppendLine("\nCondensation matrix:");
int[,] C = changedAnalyzer.GetCondensationMatrix();
for (int i = 0; i < components.Count; i++)
{
    sb.Append("  ");
    for (int j = 0; j < components.Count; j++)
        sb.Append(C[i, j] + " ");
    sb.AppendLine();
}

    return sb.ToString();
        }
    }
}

        
