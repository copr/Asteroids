using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public enum ERotateBehavior
    {
        /// <summary>
        /// Object keeps position offset from its owner, but doesnt rotate at all
        /// </summary>
        Static, 
        /// <summary>
        /// Object rotates around the center of its owner, but keeps the same angle of image
        /// </summary>
        RotateAroundOwner, 
        /// <summary>
        /// Object rotates around the center of its owner, and also rotates its own image accordingly
        /// </summary>
        RotateCompletely
    }
}
