using System.Linq;

namespace EightPuzzleGame
{
    public class EightPuzzleNode : INode
    {
        private int _emptyTileIndex;

        public EightPuzzleNode()
        {
            _emptyTileIndex = -1;
        }

        public int[] Tiles { get; set; }

        public int EmptyTileIndex
        {
            get
            {
                if (_emptyTileIndex == -1)
                    _emptyTileIndex = GetEmptyTilePosition(this);

                return _emptyTileIndex;
            }
        }

        #region INode Members

        public float F { get; set; }
        public float G { get; set; }
        public float H { get; set; }

        public INode Parent { get; set; }

        public bool HasEqualState(INode node)
        {
            var testNode = node as EightPuzzleNode;

            return testNode != null && Tiles.SequenceEqual(testNode.Tiles);
        }

        #endregion

        private static int GetEmptyTilePosition(EightPuzzleNode node)
        {
            int emptyTilePos = -1;

            for (int i = 0; i < 9; i++)
            {
                if (node.Tiles[i] == 0)
                {
                    emptyTilePos = i;
                    break;
                }
            }

            return emptyTilePos;
        }

        private int NumberOfInversions
        {
            get
            {
                var invCount = 0;
                for (var i = 0; i < 9 - 1; i++)
                    for (var j = i + 1; j < 9; j++)
                        // Value 0 is used for empty space
                        if (Tiles[i] != 0 && Tiles[j] != 0 && (Tiles[i] > Tiles[j]))
                            invCount++;
                return invCount;
            }
        }

        public bool IsSolvable
        {
            get { return NumberOfInversions%2 == 0; }
        }
    }
}