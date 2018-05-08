using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;   // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// A PigWorld is made up of a 2-dimensional grid of Cells. Each Cell is squarish in
    /// shape, and represents a piece of land.
    /// 
    /// Here are the rules about what can be in a particular cell, at any one time.
    /// 
    /// 1.   There can be ONE (at most) LifeForm in the cell, e.g. a pig or a tree.
    ///      When there is, the LifeFormOccupant property is a reference to that LifeForm.
    ///      When there isn't, LifeFormOccupant is null.
    /// 
    /// 2.   There can be MANY NonlivingThings in the cell, e.g. pig-food and pieces of rope.
    ///      But there can be restrictions on the number of each type of NonlivingThing
    ///      in the cell, e.g. there can be no more than one PigFood object in the cell.
    ///      The only exception to this is that any number of pieces of rope may be placed
    ///      in a single cell.
    ///      (This is specified by each NonlivingThing's OnlyOneObjectOfThisTypePerCell property.)
    /// 
    /// 3.   If the cell contains a LifeForm that is an Animal (e.g. a pig),
    ///      then the cell may contain that LifeForm and NonlivingThings at the same time.
    /// 
    /// 4.   If the cell contains a LifeForm that is a Plant (e.g. a tree),
    ///      then it contains NO NonlivingThings.  This is because Plants don't move,
    ///      and so LifeForms can never move into a cell containing a Plant.  Hence,
    ///      if NonlivingThings were allowed to occupy the same cell as a Plant,
    ///      then LifeForms would never be able to access the NonlivingThings.
    ///      So, these are not allowed.
    /// 
    /// When a cell contains a LifeForm and NonlivingThings, then the GUI (CellView) displays
    /// only the LifeForm, so that things don't become too obscure.
    /// 
    /// To access NonLivingThings sitting on a Cell, use the Exists(), Inspect(), PickUp()
    /// and PutDown() methods.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class Cell {

        private PigWorld pigWorld;  // A reference to the pigWorld in which this Cell exists.
        public PigWorld PigWorld { get { return pigWorld; } }

        private Position position;  // The location of this Cell in the pigWorld.
        public Position Position { get { return position; } }

        private Air air;  // The air in this Cell.
        public Air Air { get { return air; } }  // Returns a reference to the air in this Cell.

        // A reference to the LifeForm standing on this Cell, 
        // or null if no LifeForm is standing on this Cell.
        private LifeForm lifeFormOccupant;
        public LifeForm LifeFormOccupant { get { return lifeFormOccupant; } }

        // There can be many NonlivingThings existing in a single Cell.  
        // "A table of NonlivingThings mapped from Type to a List of objects."
        private Dictionary<Type, List<NonlivingThing>> nonLivingThings = new Dictionary<Type, List<NonlivingThing>>();

        public delegate void NonlivingThingChangedEvent(NonlivingThing nonlivingThing);
        // Initialise each with an empty delegate, to avoid null-check.
        public NonlivingThingChangedEvent nonlivingThingPutDownEvent = delegate { };
        public NonlivingThingChangedEvent nonlivingThingPickedUpEvent = delegate { };

        // This event is used when the cell changes in some way 
        // other than having a nonlivingThing put-down or picked-up.
        public delegate void CellChangedEvent();
        // Initialise each with an empty delegate, to avoid null-check.
        public CellChangedEvent cellChangedEvent = delegate { };

        /// <summary>
        /// Constructs a new Cell.
        /// </summary>
        /// <param name="pigWorld"></param>
        /// <param name="position"></param>
        public Cell(PigWorld pigWorld, Position position) {
            this.pigWorld = pigWorld;
            this.position = position;
            air = new Air(this);
        }

        /// <summary>
        /// Gets a reference to a neighbouring Cell (if any) in the specified direction.
        /// The direction will be automatically rounded to the nearest Cell.
        /// 
        /// If there is a wall in that direction, then null is returned.
        /// </summary>
        /// <param name="direction"> the direction in which to grab the neighbouring Cell. </param>
        /// <returns> the neighbouring Cell in the specified direction. </returns>
        public Cell GetAdjacentCell(Direction direction) {
            double radians = direction.GetRadians();

            int relativeRow = - (int)Math.Round(Math.Cos(radians));
            int relativeColumn = (int)Math.Round(Math.Sin(radians));

            int targetRow = position.Row + relativeRow;
            int targetColumn = position.Column + relativeColumn;

            Cell target = pigWorld.GetCell(new Position(targetRow, targetColumn));
            if (target == null)
                return null;

            // If there's a wall between this cell and the target cell ...
            if (pigWorld.IsWallBetweenCells(this, target))
                return null;

            return target;
        }

        /// <summary>
        /// Returns true if a NonlivingThing of the specified type exists on this
        /// Cell, or false otherwise.
        /// </summary>
        /// <param name="type"> the type of NonlivingThing to look for, e.g. typeof(PigFood). </param>
        /// <returns> true if a NonlivingThing of the specified type exists on this
        /// Cell, or false otherwise. </returns>
        public bool Exists(Type type) {  
            List<NonlivingThing> list;
            bool listExists = nonLivingThings.TryGetValue(type, out list);
            return (listExists && list.Count > 0);
        }

        /// <summary>
        /// If a NonlivingThing of the specified type exists on this Cell, a reference to
        /// it is returned. If there is more than one such nonlivingThing on this Cell,
        /// just the first of them is returned. If no such nonlivingThing is on this Cell,
        /// null is returned. If there are many nonLivingThings of the specified type on
        /// this Cell, InspectAll may be used to obtain a list of all of them.
        /// </summary>
        /// <param name="type"> the type of nonlivingThing to request. </param>
        /// <returns> a reference to a nonlivingThing of the specified type that is on this
        /// Cell, or null if no such nonlivingThing is no this Cell. </returns>
        public NonlivingThing Inspect(Type type) {  
            List<NonlivingThing> list;
            bool listExists = nonLivingThings.TryGetValue(type, out list);
            if (!listExists || list.Count == 0)
                return null;
            return list[0];
        }

        /// <summary>
        /// Return a list of all nonLivingThings of the specified type that exist on this Cell.
        /// "T" is the type of nonlivingThing of interest.
        /// </summary>
        /// <returns> a list of all such nonLivingThings on this Cell. </returns>
        public List<T> InspectAll<T>() {
            Type type = typeof(T);
            List<NonlivingThing> list;
            bool listExists = nonLivingThings.TryGetValue(type, out list);
            if (!listExists) {
                list = new List<NonlivingThing>();
                nonLivingThings.Add(type, list);
            }
            return list.Cast<T>().ToList();
        }

        /// <summary>
        /// Tests whether there is room for the given LifeForm on this Cell. If
        /// there is already a LifeForm occupying this Cell, the answer is no
        /// (false). If the given LifeForm is a Plant and there are some
        /// nonLivingThings on this Cell, then the answer is also no. But apart from that,
        /// this method has no other reasons to return false and should return
        /// true in all other cases.
        /// </summary>
        /// <param name="lifeForm"> the LifeForm to test if there is any room for. </param>
        /// <returns> true if there is any room on this Cell for the given LifeForm,
        /// or false otherwise. </returns>
        public bool IsRoomFor(LifeForm lifeForm) {
            if (lifeFormOccupant != null) {
                return false;
            }
            if (lifeForm is Plant && GetCountOfNonlivingThings() > 0) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the number of nonLivingThings that exist on this Cell.
        /// </summary>
        /// <returns> the number of nonLivingThings on this Cell. </returns>
        private int GetCountOfNonlivingThings() {
            int count = 0;
            foreach (var list in nonLivingThings.Values) {
                count += list.Count;
            }
            return count;
        }

        /// <summary>
        /// A generic method to put a Thing on this Cell, whether it be a
        /// LifeForm or a NonlivingThing.
        /// </summary>
        /// <param name="thing"> the Thing to place on this Cell. </param>
        /// <returns> true if the Thing was successfully placed, or false otherwise. </returns>
        public bool AddThing(Thing thing) {
            if (thing is LifeForm) {
                return AddLifeForm((LifeForm)thing);
            } else {
                return PutDown((NonlivingThing)thing);
            }
        }

        /// <summary>
        /// If no LifeForm currently standing on this Cell, this method takes
        /// the LifeForm given in the parameter from its old Cell and places it onto
        /// this Cell to become the new lifeFormOccupant of this Cell.
        /// 
        /// If a LifeForm is already on this Cell, then this method does
        /// nothing.
        /// </summary>
        /// <param name="newOccupant"> the new lifeFormOccupant of this Cell. </param>
        /// <returns> true if the newOccupant was successfully placed, or false otherwise. </returns>
        public bool AddLifeForm(LifeForm newOccupant) {
            LifeForm oldOccupant = lifeFormOccupant;

            if (!IsRoomFor(newOccupant))
                return false;

            Cell sourceCell = newOccupant.Cell;
            if (sourceCell != null)
                sourceCell.Release();

            lifeFormOccupant = newOccupant;
            lifeFormOccupant.Cell = this;

            return true;
        }

        /// <summary>
        /// This method will attempt to put a NonlivingThing down on the ground of a cell.
        /// See rules 2-4 (at the top of this file) for restrictions on the number
        /// of each type of NonlivingThing in the cell, e.g. there can be no more
        /// than one PigFood object in the cell.
        /// 
        /// If you attempt to break that rule, then this method will return false.
        /// But when the PutDown succeeds, the method will return true.
        /// </summary>
        /// <param name="nonlivingThing"> the nonlivingThing to put down on the ground. </param>
        /// <returns> true if the nonlivingThing was successfully put down, or false
        /// otherwise. </returns>
        public bool PutDown(NonlivingThing nonlivingThing) {

            // Don't allow when occupant is a Plant (e.g. a tree) -- see rule 4, at top of this file.
            if (lifeFormOccupant != null && lifeFormOccupant is Plant)
                return false;

            // Don't allow when the cell already has a NonlivingThing of the same type (e.g. pig-food),
            // unless OnlyOneObjectOfThisTypePerCell is false -- see rule 2, at top of this file.
            Type type = nonlivingThing.GetType();
            if (Exists(type) && nonlivingThing.OnlyOneObjectOfThisTypePerCell) {
                return false;
            }

            List<NonlivingThing> list;
            bool listExists = nonLivingThings.TryGetValue(type, out list);
            if (!listExists) {
                list = new List<NonlivingThing>();
                nonLivingThings.Add(type, list);
            }

            // If the cell already contains the same object that is trying to be added, then complain!
            if (list.Contains(nonlivingThing))
                throw new Exception("You can't put that " + type.Name + " there. It's already there!");

            list.Add(nonlivingThing);
            nonlivingThing.Cell = this;
            nonlivingThingPutDownEvent(nonlivingThing);
            return true;
        }

        /// <summary>
        /// A generic method to remove a Thing from this Cell, whether it be a
        /// LifeForm or a NonlivingThing.
        /// </summary>
        /// <param name="thing"> the Thing to remove from this Cell. </param>
        public void RemoveThing(Thing thing) {
            if (thing is LifeForm) {
                Release();
            } else {
                PickUp((NonlivingThing)thing);
            }
        }

        /// <summary>
        /// This method releases the current lifeFormOccupant from this Cell. If there is
        /// currently no lifeFormOccupant, then this method does nothing.
        /// </summary>
        /// <returns> the LifeForm that was released, or null if there was nothing
        /// occupying this Cell. </returns>
        private LifeForm Release() {
            LifeForm removed = lifeFormOccupant;
            if (lifeFormOccupant != null) {
                lifeFormOccupant = null;
            }
            return removed;
        }

        /// <summary>
        /// This method is used to pick up a NonlivingThing off the ground of the cell/square.
        /// In general, you will need to cast the object returned from this method, e.g.
        ///     PigFood food = (PigFood)Cell.PickUp(typeof(PigFood));
        /// 
        /// The NonlivingThing will continue to exist, so it may be put down again using
        /// the PutDown() method. To stop a NonlivingThing from existing, you need to
        /// pick it up, and then destroy it, using its Delete() method. 
        /// The Eat method (in the Animal class) calls Delete().
        /// 
        /// It is an error to attempt to pick up a nonlivingThing that does not exist on
        /// this Cell. You should check that it exists, by using the Exists() method of
        /// this class first.
        /// </summary>
        /// <param name="type"> the type of nonlivingThing to pickup. </param>
        /// <returns> the NonlivingThing that was picked up. </returns>
        public NonlivingThing PickUp(Type type) {
            List<NonlivingThing> list;
            bool listExists = nonLivingThings.TryGetValue(type, out list);
            if (!listExists || list.Count == 0)
                throw new Exception(
                    "There is no " + type.Name + " on this cell. "
                    + "You should check with Exists() first before trying to pick one up.");

            NonlivingThing nonlivingThing = list[0];
            return PickUp(nonlivingThing);
        }

        /// <summary>
        /// Pick up a specific nonlivingThing. The NonlivingThing will continue to exist, so it
        /// may be put down again using the PutDown() method. To stop a NonlivingThing
        /// from existing, you need to pick it up, and then destroy it, using the
        /// Delete() method.
        /// 
        /// It is an error to attempt to pick up a nonlivingThing that does not exist on
        /// this Cell.
        /// </summary>
        /// <param name="nonlivingThing"> the type of nonlivingThing to pick up. </param>
        /// <returns> the nonlivingThing that was picked up. </returns>
        public NonlivingThing PickUp(NonlivingThing nonlivingThing) {
            Type type = nonlivingThing.GetType();
            List<NonlivingThing> list;
            bool listExists = nonLivingThings.TryGetValue(type, out list);
            if (!listExists || list.Count == 0)
                throw new Exception("Can't pick that nonlivingThing up. It's not there!");

            if (!list.Remove(nonlivingThing))
                throw new Exception("Can't pick that nonlivingThing up. It's not there!");

            nonlivingThing.Cell = null;

            nonlivingThingPickedUpEvent(nonlivingThing);

            return nonlivingThing;
        }

        /// <summary>
        /// The Air in this Cell calls back to this method when its state changes
        /// so that we can update our display. This is only applicable in debug
        /// mode when we need to display the state of air.
        /// </summary>
        public void AirChanged() {

            if (pigWorld.ShowDebugInfo)
                cellChangedEvent();
        }

        /// <summary>
        /// Makes this Cell cause its CellView to update what is displayed.
        /// </summary>
        public void Pulse() {
            cellChangedEvent();
        }

        /// <summary>
        /// Creates a string representation of this Cell. 
        /// This is useful for debugging purposes and any other times 
        /// when you want to have a string showing what value(s) an object has.
        /// </summary>
        /// <returns> a string representation of this Cell </returns>
        public override string ToString() {
            return "cell[" + position.Row + "," + position.Column + "]";
        }
    }
}
