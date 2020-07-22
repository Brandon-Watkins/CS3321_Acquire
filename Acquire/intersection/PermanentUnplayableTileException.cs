using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// Exception for permanently unplayable tiles (merging safe corporations)
    /// </summary>
    [Serializable]
    public class PermanentUnplayableTileException : Exception
    {
        /// <summary>
        /// Constructor for permanently unplayable tile error.
        /// </summary>
        /// <param name="name">The tile identifier</param>
        public PermanentUnplayableTileException(string name) : base(String.Format("Unplayable Tile: {0}", name)) { }
    }
}
