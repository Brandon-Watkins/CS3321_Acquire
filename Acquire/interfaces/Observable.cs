using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire.interfaces
{
    /// <summary>
    /// Observers subscribe and get notified of updates.
    /// </summary>
    public interface Observable
    {
        /// <summary>
        /// Subscribes an observer to receive updates.
        /// </summary>
        /// <param name="observer">An object that implements observer interface.</param>
        void subscribe(Observer observer);

        /// <summary>
        /// Unsubscribes an observer from receiving updates.
        /// </summary>
        /// <param name="observer">An object that implements observer interface.</param>
        void unsubscribe(Observer observer);

        /// <summary>
        /// Notifies subscribers when something happens in the game.
        /// </summary>
        void notifySubscribers();
    }
}
