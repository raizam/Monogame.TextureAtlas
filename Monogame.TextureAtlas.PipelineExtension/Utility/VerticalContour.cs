using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextureAtlas.PipelineExtension.Utility
{
    class VerticalContour : Contour
    {
        /// <summary>
        /// Contour class for quick computation of x-coordinates during working with vertical O-Tree.
        /// </summary>
        /// <param name="root">First element of the contour.</param>
        public VerticalContour(Module root)
        {
            Construct(root);
        }

        /// <summary>
        /// Finds the minimum x-coordinate where the module can be inserted.
        /// </summary>
        /// <param name="to">Maximum y-coordinate until modules on the left of the actual module need to be checked.</param>
        /// <returns></returns>
        public override int FindMax(int to)
        {
            int max = 0;
            //Actual module does not need to be checked.
            int indexFrom = insertationIndex + 1;

            //Checking modules in contour.
            while (indexFrom < moduleSequence.Count && moduleSequence[indexFrom].Y < to)
            {
                //Overwriting maximum.
                if (max < moduleSequence[indexFrom].X + moduleSequence[indexFrom].Width)
                {
                    max = moduleSequence[indexFrom].X + moduleSequence[indexFrom].Width;
                    whereMax = moduleSequence[indexFrom];
                }

                //Removing modules, which are covered by the module will be inserted.
                if (moduleSequence[indexFrom].Y + moduleSequence[indexFrom].Height <= to)
                {
                    moduleSequence.RemoveAt(indexFrom);
                }

                else
                    indexFrom++;
            }

            return max;
        }
    }
}
