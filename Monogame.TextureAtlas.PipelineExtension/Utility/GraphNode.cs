using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextureAtlas.PipelineExtension.Utility
{
    class GraphNode
    {
        public Dictionary<int, int> incomingEdges;
        public Dictionary<int, int> outgoingEdges;

        public void InitializeEdges()
        {
            incomingEdges = new Dictionary<int, int>();
            outgoingEdges = new Dictionary<int, int>();
        }
    }
}
