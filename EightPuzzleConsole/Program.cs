using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using EightPuzzleGame;
using EightPuzzleGame.Calculators;
using EightPuzzleGame.SuccessorNodes;

namespace EightPuzzleConsole
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }    
    }
}