using System;
using System.Windows.Forms;
namespace Lab6
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
 
            GraphLogic logic = new GraphLogic();
 
            // Варіант 5212
            logic.GenerateMatrix(5212);
            logic.GenerateWeightMatrix(5212);
            logic.CalculateLayout(60, 60, 450, 350);
 
            Application.Run(new GraphForm(logic));
        }
    }
}
