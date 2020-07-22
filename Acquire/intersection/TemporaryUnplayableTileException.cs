using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// Exception that occurs when there are no more corporations to form.
    /// </summary>
    [Serializable]
    public class TemporaryUnplayableTileException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Tile info</param>
        public TemporaryUnplayableTileException(string name) : base(String.Format("Unplayable Tile: {0}", name)) { }
    }
}
