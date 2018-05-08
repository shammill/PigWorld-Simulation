using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// An Echo object holds information returned from the Ping() and Sweep() methods
    /// in the Radar class. An Echo contains information about the Thing detected,
    /// including the typeOfThing, its direction, and its distance.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class Echo {

        public readonly Type typeOfThing;  // The type of thing detected.

        public readonly Direction direction;  // The direction of the thing detected.

        public readonly double distance;  // The distance to the thing detected.

        /// <summary>
        /// Constructs a new Echo object.
        /// </summary>
        /// <param name="typeOfThing"> the type of thing detected. </param>
        /// <param name="direction"> the direction of the thing detected. </param>
        /// <param name="distance"> the distance of the thing detected. </param>
        public Echo(Type typeOfThing, Direction direction, double distance) {  
            this.typeOfThing = typeOfThing;
            this.direction = direction;
            this.distance = distance;
        }

        /// <summary>
        /// Returns all information about the echo as a string.
        /// 
        /// It can be very useful to have a "ToString" method in any class you write,
        /// for debugging purposes and any other times 
        /// when you want to have a string showing what value(s) an object has.
        /// </summary>
        /// <returns> all information about the echo as a string </returns>
        public override string ToString() {
            return "Echo(typeOfThing=" + typeOfThing + ", direction=" + direction +
                    ", distance=" + distance + ")";
        }
    }
}
