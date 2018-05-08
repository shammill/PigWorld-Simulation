using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;       // For Point
using System.Diagnostics;   // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// A PigWorld is made up from a grid of square-shaped Cells. A Cell can have many
    /// "Things" on it. There are two major types of Things: "LifeForms" are living
    /// entities which have behaviour and may be able to move around (for example, animals),
    /// while "NonlivingThings" are static entities which just sit on the ground if a Cell
    /// and don't move (for example, food).
    /// 
    /// When a new Thing is created, this PigWorld should be passed as a parameter to
    /// that Thing's constructor. The new Thing will then be automatically added to
    /// this PigWorld.
    /// 
    /// When the pigWorld is created, time will initially stand still. To make the pigWorld 
    /// move forward one "step" in time, you need to call the Step() method.
    /// Every time it steps, the pigWorld will ask every LifeForm to do something by 
    /// calling the DoSomething() method of that LifeForm.  In a single unit of time, 
    /// each particular LifeForm should just do one small thing, such as moving one cell. 
    /// If an LifeForm moves more than one Cell in a single unit of time, then 
    /// it will appear as though that LifeForm had teleported through space! 
    /// NonlivingThings do not do anything, so they do not have a DoSomething() method.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class PigWorld {

        private int numOfRows;  // The number of rows in this pigWorld.
        public int NumOfRows { get { return numOfRows; } }

        private int numOfColumns;  // The number of columns in this pigWorld.
        public int NumOfColumns { get { return numOfColumns; } }

        private Cell[,] cells;  // A 2-dimensional array of cells in this pigWorld.

        private int numOfHorizontalGapsPerColumn;
        public int NumOfHorizontalGapsPerColumn { get { return numOfHorizontalGapsPerColumn; } }

        private int numOfVerticalGapsPerRow;
        public int NumOfVerticalGapsPerRow { get { return numOfVerticalGapsPerRow; } }

        private Gap[,] horizontalGaps;  // A 2-dimensional array of horizontal gaps in this pigWorld.

        private Gap[,] verticalGaps;  // A 2-dimensional array of vertical gaps in this pigWorld.

        private List<Thing> things = new List<Thing>();  // A list of every thing in the pigWorld.
        public List<Thing> Things { get { return things; } }

        // This value is used when a GirlPig grunts.
        // The value 20 is good because it is not too loud that the pigWorld will be 
        // saturated in sound, and not to soft that nobody far away can hear it.
        public const int OINK_SOUND_LEVEL = 20;

        private int now = 0;  // The current point in time.
        public int Now { get { return now; } }

        private bool showDebugInfo = false;  // When true, more information may be displayed to the program developer.
        public bool ShowDebugInfo {
            get { return showDebugInfo; }
            set { showDebugInfo = value; showDebugInfoChangedEvent(); }
        }

        private bool enableRealAudio = false;  // When true, real audio sounds may be played.
        public bool EnableRealAudio {
            get { return enableRealAudio; }
            set { enableRealAudio = value; }
        }

        public delegate void ShowDebugInfoChangedEvent();
        // Initialise each with an empty delegate, to avoid null-check.
        public ShowDebugInfoChangedEvent showDebugInfoChangedEvent = delegate { };

        public delegate void WorldThingChangedEvent(Thing thing);
        // Initialise each with an empty delegate, to avoid null-check.
        public WorldThingChangedEvent worldThingAddedEvent = delegate { };
        public WorldThingChangedEvent worldThingRemovedEvent = delegate { };

        /// <summary>
        /// Constructs a new PigWorld, with the specified number of columns and rows.
        /// </summary>
        /// <param name="numOfRows"> the number of rows in the pigWorld. </param>
        /// <param name="numOfColumns"> the number of columns in the pigWorld. </param>
        public PigWorld(int numOfRows, int numOfColumns) {

            this.numOfRows = numOfRows;
            this.numOfColumns = numOfColumns;

            numOfHorizontalGapsPerColumn = numOfRows - 1;
            numOfVerticalGapsPerRow = numOfColumns - 1;
            CreateCells();
            CreateGaps();
        }

        /// <summary>
        /// Creates all the Cells in this pigWorld.
        /// </summary>
        private void CreateCells() {
            cells = new Cell[numOfRows, numOfColumns];
            for (int row = 0; row < numOfRows; row++) {
                for (int column = 0; column < numOfColumns; column++) {
                    Position position = new Position(row, column);
                    Cell cell = new Cell(this, position);
                    cells[row, column] = cell;
                }
            }
        }

        /// <summary>
        /// Creates all the Gaps in this pigWorld, 
        /// both the Horizontal ones and the Vertical ones.
        /// </summary>
        private void CreateGaps() {

            horizontalGaps = new Gap[numOfHorizontalGapsPerColumn, numOfColumns];
            for (int row = 0; row < numOfHorizontalGapsPerColumn; row++) {
                for (int column = 0; column < numOfColumns; column++) {
                    Position position = new Position(row, column);
                    Gap gap = new Gap(position, GapDirection.Horizontal);
                    horizontalGaps[row, column] = gap;
                }
            }

            verticalGaps = new Gap[numOfRows, numOfVerticalGapsPerRow];
            for (int row = 0; row < numOfRows; row++) {
                for (int column = 0; column < numOfVerticalGapsPerRow; column++) {
                    Position position = new Position(row, column);
                    Gap gap = new Gap(position, GapDirection.Vertical);
                    verticalGaps[row, column] = gap;
                }
            }
        }

        /// <summary>
        /// Gets the cell at the specified Position, if it is a valid Position on the grid.
        /// Otherwise return null to indicate that an animal may be trying to step off the edge of the pigWorld.
        /// </summary>
        public Cell GetCell(Position position) {

            if ((position.Row >= 0) && (position.Row < NumOfRows)
              && (position.Column >= 0) && (position.Column < NumOfColumns)) {
                return cells[position.Row, position.Column];
            } else {
                return null;
            }
        }

        /// <summary>
        /// Returns a reference to the Gap at the specified row and column.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="gapDirection"></param>
        /// <returns></returns>
        public Gap GetGap(int row, int column, GapDirection gapDirection) {

            if (gapDirection == GapDirection.Horizontal)
                return horizontalGaps[row, column];
            else if (gapDirection == GapDirection.Vertical)
                return verticalGaps[row, column];
            else
                throw new Exception("Invalid gap direction: " + gapDirection);
        }

        /// <summary>
        /// Returns true when there is a wall between two adjacent cells.
        /// I.e. their Row and Column values differ by one, at most, although both may differ,
        /// i.e. adjacent diagonally.
        /// 
        /// If they are adjacent diagonally, then this method returns true when there is
        /// a wall in either direction (horizontally or vertically).  This prevents Animals
        /// moving past the corner of a wall.
        /// 
        /// Precondition: the two cells are adjacent.
        /// </summary>
        /// <param name="cell1"></param>
        /// <param name="cell2"></param>
        /// <returns></returns>
        public bool IsWallBetweenCells(Cell cell1, Cell cell2) {
            Position pos1 = cell1.Position;
            Position pos2 = cell2.Position;

            // Is there a horizontal wall?
            if (pos1.Row != pos2.Row) {
                int row = Math.Min(pos1.Row, pos2.Row);
                if (horizontalGaps[row, pos1.Column].HasWall) {
                    return true;
                }
                if (horizontalGaps[row, pos2.Column].HasWall) {
                    return true;
                }
            }

            // Is there a vertical wall?
            if (pos1.Column != pos2.Column) {
                int column = Math.Min(pos1.Column, pos2.Column);
                if (verticalGaps[pos1.Row, column].HasWall) {
                    return true;
                }
                if (verticalGaps[pos2.Row, column].HasWall) {
                    return true;
                }
            }
        
            return false;
        }
    
        /// <summary>
        /// Fills a horizontal gap with a Wall.
        /// </summary>
        /// <param name="row"> the row position of the horizontal gap to be filled. </param>
        /// <param name="column"> the column position of the horizontal gap to be filled. </param>
        public void FillHorizontalGap(int row, int column) {
            Gap gap = horizontalGaps[row, column];
            gap.HasWall = true;
        }

        /// <summary>
        /// Fills a vertical gap with a Wall.
        /// </summary>
        /// <param name="row"> the row position of the vertical gap to be filled. </param>
        /// <param name="column"> the column position of the vertical gap to be filled. </param>
        public void FillVerticalGap(int row, int column) {
            Gap gap = verticalGaps[row, column];
            gap.HasWall = true;
        }

        /// <summary>
        /// This method steps through one unit of time in the pigWorld. It loops over
        /// every LifeForm in the pigWorld and calls the DoSomething() method on each
        /// LifeForm once. You can use the Step method while the pigWorld is stopped to
        /// see how the pigWorld changes after one unit of time. 
        /// To make multiple steps, this method can be called repeatedly, 
        /// e.g. by the periodic tick of a timer.
        /// </summary>
        public virtual void Step() {

            now += 1;

            // Make any sounds in the air dampen over time.
            if (now % 3 == 0) {
                for (int row = 0; row < numOfRows; row++) {
                    for (int column = 0; column < numOfColumns; column++) {
                        cells[row, column].Air.DecrementSoundLevel();
                    }
                }
            }

            // Update each Thing in the pigWorld, for this moment in time.

            // Items can be added to, and removed from, the list of Things as each LifeForm gets 
            // its slice of time. E.g. pigs can be born, or eaten by a wolf.
            // To make sure that only existing Things get their time-slice on the current Step,
            // we make a copy of the list first.
            List<Thing> localListOfThings = new List<Thing>(Things);

            for (int i = 0; i < localListOfThings.Count; i++) {  
                // The Things update themselves, so we can't use "foreach" here.
                Thing thing = localListOfThings[i];

                if (thing is LifeForm) {
                    LifeForm lifeForm = (LifeForm)thing;
                    if (lifeForm.Exists())
                        lifeForm.HandleTime();
                }
                else {
                    // Refresh the display of NonlivingThings periodically.
                    if (now % 6 == 0) {
                        Cell cell = thing.Cell;
                        if (cell != null)
                            cell.Pulse();
                    }
                }
            }
        }

        /// <summary>
        /// Adds a thing of the specified Type to the pigWorld at the specified position.
        /// This method should ONLY BE USED by GUI View classes when the type of object
        /// is NOT known at compilation time, i.e. is selected dynamically by the user.
        /// On the other hand, when the type of object IS KNOWN at compilation time,
        /// then the following AddThing method should be used instead, as it provides
        /// better checking at compilation time, rather than execution time.
        /// 
        /// This AddThingOfType method calls the pigWorld's AddThing method (indirectly),
        /// so do not call both methods when adding a new object.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        public void AddThingOfType(Type type, Position position) {
            string typeName = type.Name;

            switch (typeName) {
                case "BoyPig": new BoyPig(this, position); break;
                case "GirlPig": new GirlPig(this, position); break;
                case "PigFood": new PigFood(this, position); break;
                case "Tree": new Tree(this, position); break;
                case "Wolf": new Wolf(this, position); break;
                default: throw new Exception("Unexpected type-name " + typeName);
            }
        }

        /// <summary>
        /// Adds a Thing to the pigWorld at the specified position.
        /// If there is not enough room in that Cell, then a nearby Cell will be used instead.
        /// If no nearby Cells are free, then the Thing will not be added to the pigWorld.
        /// 
        /// If Position.any.Row is specified for the position's row, then a random row will be chosen.
        /// If Position.any.Column is specified for the position's column, then a random column will be chosen.
        /// </summary>
        /// <param name="thing"></param>
        /// <param name="position"></param>
        /// <returns> true if the Thing was successfully added, or false otherwise. </returns>
        public bool AddThing(Thing thing, Position position) {

            if (position.Row == Position.any.Row)
                position.Row = Util.random.Next(numOfRows);
            if (position.Column == Position.any.Column)
                position.Column = Util.random.Next(numOfColumns);
        
            Cell targetCell = GetCell(position);

            bool success = AddThingToCell(thing, targetCell);

            if (!success) {
                for (int i = 0; i < Direction.NUMBER_POSSIBLE; i++) {
                    Direction direction = Direction.GetAdjacentCellDirection(i);
                    Cell adjacentCell = targetCell.GetAdjacentCell(direction);

                    if (adjacentCell != null) {
                        if (AddThingToCell(thing, adjacentCell)) {
                            success = true;
                            break;
                        }
                    }
                } // endfor
            }

            if (success) {
                worldThingAddedEvent(thing);
            }

            return success;
        }
    
        /// <summary>
        /// A private method to add a Thing to a particular cell. This method sets
        /// the creation date of the Thing, adds the thing to the Cell, and creates
        /// a controller for the controller panel.
        /// </summary>
        /// <param name="thing"> the Thing to add. </param>
        /// <param name="cell"> the Cell that the thing will be added to. </param>
        /// <returns> true if the thing was successfully added, or false otherwise. </returns>
        private bool AddThingToCell(Thing thing, Cell cell)  {

            if (cell.AddThing(thing)) {
                thing.SetCreationDate(now);
                things.Add(thing);
                return true;
            }
            else {
                return false;
            }
        }
    
        /// <summary>
        /// Removes the specified Thing from the pigWorld.
        /// 
        /// Note: This method should only be called by LifeForm.Delete() or NonlivingThing.Delete().
        /// </summary>
        public void RemoveThing(Thing thing) {

            worldThingRemovedEvent(thing);

            Cell cell = thing.Cell;
            if (cell != null)
                cell.RemoveThing(thing);

            things.Remove(thing);
        }

        /// <summary>
        /// Removes all Things and all walls from this pigWorld.
        /// </summary>
        public virtual void RemoveAll() {
            RemoveAllThings();
            RemoveAllWalls();
        }
    
        /// <summary>
        /// Removes every Thing from the pigWorld, but will leave the Walls in place.
        /// </summary>
        public void RemoveAllThings() {

            // Because the Delete method updates the list of Things, we make a copy of the list first.
            List<Thing> localListOfThings = new List<Thing>(Things);

            for (int i = 0; i < localListOfThings.Count; i++) {  
                // The Things update themselves, so we can't use "foreach" here.
                Thing thing = localListOfThings[i];
                if (!thing.Exists())
                    continue;
                thing.Delete();
            }

            LifeForm.ResetAllIds();
        }
    
        /// <summary>
        /// Removes all walls from the pigWorld.
        /// </summary>
        public void RemoveAllWalls() {
            for (int row = 0; row < numOfRows; row++) {
                for (int column = 0; column < numOfColumns; column++) {
                    if (row < numOfHorizontalGapsPerColumn) {
                        horizontalGaps[row, column].HasWall = false;
                    }
                    if (column < numOfVerticalGapsPerRow) {
                        verticalGaps[row, column].HasWall = false;
                    }
                }
            }
        }
    
        /// <summary>
        /// Gets the distance from one Thing to another Thing.
        /// </summary>
        /// <param name="source"> the source Thing. </param>
        /// <param name="target"> the target Thing. </param>
        /// <returns> the distance between source and target. </returns>
        public double GetDistance(Thing source, Thing target) {
            Position p1 = source.Cell.Position;
            Position p2 = target.Cell.Position;
            int xdist = Math.Abs(p2.Column - p1.Column);
            int ydist = Math.Abs(p2.Row - p1.Row);

            // using a Manhattan metric
            if (xdist > ydist)
                return xdist;
            else
                return ydist;

            // for Euclidean metric return Math.Sqrt(xdist * xdist + ydist * ydist);
        }

        /// <summary>
        /// Gets the direction of a "target" Thing, from a "source" Thing.
        /// </summary>
        /// <param name="source"> the source Thing. </param>
        /// <param name="target"> the target Thing. </param>
        /// <returns> the direction of "target" from "source". </returns>
        public Direction GetDirection(Thing source, Thing target) {
            return GetDirection(source.Cell, target.Cell);
        }

        /// <summary>
        /// Gets the direction of a "target" Cell, from a "source" Cell.
        /// </summary>
        /// <param name="source"> the source Cell. </param>
        /// <param name="target"> the target Cell. </param>
        /// <returns> the direction of "target" from "source". </returns>
        public Direction GetDirection(Cell source, Cell target) {
            Position sourcePos = source.Position;
            Position targetPos = target.Position;

            if ((sourcePos.Row == targetPos.Row) && (sourcePos.Column == targetPos.Column)) {
                throw new Exception("GetDirection: source and target on same cell");
            }

            if (sourcePos.Column == targetPos.Column) {
                if (sourcePos.Row > targetPos.Row) {
                    return (Direction.NORTH);
                } else {
                    return (Direction.SOUTH);
                }
            }

            if (sourcePos.Row == targetPos.Row) {
                if (sourcePos.Column > targetPos.Column) {
                    return (Direction.WEST);
                } else {
                    return (Direction.EAST);
                }
            }

            // Set the relative vector, using Cartesian (x,y) coordinates, rather than row & column values.
            Point targetVector = new Point(targetPos.Column - sourcePos.Column, -(targetPos.Row - sourcePos.Row));
            double angleInDegrees;
            double angleInRadians = Math.Atan2((double)targetVector.Y, (double)targetVector.X);
            angleInDegrees = Util.GetDegreesFromRadians(angleInRadians);
            Debug.Assert((-180 <= angleInDegrees) && (angleInDegrees <= 180));
            
            // Convert into the range [0, 360].
            if (angleInDegrees < 0)
                angleInDegrees += 360;
            
            // Convert from a anti-clockwise Cartesian angle, relative to the x-axis,
            // to a clockwise one, relative to North.
            angleInDegrees = (360 - angleInDegrees) + 90;
            if (angleInDegrees >= 360) {
                angleInDegrees -= 360;
            }
            return new Direction(angleInDegrees);
        }
    }
}
