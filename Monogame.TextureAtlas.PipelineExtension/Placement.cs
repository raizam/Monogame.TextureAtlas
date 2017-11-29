using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TextureAtlas.PipelineExtension.Utility;

namespace TextureAtlas.PipelineExtension
{
    public class Placement
    {
        private List<Module> modules;

        public Placement(List<Module> _modules)
        {
            modules = _modules;
        }

        /// <summary>
        /// Gets the half perimeter of the placement.
        /// </summary>
        public int Perimeter
        {
            get { return modules.Max(m => m.X + m.Width) + modules.Max(m => m.Y + m.Height); }
        }

        /// <summary>
        /// Gets the width of the palcement.
        /// </summary>
        public int Width
        {
            get { return modules.Max(m => m.X + m.Width); }
        }

        /// <summary>
        /// Gets the height of the placement.
        /// </summary>
        public int Height
        {
            get { return modules.Max(m => m.Y + m.Height); }
        }

        /// <summary>
        /// Gets the modules in the placement.
        /// </summary>
        public List<Module> Modules
        {
            get { return modules; }
        }
    }
}
