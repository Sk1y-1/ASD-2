using System;
using System.Windows.Forms;
namespace Lab3
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        
            GraphLogic myGraph = new GraphLogic();
            myGraph.GenerateMatrix(5212);
            myGraph.CalculateLayout(150, 100, 450, 350);

            Console.WriteLine("Matrix of directed graph:");
            for (int i = 0; i < myGraph.n; i++) {
        for (int j = 0; j < myGraph.n; j++) {
            Console.Write(myGraph.AdjMatrixDir[i, j] + " ");
        }
        Console.WriteLine();
    }
        Console.WriteLine("\nMatrix of undirected graph:");
        for (int i = 0; i < myGraph.n; i++) {
        for (int j = 0; j < myGraph.n; j++) {
            Console.Write(myGraph.AdjMatrixUnDir[i, j] + " ");
        }
        Console.WriteLine();
    }

            Application.Run(new GraphForm(myGraph));
            }
        }
    }




