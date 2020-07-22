using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire.interfaces
{
    /// <summary>
    /// Gets updates from observable.
    /// </summary>
    public interface Observer
    {
        /// <summary>
        /// Updates the specified game event.
        /// </summary>
        void update();
    }
}
