using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextureAtlas.PipelineExtension.Utility;

namespace TextureAtlas.PipelineExtension
{
    //Code class for O-Tree representation. O-Tree encodes a placement by a sequence of 
    //module labels and a 0-1 sequence describing the tree structure.
    class OTree
    {
        private List<int> moduleSequence;
        private List<Bit> dfsSequence;

        /// <summary>
        /// Creates new instance of O-Tree code with empty sequences.
        /// </summary>
        public OTree()
        {
            moduleSequence = new List<int>();
            dfsSequence = new List<Bit>();
        }

        /// <summary>
        /// Gets or sets the sequence of module labels.
        /// </summary>
        public List<int> ModuleSequence
        {
            get { return moduleSequence; }
            set { moduleSequence = value; }
        }

        /// <summary>
        /// Gets or sets the 0-1 sequence of DFS-order.
        /// </summary>
        public List<Bit> DfsSequence
        {
            get { return dfsSequence; }
        }

        /// <summary>
        /// Deep copy.
        /// </summary>
        /// <returns></returns>
        public OTree Copy()
        {
            OTree ot = new OTree();
            ot.moduleSequence = new List<int>(moduleSequence);
            ot.dfsSequence = new List<Bit>(dfsSequence);

            return ot;
        }

        /// <summary>
        /// Calculates insertation positions according to DFS-sequence.
        /// </summary>
        /// <returns>IEnumerable collection of zero based indices in DFS-sequence where new element 
        /// can be inserted.</returns>
        public IEnumerable<int> InsertationPoints()
        {
            for (int i = 0; i <= dfsSequence.Count; i++)
                yield return i;
        }

        /// <summary>
        /// Inserts new element into O-Tree code.
        /// </summary>
        /// <param name="item">Module label.</param>
        /// <param name="dfsIndex">DFS-sequence index.</param>
        public void Insert(int item, int dfsIndex)
        {
            //Inserting 0 and 1 into DFS sequence representing a forth and a back step.
            dfsSequence.InsertRange(dfsIndex, new Bit[] { 0, 1 });

            //Calculating index of module label in moduleSequence.
            //Every forth step in DFS traversing before dfsIndex belong to a previous module in moduleSequence.
            int moduleIndex = dfsSequence.GetRange(0, dfsIndex).Count(b => b == 0);
            moduleSequence.Insert(moduleIndex, item);
        }
    }
}
