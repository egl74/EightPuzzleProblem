using System;
using System.Collections.Generic;
using System.Linq;

namespace EightPuzzleGame.SuccessorNodes
{
    public class EightPuzzleSuccessorNodesGenerator : ISuccessorNodesGenerator
    {
        #region ISuccessorNodesGenerator Members

        public IEnumerable<INode> Execute(INode currentNode)
        {
            try
            {
                var node = currentNode as EightPuzzleNode;

                var successorRules =
                    new List<ISuccessorNodesCalculationRule>
                    {
                        new EightPuzzleSuccessorsForEmptyTileZero(),
                        new EightPuzzleSuccessorsForEmptyTileOne(),
                        new EightPuzzleSuccessorsForEmptyTileTwo(),
                        new EightPuzzleSuccessorsForEmptyTileThree(),
                        new EightPuzzleSuccessorsForEmptyTileFour(),
                        new EightPuzzleSuccessorsForEmptyTileFive(),
                        new EightPuzzleSuccessorsForEmptyTileSix(),
                        new EightPuzzleSuccessorsForEmptyTileSeven(),
                        new EightPuzzleSuccessorsForEmptyTileEight()
                    };

                return successorRules
                    .Single(r => r.Match(node))
                    .GetSuccessors(node);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return null;
            }

        }

        #endregion
    }
}