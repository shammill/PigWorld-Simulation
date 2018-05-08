using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    public enum GapDirection { Horizontal, Vertical };

    /// <summary>
    /// Between each pair of Cells is a Gap.  Gaps can be filled with walls 
    /// which are used to block the pathways from one Cell to the next.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class Gap {

        private bool hasWall;
        public bool HasWall { 
            get { return hasWall; }
            set { hasWall = value; gapChangedEvent();  }
        }

        private Position position;  // The position of this gap.

        private GapDirection gapDirection;  // The direction of this gap (either Horizontal or Vertical).

        public delegate void GapChangedEvent();
        // Initialise each with an empty delegate, to avoid null-check.
        public GapChangedEvent gapChangedEvent = delegate { };

        /// <summary>
        /// Constructs a new gap, at the specified position and GapDirection
        /// (either HORIZONTAL or VERTICAL).
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gapDirection"></param>
        public Gap(Position position, GapDirection gapDirection) {
            this.position = position;
            this.gapDirection = gapDirection;
            HasWall = false;
        }

    }
}
