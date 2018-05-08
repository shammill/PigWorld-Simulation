using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// A Direction object represents a direction in the 2-dimensional space of the pigWorld.
    /// Directions are mainly used by Animals as they move around the pigWorld.
    /// 
    /// The angle of a Direction is measured using the same convention as a magnetic compass.  
    /// North corresponds to zero degrees, and the angles increase clockwise, 
    /// so east is 90 degrees, south is 180, and west is 270.  Other angles are possible.
    ///
    /// The angle may be obtained by using either the Degrees property or
    /// the GetRadians() method.
    /// 
    /// A new Direction object may be created by specifying the degrees as a
    /// parameter to the constructor. In addition, common Directions have been
    /// defined as static members of this class. For example:
    /// "Direction.EAST" may be used instead of "new Direction(90.0)".
    /// 
    /// Direction objects are immutable.  I.e. once a Direction object has been created, 
    /// its degree value cannot be changed.  If you want a different direction, create a new one.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class Direction {

        // The eight named directions:
        public static readonly Direction NORTH = new Direction(0.0);  // A predefined Direction representing North.
        public static readonly Direction NORTH_EAST = new Direction(45.0);  // A predefined Direction representing Northeast.
        public static readonly Direction EAST = new Direction(90.0);  // A predefined Direction representing East.
        public static readonly Direction SOUTH_EAST = new Direction(135.0);  // A predefined Direction representing Southeast.
        public static readonly Direction SOUTH = new Direction(180.0);  // A predefined Direction representing South.
        public static readonly Direction SOUTH_WEST = new Direction(225.0);  // A predefined Direction representing Southwest.
        public static readonly Direction WEST = new Direction(270.0);  // A predefined Direction representing West.
        public static readonly Direction NORTH_WEST = new Direction(315.0);  // A predefined Direction representing Northwest.

        public const int NUMBER_POSSIBLE = 8;

        public const double ANGLE_INCREMENT = 360 / NUMBER_POSSIBLE;  // I.e. 45 degrees.

        // The angle of this direction, in degrees.  
        // Might be one of the named directions above, but may be a more general value, such as 66.123 degrees.
        // Direction objects are immutable. 
        // I.e. once a Direction object has been created, its degree value cannot be changed.
        private readonly double degrees;
        public double Degrees { get { return degrees; } }

        /// <summary>
        /// Constructs a new Direction object, specified in degrees.
        /// </summary>
        /// <param name="degrees"> the angle of this direction, specified in degrees. </param>
        public Direction(double degrees) {
            if (degrees < 0.0)
                throw new Exception("Can't have a direction < 0 degrees");
            if (degrees >= 360.0)
                throw new Exception("Can't have a direction >= 360 degrees");

            this.degrees = degrees;
        }

        /// <summary>
        /// The eight cells that are adjacent to a given cell (where they exist) are in the eight 
        /// directions named above.  This method allows each of those eight directions to be created
        /// easily in a loop where the directionIndex is in the range 0 to 7, e.g.
        /// 
        ///     for (int i = 0; i < Direction.NUMBER_POSSIBLE; i++) {
        ///         Direction direction = Direction.GetAdjacentCellDirection(i);
        ///         Cell adjacentCell = Cell.GetAdjacentCell(direction);
        ///         // Do something with the direction and/or adjacentCell.
        ///         ...
        ///     } // endfor
        ///     
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Direction GetAdjacentCellDirection(int directionIndex) {
            return new Direction(directionIndex * ANGLE_INCREMENT);
        }

        public static Direction GetRandomDirection() {
            return new Direction(Util.random.Next(NUMBER_POSSIBLE) * ANGLE_INCREMENT);
        }

        /// <summary>
        /// Returns a new direction object that points in the opposite direction to
        /// this one.
        /// </summary>
        /// <returns> the opposite direction to this direction. </returns>
        public Direction GetOppositeDirection() {
            return GetRelativeDirection(180.0);
        }

        /// <summary>
        /// Gets a new direction relative to this direction by adding the number of
        /// degrees specified. For example:
        /// 
        ///     Direction east = new Direction(20.0);
        ///     Direction newDirection = east.GetRelativeDirection(30.0);
        /// 
        /// newDirection.Degrees should return 50.0.
        /// </summary>
        /// <param name="degrees"> specifies how many degrees should be added to produce
        /// the new direction. </param>
        /// <returns> the new relative direction. </returns>
        public Direction GetRelativeDirection(double degrees) {
            double newDegrees = this.degrees + degrees;
            newDegrees = newDegrees % 360.0;
            if (newDegrees < 0.0)
                newDegrees += 360.0;

            return new Direction(newDegrees);
        }

        /// <summary>
        /// Gets the angle of this Direction in radians.
        /// </summary>
        /// <returns> the angle of this Direction in radians. </returns>
        public double GetRadians() {
            return Util.GetRadiansFromDegrees(degrees);
        }

        /// <summary>
        /// Returns information about the Direction object as a string.
        /// 
        /// It can be very useful to have a "ToString" method in any class you write.
        /// It allows you to use a reference to an object of that class as if that
        /// object is a string.  For example, you can use the name of the object in
        /// some kind of output -- as if the object is a string -- and the C#
        /// runtime environment will use your "ToString" to produce a real string.
        /// </summary>
        /// <returns> information about the Direction object as a string </returns>
        public override string ToString() {
            if (this.degrees == NORTH.degrees) return "Direction.NORTH";
            if (this.degrees == NORTH_EAST.degrees) return "Direction.NORTH_EAST";
            if (this.degrees == EAST.degrees) return "Direction.EAST";
            if (this.degrees == SOUTH_EAST.degrees) return "Direction.SOUTH_EAST";
            if (this.degrees == SOUTH.degrees) return "Direction.SOUTH";
            if (this.degrees == SOUTH_WEST.degrees) return "Direction.SOUTH_WEST";
            if (this.degrees == WEST.degrees) return "Direction.WEST";
            if (this.degrees == NORTH_WEST.degrees) return "Direction.NORTH_WEST";
            return ("direction=" + degrees);
        }
    }
}
