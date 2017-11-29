using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextureAtlas.PipelineExtension.Utility
{
    //Contour is the list of the modules on the top (horizontal contour) or on the right (vertical contour) of the 
    //placement. It is needed for linear time computation of the modules coordinates. It is easier to understand 
    //it from some figure. See reference.
    abstract class Contour
    {
        protected List<Module> moduleSequence;
        protected int insertationIndex;
        protected Module whereMax;

        protected void Construct(Module root)
        {
            moduleSequence = new List<Module>();
            moduleSequence.Add(root);
            whereMax = root;
            insertationIndex = -1;
        }

        /// <summary>
        /// Sets the insertation index of the contour.
        /// </summary>
        public int InsertationIndex
        {
            set { insertationIndex = value; }
        }

        /// <summary>
        /// Gets the sequence of modules whereof the contour consists.
        /// </summary>
        public List<Module> ModuleSequence
        {
            get { return moduleSequence; }
        }

        /// <summary>
        /// Gets the module with the maximum y-coordinate in a given x-coordinate range or conversely.
        /// It is calculated by FindMax method.
        /// </summary>
        public Module WhereMax
        {
            get { return whereMax; }
        }

        public abstract int FindMax(int to);

        /// <summary>
        /// Inserts new module into the contour and clears WhereMax value.
        /// </summary>
        /// <param name="module"></param>
        public void Update(Module module)
        {
            moduleSequence.Insert(++insertationIndex, module);
            whereMax = new Module(-1, null, 0);
        }
    }
}
