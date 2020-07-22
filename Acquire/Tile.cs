using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// Represents a single Tile. Has a position and (potentially) associated corporation.
    /// </summary>
    public class Tile
    {
        private Corporation corp;
        private string position;

        /// <summary>
        /// Tile Constructor
        /// </summary>
        /// <param name="position">string of the form '1A'</param>
        /// <exception cref="ArgumentException">Invalid position exception</exception>
        public Tile(string position)
        {
            if (!isValidPosition(position))
            {
                throw new ArgumentException("String " + position + " is not a valid tile position.");
            }
            this.position = position;
            this.corp = null;
        }
        /// <summary>
        /// Corporation property, to get and set the corporation associated with tile.
        /// </summary>
        public Corporation Corp { get => corp; set => corp = value; }

        /// <summary>
        /// Checks if the position is in valid format 1A - 12I
        /// </summary>
        /// <param name="position">string of the form '1A'</param>
        /// <returns>True if valid</returns>
        private bool isValidPosition(string position)
        {
            // Check length
            int strLen = position.Length;
            if (strLen < 2 || strLen > 3)
                return false;

            // row - last char 'A' - 'I'
            char row = position[strLen - 1];
            if(row < 'A' || row > 'I')
            {
                return false;
            }

            // col - First digits 1 - 12
            int col = Int32.Parse(position.Substring(0, strLen - 1));
            return col >= 1 && col <= 12;
        }

        /// <summary>
        /// Get Position
        /// </summary>
        /// <returns>The position of the tile</returns>
        public string getPosition()
        {
            return this.position;
        }

        /// <summary>
        /// Get the row based on tile position (A-I)
        /// </summary>
        /// <returns>string from A to I</returns>
        public char getRow()
        {
            return this.position.Last();
        }

        /// <summary>
        /// Get the column based on tile position (1-12)
        /// </summary>
        /// <returns>integer between 1 and 12</returns>
        public int getColumn()
        {
            return Int32.Parse(this.position.Substring(0, position.Length-1));
        }

        /// <summary>
        /// Sets the corporation associated with the current tile.
        /// </summary>
        /// <param name="corporation">(Corporation) Corporation to be associated with.</param>
        public void setCorporation(Corporation corporation)
        {
            this.corp = corporation;
        }


    }
}
