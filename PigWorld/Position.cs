using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// This class is simply used to hold a row and column combination. 
    /// Using this class makes other code clearer and more concise.
    /// E.g. compared to having to pass separate row and column values as parameters,
    /// you can use one Position parameter rather than two values.
    /// Positions are used in many places, in PigWorld.
    /// 
    /// Author: Jim Reye
    /// </summary>
    public class Position {

        public static readonly Position any = new Position(-1, -1);

        private int row;
        public int Row {
            get { return row; }
            set { row = value; }
        }

        private int column;
        public int Column {
            get { return column; }
            set { column = value; }
        }

        /// <summary>
        /// Construct a Position.
        /// </summary>
        /// <param name="row"> the position's row </param>
        /// <param name="column"> the position's column</param>
        public Position(int row, int column) {
            this.row = row;
            this.column = column;
        }

        /// <summary>
        /// Creates a string representation of this Position. 
        /// This is useful for debugging purposes and any other times 
        /// when you want to have a string showing what value(s) an object has.
        /// </summary>
        /// <returns> a string representation of this Position </returns>
        public override string ToString() {
            return "[" + row + "," + column + "]";
        }
    }
}
