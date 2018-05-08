using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// A LifeForm is a "living" entity and has behaviour. 
    /// There are two major (sub)types of LifeForms: 
    /// "Animals" can eat and move around, pick up NonlivingThings, 
    /// and interact with other animals. Examples are Pigs and Wolves.
    /// "Plants" are stationary but still have behaviour (e.g. a Tree produces pig food).
    /// 
    /// A new type of LifeForm can be defined by creating another subclass of this
    /// class (for example, a "Snake" subclass). That subclass should define a
    /// constructor which, as its very last step of initialisation, calls the
    /// AddToWorld() method inherited from this class. That will cause the new
    /// LifeForm to appear in the pigWorld.
    /// 
    /// As each unit of time passes in the pigWorld, the DoSomething() method of each
    /// LifeForm will be called. Each subclass of LifeForm must implement this
    /// method to define how it is to behave as time passes. For example, when a
    /// Pig is asked to do something, it might move one Cell in the direction of
    /// the nearest piece of food. A Pig should not move more than one Cell, or eat
    /// more than one piece of food in a single unit of time.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public abstract class LifeForm : Thing {

        private static int lastId = 0;  // The id of the last LifeForm created (when greater than 0).

        private int id;  // The unique id of this LifeForm.
        public int Id { get { return id; } }

        public delegate void LifeFormMovedEvent();
        // Initialise each with an empty delegate, to avoid null-check.
        public LifeFormMovedEvent lifeFormMovedEvent = delegate { };

        /// <summary>
        /// LifeForm constructor.
        /// </summary>
        protected LifeForm() {
            id = GetNextId();
        }

        /// <summary>
        /// Returns a new id to be used for a newly created LifeForm.
        /// </summary>
        /// <returns> the new id. </returns>
        private static int GetNextId() {
            // Increment lastId first, then return the value.
            return ++lastId;
        }

        /// <summary>
        /// Reset the lastId.  This method must only be called when resetting the entire PigWord,
        /// so that Ids are consistent, when multiple pigWorld setups are used.
        /// </summary>
        public static void ResetAllIds() {
            lastId = 0;
        }

        /// <summary>
        /// This method is called by the PigWorld to handle each unit of time. This
        /// method in turn calls DoSomething(). You should not use this method to
        /// define the behaviour of LifeForms. Behaviour should be defined in the
        /// DoSomething() method instead.
        /// </summary>
        public virtual void HandleTime() {
            DoSomething();
        }

        /// <summary>
        /// This method is called every unit of time requesting that we "do something".
        /// Each subclass needs to implement this method to define how this LifeForm
        /// behaves as each unit of time passes. At the very least,
        /// the implementation of this method can do nothing.
        /// </summary>
        protected abstract void DoSomething();

        /// <summary>
        /// Find the nearest object that belongs to the targetType, e.g. a Pig.
        /// The targetType may be a base class (e.g. a Pig class) 
        /// or a derived class (e.g. a BoyPig class).
        /// 
        /// This method uses a Radar, so it sees through walls.
        /// 
        /// Returns null when no objects of the targetType are found.
        /// </summary>
        /// <param name="targetType"> typeof(XXX) where XXX is the name of a class, or equivalent. </param>
        /// <returns> Either null or the Echo of the nearest object that belongs to the targetType. </returns>
        protected Echo FindNearest(Type targetType) {
            Radar radar = new Radar(this, targetType);
            Echo bestEchoSoFar = null;
            Echo echo = radar.Ping();   // priming ping

            while (echo != null) {

                if ( (bestEchoSoFar == null) || (echo.distance < bestEchoSoFar.distance) )
                    bestEchoSoFar = echo;

                echo = radar.Ping();
            }

            return bestEchoSoFar; 
        } // method FindNearest
    }
}
