using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextureAtlas.PipelineExtension.Utility
{
    class OT
    {
        private OTree oTree;
        private Dictionary<int, Module> modules;
        private Placement placement;

        /// <summary>
        /// Algoritms class for O-Tree representation.
        /// </summary>
        /// <param name="ot">The O-tree code which describes the placement.</param>
        /// <param name="_modules">Modules to be packed.</param>
        public OT(OTree ot, List<Module> _modules)
        {
            oTree = ot;
            modules = _modules.ToDictionary(item => item.Name, item => item);
            placement = null;
        }

        /// <summary>
        /// Gets the calculated placement.
        /// </summary>
        public Placement Placement
        {
            get 
            {
                ToCompact();
                return placement; 
            }
        }

        /// <summary>
        /// Operation of horizontal O-tree. Calculates coordinates and takes one compaction step.
        /// </summary>
        /// <returns>Vertical constraint graph</returns>
        private Graph ToVerticalConstraintGraph()
        { 
            //Empty module representing the root of the tree
            Module root = new Module(-1, null, 0);
            modules.Add(-1, root);
            oTree.ModuleSequence.Insert(0, -1);

            //Stack containing module labels for parent module calculation
            Stack<int> stack = new Stack<int>();
            stack.Push(-1);

            //Vertical contsraint graph
            Graph constraintGraph = new Graph(oTree.ModuleSequence);

            //Actual parent and child module
            Module parent = root;
            Module child;

            //Horizontal contour for quick y-coordinate calculation
            Contour contour = new HorizontalContour(root);

            //Index of child module in ModuleSequence
            int childIndex = 0;

            foreach (Bit bit in oTree.DfsSequence)
            {
                //Forth step in DFS traversing, coordinates need to be calcuted
                if (bit == 0)
                {
                    child = modules[oTree.ModuleSequence[++childIndex]];

                    //In horizontal O-tree, child module is on the rigth side of the parent module and adjacent with it
                    child.X = parent.X + parent.Width;
                    //Finding the minimum y-coordinate
                    child.Y = contour.FindMax(child.X + child.Width);

                    //There is an egde in the vertical constraint graph, where the minimum is found
                    constraintGraph.AddEdge(contour.WhereMax.Name, child.Name, child.X);

                    //Updating contour
                    contour.Update(child);

                    //Now child module is the actual parent
                    parent = child;
                    stack.Push(parent.Name);
                }

                //Back step in DFS traversing
                else
                {
                    //Updating parent module and the insertation index of the contour
                    stack.Pop();
                    parent = modules[stack.Peek()];
                    contour.InsertationIndex = contour.ModuleSequence.IndexOf(parent);
                }

            }

            //Removing root module
            modules.Remove(-1);
            oTree.ModuleSequence.RemoveAt(0);

            return constraintGraph;
        }

        /// <summary>
        /// Operation of vertical O-tree. Calculates coordinates and takes one compaction step.
        /// </summary>
        /// <returns>Horizontal constraint graph</returns>
        private Graph ToHorizontalConstraintGraph()
        {
            //Empty module representing the root of the tree
            Module root = new Module(-1, null, 0);
            modules.Add(-1, root);
            oTree.ModuleSequence.Insert(0, -1);

            //Stack containing module labels for parent module calculation
            Stack<int> stack = new Stack<int>();
            stack.Push(-1);

            //Vertical contsraint graph
            Graph constraintGraph = new Graph(oTree.ModuleSequence);

            //Actual parent and child module
            Module parent = root;
            Module child;

            //Vertical contour for quick x-coordinate calculation
            Contour contour = new VerticalContour(root);

            //Index of child module in ModuleSequence
            int childIndex = 0;

            foreach (Bit bit in oTree.DfsSequence)
            {
                //Forth step in DFS traversing, coordinates need to be calcuted
                if (bit == 0)
                {
                    child = modules[oTree.ModuleSequence[++childIndex]];

                    //In vertical O-tree, child module is on the top of parent module and adjacent with it
                    child.Y = parent.Y + parent.Height;
                    //Finding the minimum x-coordinate
                    child.X = contour.FindMax(child.Y + child.Height);

                    //There is an egde in the horizontal constraint graph, where the minimum is found
                    constraintGraph.AddEdge(contour.WhereMax.Name, child.Name, child.Y);

                    //Updating contour
                    contour.Update(child);

                    //Now child module is the actual parent
                    parent = child;
                    stack.Push(parent.Name);
                }

                //Back step in DFS traversing
                else
                {
                    //Updating parent module and insertation index of the contour
                    stack.Pop();
                    parent = modules[stack.Peek()];
                    contour.InsertationIndex = contour.ModuleSequence.IndexOf(parent);
                }

            }

            //Removing root module
            modules.Remove(-1);
            oTree.ModuleSequence.RemoveAt(0);
            return constraintGraph;
        }

        /// <summary>
        /// Creates LB-compact placement with sequence of compaction steps.
        /// </summary>
        private void ToCompact()
        {
            //Stop condition of while loop, true if compaction steps change the O-tree
            bool changed = true;

            //Copy of components of the actual O-tree. If compaction steps does not change them, placement is compact.
            List<Bit> DfsSequenceCopy = new List<Bit>(oTree.DfsSequence);
            List<int> moduleSequenceCopy = new List<int>(oTree.ModuleSequence);

            while (changed)
            {
                changed = false;

                //Vertical constraint graph from horizontal O-tree.
                Graph gVertical = ToVerticalConstraintGraph();
                //Vertical O-tree from vertical constraint graph.
                oTree.ModuleSequence = gVertical.DepthFirstSearch(oTree.DfsSequence);

                //Horizontal constraint graph from vertical O-tree.
                Graph gHorizontal = ToHorizontalConstraintGraph();
                //Horizontal O-tree from horizontal constraint graph.
                oTree.ModuleSequence = gHorizontal.DepthFirstSearch(oTree.DfsSequence);

                //Checking the changes of the O-Tree after compaction steps.
                //If the O-tree has changed, more compaction steps could be needed on the changed tree.
                if (!(oTree.DfsSequence.SequenceEqual(DfsSequenceCopy) &&
                      oTree.ModuleSequence.SequenceEqual(moduleSequenceCopy)))
                {
                    moduleSequenceCopy = new List<int>(oTree.ModuleSequence);
                    DfsSequenceCopy = new List<Bit>(oTree.DfsSequence);
                    changed = true;
                }
            }

            //Compact placement.
            placement = new Placement(modules.Values.ToList<Module>());
        }
    }
}
