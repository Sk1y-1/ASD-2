using System;
using System.Windows.Forms;
using System.Drawing;
namespace Lab4
{
    static class Program
    {
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    GraphLogic myGraph = new GraphLogic();
    myGraph.GenerateMatrix(5212);
    myGraph.CalculateLayout(150, 100, 450, 350);

    ChangedMatrix changedGraph = new ChangedMatrix();
    changedGraph.GenerateMatrix(5212);
    changedGraph.CalculateLayout(150, 100, 450, 350);

    Application.Run(new GraphForm(myGraph, changedGraph));
        }
    }
}
