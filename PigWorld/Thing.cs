using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// A "Thing" represents some object in the pigWorld, such as a pig, or
    /// a piece of pig food. Each thing exists in a particular Cell in the pigWorld.
    /// Using a Thing's Cell property will return the Cell where the Thing is located. 
    /// The PigWorld property will return a reference to the PigWorld in which this Thing exists.
    /// 
    /// There are two major (sub)types of Things: NonlivingThings and LifeForms.
    /// 
    /// A NonlivingThing is a static, non-moving entity which just sits on the ground 
    /// of a Cell. An example is PigFood.
    /// 
    /// A LifeForm is a "living" entity that has behaviour.
    /// One type of LifeForm is Animal, which can move around from Cell to Cell.
    /// Another type of LifeForm is Plant which is stationary, but can still
    /// perform actions. A Tree for example, is a Plant that produces PigFood
    /// as its behaviour.
    /// 
    /// As each unit of time passes in the pigWorld, the DoSomething() method of each
    /// LifeForm will be called. Each subclass of LifeForm should implement this
    /// method to define how the LifeForm will behave as time passes. For example, 
    /// when a Pig has to "do something", it might move one Cell in the
    /// direction of the nearest piece of food. A Pig should not move more than one
    /// Cell, or eat more than one piece of food in a single unit of time.
    /// 
    /// Each Thing has an amount of energy, which can be discovered through the
    /// Energy property. If a Thing is eaten, then its energy is transferred to
    /// whatever ate it. For example, a Pig's energy will increase after it eats
    /// PigFood by the amount of energy inside that piece of PigFood.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public abstract class Thing {

        private int creationTime;  // The time when this Thing was created.
        public int Age { get { return pigWorld.Now - creationTime; } }  // Gets the age of this Thing.

        private PigWorld pigWorld;  // A reference to the PigWorld in which this Thing exists.
        public PigWorld PigWorld { get { return pigWorld; } }

        private Cell cell;  // A reference to the current Cell this Thing is sitting on.
        public Cell Cell { 
            get { return cell; }
            set { cell = value; }
        }

        private int energy;  // The current amount of energy in this Thing: LifeForm or food.
        public int Energy { 
            get { return energy; }
            set { energy = value; }
        }

        public delegate void ThingEvent();
        // Initialise each with an empty delegate, to avoid null-check.
        public ThingEvent thingChangedEvent = delegate { };  // Raised in derived classes only.
        public ThingEvent thingDestroyedEvent = delegate { };  // Raised in this class only.

        /// <summary>
        /// Sets the creation date of this Thing. This method should only be
        /// called by code in the PigWorld class, not by code in other classes.
        /// </summary>
        /// <param name="date"> the creation date of this Thing </param>
        public void SetCreationDate(int time) {
            creationTime = time;
        }

        /// <summary>
        /// Every subclass MUST call this method, or the second AddToWorld()
        /// method, as the last line of its constructor. This method will complete
        /// the creation of a new Thing by making it appear in the pigWorld. 
        /// The Thing will be placed at a random location.
        /// </summary>
        /// <param name="pigWorld"> the PigWorld in which this Thing should be created. </param>
        protected void AddToWorld(PigWorld pigWorld) {
            AddToWorld(pigWorld, Position.any);
        }

        /// <summary>
        /// Every subclass MUST call this method, or the above AddToWorld() method,
        /// as the last line of its constructor. This method will complete the
        /// creation of a new Thing by making it appear in the pigWorld. 
        /// The Thing will be placed at, or nearby, the position specified.
        /// </summary>
        /// <param name="pigWorld"> the PigWorld in which this Thing should be created. </param>
        /// <param name="position"></param>
        protected void AddToWorld(PigWorld pigWorld, Position position) {
            // It is necessary for this Thing to know its pigWorld before pigWorld.AddThing() is called.
            this.pigWorld = pigWorld;

            if (!pigWorld.AddThing(this, position)) {
                // If we failed to add ourselves to the pigWorld, set our pigWorld
                // reference back to null...
                this.pigWorld = null;
            }
        }

        /// <summary>
        /// Destroys this Thing so that it ceases to exist.
        /// Example 1: the Eat() method inside Animal calls Delete() on the food
        /// after transferring its energy to the eater.
        /// Example 2: to make a NonlivingThing disappear, you need to pick it up
        /// (with Cell.pickup), then destroy it (with Thing.Delete()).
        /// </summary>
        public virtual void Delete() {
            thingDestroyedEvent();
            pigWorld.RemoveThing(this);
            pigWorld = null;
        }

        /// <summary>
        /// This method returns true as long as this Thing exists in a pigWorld. If
        /// this thing is destroyed (e.g. eaten), then this method will return false.
        /// </summary>
        /// <returns> true if this Thing exists, or false otherwise. </returns>
        public bool Exists() {
            return pigWorld != null;
        }

        /// <summary>
        /// Creates a string representation of this Thing. 
        /// This is useful for debugging purposes and any other times 
        /// when you want to have a string showing what value(s) an object has.
        /// </summary>
        /// <returns> a string representation of this Thing </returns>
        public override string ToString() {
            return GetType().Name + " at " + Cell;
        }

    }
}
