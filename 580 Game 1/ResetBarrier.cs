using System;
using System.Collections.Generic;
using System.Text;
using SwordsDance.Collisions;

namespace SwordsDance
{
    public class ResetBarrier
    {

        private BoundingRectangle bounds = new BoundingRectangle(-100, 0, 40, 600);

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;
    }
}
