using System;
using System.Windows.Forms;
using System.Drawing;
namespace Lab4
{
    static class Program
    {
static void Main()
{

    GraphLogic myGraph = new GraphLogic();
    myGraph.GenerateMatrix(5212);
    myGraph.CalculateLayout(150, 100, 450, 350);

    ChangedMatrix changedGraph = new ChangedMatrix();
    changedGraph.GenerateMatrix(5212);
    changedGraph.CalculateLayout(150, 100, 450, 350);
    
    // Test for change matrix 
    Console.WriteLine("Changed directed matrix:");
for (int i = 0; i < changedGraph.n; i++) {
    for (int j = 0; j < changedGraph.n; j++)
        Console.Write(changedGraph.AdjMatrixDir[i, j] + " ");
    Console.WriteLine();
}
Console.ReadLine();

    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new GraphForm(myGraph, changedGraph));
        }
    }
}
