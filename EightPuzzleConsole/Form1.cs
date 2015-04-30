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


            //dataGridView1.Rows.Add(1, 2, 3);
            //dataGridView1.Rows.Add(4, 5, 6);
            //dataGridView1.Rows.Add(7, null, 8);

        }

        private void Calculate(EightPuzzleNode start)
        {
            try
            {
                dataGridView1.ClearSelection();
                //do
                //{
                var goal = new EightPuzzleNode {Tiles = new[] {1, 2, 3, 4, 5, 6, 7, 8, 0}};
                //var start = new EightPuzzleNode {Tiles = new int[9]};

                //Console.WriteLine("Enter a valid start state (e.g. 867254031) - numbers '1'-'8' + marker '0'");
                //string userinput = Console.ReadLine();


                //if (userinput != null)
                //{
                //int i = 0;

                //foreach (char s in userinput)
                //{
                //    int tile;


                //    if (!int.TryParse(s.ToString(), out tile)) continue;

                //    start.Tiles[i++] = tile;
                //}

                var pathFinder = new PathFinder(
                    new EightPuzzleSuccessorNodesGenerator(),
                    new EightPuzzleGValueCalculator(),
                    new EightPuzzleManhattanDistanceCalulator());

                INode result = pathFinder.Execute(start, goal);
                PrintSolution(result, dataGridView1);

                // Console.ReadKey();
                //}
                //} while (Console.ReadLine() != "exit");
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

                    //dg.SelectAll();

                    //Console.Clear();

                    //Console.WriteLine("");

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


                        //dg[i, j].Value = tiles[i];
                        //Console.WriteLine("  {0} {1} {2}",
                        //    tiles[i] == '0' ? '_' : tiles[i],
                        //    tiles[i + 1] == '0' ? '_' : tiles[i + 1],
                        //    tiles[i + 2] == '0' ? '_' : tiles[i + 2]);
                        //i = i + 2;

                    }

                    //Thread.Sleep(500);
                    new ManualResetEvent(false).WaitOne(500);
                    //step++;
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
            //int[] column0Array = new int[dataGridView1.Rows.Count];
            //int[] column1Array = new int[dataGridView1.Rows.Count];
            //int[] column2Array = new int[dataGridView1.Rows.Count];

            //int i = 0;

            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{

            //    column0Array[i] = row.Cells[0].Value != null ? Convert.ToInt32(row.Cells[0].Value) : 0;
            //    column1Array[i] = row.Cells[1].Value != null ? Convert.ToInt32(row.Cells[1].Value) : 0;
            //    column2Array[i] = row.Cells[2].Value != null ? Convert.ToInt32(row.Cells[2].Value) : 0;
            //    i++;
            //}

            //var array = new int[column0Array.Length + column1Array.Length + column2Array.Length];
            //column0Array.CopyTo(array, 0);
            //column1Array.CopyTo(array, column1Array.Length);
            //column2Array.CopyTo(array, column1Array.Length + column2Array.Length);
            //column0Array
            var array = new int[9];
            for (var i = 0; i < dataGridView1.Rows.Count; i++)
                for (var j = 0; j < dataGridView1.Columns.Count; j++)
                    array[3*j + i] = dataGridView1[i, j].Value != "" ? Convert.ToInt32(dataGridView1[i, j].Value) : 0;
            Calculate(new EightPuzzleNode {Tiles = array});
            //Calculate(new EightPuzzleNode {Tiles = new [] {2,8,7,4,3,0,5,6,1}});
        }
    }
}
