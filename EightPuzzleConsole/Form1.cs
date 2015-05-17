using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using EightPuzzleGame;
using EightPuzzleGame.Calculators;
using EightPuzzleGame.SuccessorNodes;

namespace EightPuzzleConsole
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(8, 2, null);
            dataGridView1.Rows.Add(1, 5, 3);
            dataGridView1.Rows.Add(4, 6, 7);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        private void Calculate(EightPuzzleNode start)
        {
            try
            {
                for (var i = 0; i <= 8; i++)
                {
                    if (start.Tiles.Contains(i)) continue;
                    throw new Exception("Fields filled improperly");
                }
                if (!start.IsSolvable)
                    throw new Exception("Puzzle is unsolvable");
                dataGridView1.ClearSelection();
                
                var goal = new EightPuzzleNode {Tiles = new[] {1, 2, 3, 4, 5, 6, 7, 8, 0}};

                var pathFinder = new PathFinder(
                    new EightPuzzleSuccessorNodesGenerator(),
                    new EightPuzzleGValueCalculator(),
                    new EightPuzzleManhattanDistanceCalulator());

                INode result = pathFinder.Execute(start, goal);
                PrintSolution(result, dataGridView1);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private static void PrintSolution(INode node, DataGridView dg)
        {

            if (node != null)
            {
                var stack = new Stack<INode>();

                do
                {
                    stack.Push(node);
                } while ((node = node.Parent) != null);

                EightPuzzleNode check = (EightPuzzleNode) stack.First();
                if (check.Tiles.SequenceEqual(new[] {1, 2, 3, 4, 5, 6, 7, 8, 0}))
                {
                    MessageBox.Show("No need to solve solved puzzle");
                    return;
                }

                //var step = 0;

                foreach (EightPuzzleNode solutionNode in stack)
                {

                    dg.EditMode = DataGridViewEditMode.EditProgrammatically;
                    
                    string tiles = solutionNode.Tiles
                        .Aggregate("", (current, i) => current + i.ToString());

                    for (var i = 0; i < 9; i++)
                    {
                        
                        var col = (i)%3;
                        var row = (i)/3;
                        dg.BeginEdit(true);
                        dg.NotifyCurrentCellDirty(false);
                        dg[col, row].Value = tiles[i] != '0' ? (object) (int) Char.GetNumericValue(tiles[i]) : null;
                        dg.EndEdit();
                        dg.NotifyCurrentCellDirty(false);
                    }

                    new ManualResetEvent(false).WaitOne(500);
                    dg.EditMode = DataGridViewEditMode.EditOnKeystroke;
                }

                MessageBox.Show(stack.Count - 1 + " steps");
            }
            else
            {
                MessageBox.Show("No solution");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var array = new int[9];
            for (var i = 0; i < dataGridView1.Rows.Count; i++)
                for (var j = 0; j < dataGridView1.Columns.Count; j++)
                    array[3*j + i] = dataGridView1[i, j].Value != "" ? Convert.ToInt32(dataGridView1[i, j].Value) : 0;
            Calculate(new EightPuzzleNode {Tiles = array});
        }
    }
}
