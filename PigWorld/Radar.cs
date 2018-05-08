using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// Radars may be used by any type of LifeForm, including Plants such as Trees.
    /// Radars are used to find out what objects of a certain type are in the 
    /// environment around the LifeForm.  The "certain type" is specified as a 
    /// parameter to the Radar constructor.
    /// 
    /// Radars work in a way analogous to radars in the real pigWorld. A 360 degree
    /// sweep begins with the radar looking directly North. The sweep proceeds
    /// clockwise until it returns to North. As it sweeps, it returns an instance
    /// of an Echo each time the radar sweeps over an object of the desired class.
    /// Successive messages to the "Ping" method return successive Echoes.
    /// Each time the sweep returns to North, a null reference is returned. 
    /// 
    /// Radars see through walls. I.e. a radar is not affected by the presence
    /// of any walls in its path. But a radar does not necessarily detect every object 
    /// of the "certain type". If an object of the target type is directly behind 
    /// another object of that type (relative to the owner of the radar), 
    /// then the radar will not detect it.
    /// 
    /// Original authors: Ryan Heise and Raymond Lister 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class Radar {

        private LifeForm owner;  // the LifeForm that owns this radar

        private PigWorld pigWorld;  // the pigWorld inhabited by the owner 

        private Type targetType;    // the sort of Thing the radar detects

        private double sweepAngle;  // the current angle of the radar's sweep

        /// <summary>
        /// Constructs a new Radar.
        /// </summary>
        /// <param name="owner">A reference to the LifeForm that owns this radar.  </param> 
        /// <param name="targetType"> The type of object detected by this radar. </param>
        public Radar(LifeForm owner, Type targetType) {  
            this.owner = owner;
            this.pigWorld = owner.PigWorld;
            this.targetType = targetType;

            sweepAngle = -1.0;  // negative value ensures 0 degrees will be swept first.
        }

        /// <summary>
        /// The radar commences or continues its clockwise sweep until either:
        /// (a) a target object is found of the right type; or 
        /// (b) the sweep returns to North and null is returned. 
        /// </summary>
        /// <returns> Either null or the Echo of a new object that belongs to the targetType. </returns>
        public Echo Ping() {

            Thing bestSoFarThing = null;
            Direction bestSoFarDirection = null;
            double bestSoFarAngle = 0.0;
            double bestSoFarDistance = 0.0;

            foreach (Thing thing in pigWorld.Things) {

                // the radar should not detect its owner
                if (thing == owner) {
                    continue;
                }

                // the radar only detects instances of the right class, and its subclasses (if any).
                if (!Util.IsSameTypeOrSubtype(thing.GetType(), targetType)) {
                    continue;
                }

                Direction tempDirection = pigWorld.GetDirection(owner, thing);
                double tempAngle = tempDirection.Degrees;
                if (tempAngle <= sweepAngle) {
                    continue;
                }

                if (bestSoFarThing == null) {
                    bestSoFarThing = thing;
                    bestSoFarDirection = tempDirection;
                    bestSoFarAngle = tempAngle;
                    bestSoFarDistance = pigWorld.GetDistance(owner, bestSoFarThing);
                } else {
                    if (tempAngle < bestSoFarAngle) {
                        bestSoFarThing = thing;
                        bestSoFarDirection = tempDirection;
                        bestSoFarAngle = tempAngle;
                        bestSoFarDistance = pigWorld.GetDistance(owner, bestSoFarThing);
                    } else
                        if (tempAngle == bestSoFarAngle) {
                            double tempDistance = pigWorld.GetDistance(owner, thing);
                            if (tempDistance < bestSoFarDistance) {
                                bestSoFarThing = thing;
                                // Don't have to change bestSoFarDirection or bestSoFarAngle.
                                bestSoFarDistance = tempDistance;
                            }
                        } // if (tempAngle == bestSoFarAngle)
                } // if else (bestSoFarThing == null)
            } // end foreach

            if (bestSoFarThing == null) {
                sweepAngle = -1.0; // ready for a new sweep
                return null;
            } else {
                sweepAngle = bestSoFarAngle;
                Type typeOfFirstThing = bestSoFarThing.GetType();  
                Echo echo = new Echo(typeOfFirstThing, bestSoFarDirection, bestSoFarDistance);
                return echo;
            }
        } // method Ping

    } // class Radar
}
