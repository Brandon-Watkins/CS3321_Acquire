using Acquire;
using System;
using System.Collections.Generic;

namespace Acquire
{
    /// <summary>
    /// 9x12 grid representing the Acquire game board on which tiles and corporations can be place.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// The maximum number of rows on the board.
        /// </summary>
        public const int MAX_ROWS = 9;
        /// <summary>
        /// The maximum number of columns on the board.
        /// </summary>
        public const int MAX_COLUMNS = 12;
        private Tile[,] board;

        /// <summary>
        /// Board Constructor
        /// </summary>
        public Board()
        {
            this.board = new Tile[Board.MAX_ROWS, Board.MAX_COLUMNS];
        }

        /// <summary>
        /// checks for adjacent tiles, and sends them onward in a List of Tiles.
        /// </summary>
        /// <param name="tile">(Tile) The newly placed tile / "connector tile".</param>
        /// <returns>(List of Tile) A list of adjacent tiles, possibly empty. </returns>
        private List<Tile> getAdjacentTiles(Tile tile)
        {
            // checks for edge cases.
            List<Tile> adjacents = new List<Tile>();
            Tuple<int, int> pos = convertPositionToGrid(tile.getPosition());
            int row = pos.Item1;
            int col = pos.Item2;
            if (row != 0)//find top tile
            {
                // if the tile has been placed...
                if (board[row - 1, col] != null)
                {
                    adjacents.Add(board[row - 1, col]);
                }
            }

            if (row != Board.MAX_ROWS - 1)//find bottom tile
            {
                // if the tile has been placed...
                if (board[row + 1, col] != null)
                {
                    adjacents.Add(board[row + 1, col]);
                }
            }

            if (col != 0)//find left tile
            {
                // if the tile has been placed...
                if (board[row, col - 1] != null)
                {
                    adjacents.Add(board[row, col - 1]);
                }
            }

            if (col != Board.MAX_COLUMNS - 1)//find right tile
            {
                // if the tile has been placed...
                if (board[row, col + 1] != null)
                {
                    adjacents.Add(board[row, col + 1]);
                }
            }

            // Returns a possibly empty List
            return adjacents;
        }

        /// <summary>
        /// Converts the tile position to two ints representing the row and column indices on the board.
        /// </summary>
        /// <param name="position">string in the for "1A"</param>
        /// <returns>2-integer Tuple representing row and column indices on the board.</returns>
        private Tuple<int, int> convertPositionToGrid(string position)
        {
            int gridRow = position[position.Length - 1] - 'A';
            int gridColumn = Int32.Parse(position.Substring(0, position.Length - 1)) - 1;
            return Tuple.Create(gridRow, gridColumn);
        }

        /// <summary>
        /// Get the Tile at the position
        /// </summary>
        /// <param name="position">string in the for "1A"</param>
        /// <returns>A tile object or null</returns>
        public Tile getTileAt(string position)
        {
            Tuple<int, int> tuple = this.convertPositionToGrid(position);
            return board[tuple.Item1, tuple.Item2];
        }

        /// <summary>
        /// Plays the tile on the board. It may cause several different events to occur.
        /// </summary>
        /// <param name="tile">The tile object to play.</param>
        /// <returns>TileIntersection: object containing surrounding tiles, or null if there were no surrounding tiles.</returns>
        public TileIntersection playTile(Tile tile)
        {
            // Get row and column on the board grid
            var tuple = this.convertPositionToGrid(tile.getPosition());
            // Put the tile on the board
            board[tuple.Item1, tuple.Item2] = tile;

            // Forms a corporation, if possible.
            List<Tile> adjTiles = getAdjacentTiles(tile);
            if (adjTiles.Count > 0)
            {
                TileIntersectionFactory factory = new TileIntersectionFactory();
                return factory.determineIntersection(tile, adjTiles);
            }
            return null;
        }

        /// <summary>
        /// Removes a tile from the board at position
        /// </summary>
        /// <param name="position">string of the form '1A'</param>
        /// <returns>Tile object or null</returns>
        public Tile removeTileAt(string position)
        {
            Tuple<int, int> tuple = this.convertPositionToGrid(position);
            Tile tile = board[tuple.Item1, tuple.Item2];
            if (tile != null)
                board[tuple.Item1, tuple.Item2] = null; // Remove tile from board
            return tile;
        }
    }
}
