using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// A NonlivingThing is a static, non-moving entity which just sits on the ground 
    /// of a Cell.  Examples are PigFood and RopePieces.
    /// 
    /// NonlivingThings may be found and picked up by Animals. A Animal should
    /// first move to some target Cell, check if there is a NonlivingThing on it, and
    /// then pick it up. To accomplish this, the Cell class provides Exists() and
    /// PickUp() methods to discover and pick up NonlivingThings.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public abstract class NonlivingThing : Thing {

        /// <summary>
        /// There can be restrictions on the number of each type of NonlivingThing
        /// in a cell, e.g. there can be no more than one PigFood object in a given cell.
        /// See the rules at the top of Cell.cs for more information about what can be in a cell.
        /// </summary>
        private bool onlyOneObjectOfThisTypePerCell;
        public bool OnlyOneObjectOfThisTypePerCell { 
            get { return onlyOneObjectOfThisTypePerCell; }
            set { onlyOneObjectOfThisTypePerCell = value; }         
        }

        /// <summary>
        /// Creates a nonlivingThing. By default, onlyOneObjectOfThisTypePerCell is true.
        /// </summary>
        protected NonlivingThing() {
            this.onlyOneObjectOfThisTypePerCell = true;
        }

    }
}
