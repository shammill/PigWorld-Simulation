using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// RopePieces are dropped by pigs when they are looking for food. 
    /// The trail of rope indicates where a pig has been, so that they 
    /// know the places that they've already explored.
    /// 
    /// Original author: unknown
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class RopePiece : NonlivingThing {

        private Pig ownerPig;  // The owner of this piece of rope.
        public Pig OwnerPig { get { return ownerPig; } }

        // Gets the colour of this rope. This will be the same colour as the
        // colour of the pig that owns this rope.
        public Color Color { get { return ownerPig.Color; } }

        /// <summary>
        /// Constructs a new piece of rope.
        /// </summary>
        /// <param name="pigWorld"></param>
        /// <param name="ownerPig"></param>
        /// <param name="position"></param>
        public RopePiece(PigWorld pigWorld, Pig ownerPig, Position position) {
            this.ownerPig = ownerPig;

            // Allow more than one ropePiece in a cell.  (See comments at top of Cell.cs.)
            OnlyOneObjectOfThisTypePerCell = false;

            AddToWorld(pigWorld, position);
            if (this.PigWorld == null)
                throw new Exception("Error: Rope could not be created!");
        }

        /// <summary>
        /// Gets the distance of this piece of rope from its owner. As the owner
        /// pig continues extending the rope while looking for food, the length
        /// returned by this method will increase, for any existing piece of rope.
        /// </summary>
        /// <returns> distance of this piece of rope from its owner </returns>
        public int GetDistanceFromOwner() {
            int index = GetIndex();
            return ownerPig.ropePieces.Count - index;
        }

        /// <summary>
        /// Gets the position of this piece of rope in its owner's collection of ropePieces.
        /// </summary>
        /// <returns> the position of this piece of rope in its owner's collection </returns>
        public int GetIndex() {
            int index = ownerPig.ropePieces.IndexOf(this);
            Debug.Assert(index >= 0);  // Confirm that this piece DOES exist in the collection.
            return index;
        }
    }
}
