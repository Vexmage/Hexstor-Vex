using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexstor.Module.Shared.Models
{
    /// <summary>
    /// Represents a double-coordinate hex system.
    /// https://www.redblobgames.com/grids/hexagons/#coordinates-doubled
    /// </summary>
    public class DoubCoord
    {
        // Addition of encapsulation to ensure that the properties are only modified in controlled ways and prevent accidental modification.
        // Xcord and Ycord are calculated based on the row and column values passed in not set directly
        public int Row { get; private set; }
        public int Col { get; private set; }
        public int Xcord { get; private set; }
        public int Ycord { get; private set; }

        // constrctor takes a row and column and calculates the x and y coordinates
        // Xcord = col, Ycord = (row * 2) + (col % 2) as per the doubled coordinate system
        public DoubCoord(int row, int col)
        {
            if (row < 0 || col < 0) // Validation to ensure that the row and column are non-negative
                throw new ArgumentOutOfRangeException("Row and Col must be non-negative.");

            Row = row; // Row and Col are set to the values passed in
            Col = col; 
            Xcord = col;  
            Ycord = (row * 2) + (col % 2); 
        }

        public override string ToString() => $"{Xcord}, {Ycord}"; // Overriding the ToString method to return the X and Y coordinates

        public override bool Equals(object obj) =>
            obj is DoubCoord other && Row == other.Row && Col == other.Col; // Overriding the Equals method to compare the row and column values

        public override int GetHashCode() => HashCode.Combine(Row, Col); // Overriding the GetHashCode method to return a hash code based on the row and column values

    }

    /// <summary>
    /// Represents a grid of hexes using a doubled coordinate system.
    /// </summary>

    public class HexGrid
    {
        private Dictionary<(int, int), DoubCoord> HexMap;
        public List<DoubCoord> Hexes { get; private set; }

        public HexGrid(int rows, int cols)
        {
            Hexes = new List<DoubCoord>(); // Creating a list to store the hexes
            HexMap = new Dictionary<(int, int), DoubCoord>(); // Using a dictionary to store the hexes with the row and column as the key

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var hex = new DoubCoord(row, col);
                    Hexes.Add(hex);
                    HexMap[(row, col)] = hex;
                }
            }
        }

        /// <summary>
        /// Retrieves all valid neighboring hexes of a given hex.
        /// </summary>
        public List<DoubCoord> GetNeighbors(DoubCoord hex)
        {
            var neighbors = new List<DoubCoord>();
            var offsets = new (int RowOffset, int ColOffset)[]
            {
                (-1, 0), (-1, 1), (0, -1), (0, 1), (1, 0), (1, 1)
            };

            foreach (var (rowOffset, colOffset) in offsets)
            {
                if (HexMap.TryGetValue((hex.Row + rowOffset, hex.Col + colOffset), out var neighbor)) // Checking if the neighbor exists in the HexMap
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }
    }
}