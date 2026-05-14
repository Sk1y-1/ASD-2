using System;
using System.Windows.Forms;
using Lab5; 

namespace Lab5
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GraphLogic logic = new GraphLogic();
            logic.GenerateMatrix(5212); 
            logic.CalculateLayout(100, 100, 450, 350);

            Application.Run(new GraphForm(logic));
        }
    }
}
