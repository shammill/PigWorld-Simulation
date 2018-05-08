using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;       // For Color
using System.Diagnostics;   // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// BoyPigs override the LookForPig() method to look specifically for GirlPigs.
    /// When one is found, and the conditions are right, they make a baby.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class BoyPig : Pig {

        private static readonly Color BOY_COLOR = Color.Blue;

        /// <summary>
        /// Constructs a new BoyPig without parents, 
        /// e.g. when a pig is added to the pigWorld by the user.
        /// See the following constructor for further details.
        /// </summary>
        /// <param name="pigWorld"> the pigWorld that this pig is entering </param>
        /// <param name="position"> the preferred position for the new pig </param>
        public BoyPig(PigWorld pigWorld, Position position)
            : this(pigWorld, position, null, null) {  // In this context, "this" is a call to the constructor below.
        }

        /// <summary>
        /// Constructs a new BoyPig at, or near, the specified position. 
        /// If the desired position is already occupied by another LifeForm, 
        /// then the pig will be placed in a nearby Cell. 
        /// If no nearby Cells are free, then the pig will not be added to the pigWorld.
        /// </summary>
        /// <param name="pigWorld"> the pigWorld that this pig is entering </param>
        /// <param name="position"> the preferred position for the new pig </param>
        /// <param name="color"> the pig's colour (blue or pink) </param>
        /// <param name="mother"> the pig's mother (may be null)</param>
        /// <param name="father"> the pig's father (may be null)</param>
        public BoyPig(PigWorld pigWorld, Position position, GirlPig mother, BoyPig father)
            : base(pigWorld, position, BOY_COLOR, mother, father) {
        }

        /// <summary>
        /// When conditions are right, the BoyPig listens for a grunting GirlPig.
        /// When one is heard, the BoyPig tries to reach that GirlPig, 
        /// which will only succeed when she is in an adjacent cell.
        /// 
        /// If the BoyPig reaches a GirlPig, then they try to produce a baby pig.
        /// 
        /// But if no GirlPig can be reached, then the BoyPig tries to move in the direction of the sound.
        /// (If there is no sound, then the BoyPig can move in any direction.)
        /// 
        /// Overrides the LookForPig method in the base class, Pig.
        /// </summary>
        protected override void LookForPig() {
            // Do not modify, delete, or move the debugAnimalAction line below, 
            // or the Debug Info will not show correctly in the GUI, and that could be confusing.
            debugAnimalAction = "LookForPig";

            Direction direction = Listen();  // When no GirlPigs are grunting, this will be null.

            if (direction == null) {
                WanderAround();
            } else {
                LifeForm lifeForm = Reach(direction);  // Will be null when that direction's cell has no LifeForm occupant.

                if (lifeForm is GirlPig) {
                    GirlPig girl = (GirlPig)lifeForm;
                    if (!IsTired() && IsInTheMoodForLove()) {
                        girl.TryToMakeBaby(this);
                        UseEnergy(STOMACH_EMPTY_LEVEL);
                        IncreaseTiredness(5);
                    }

                } else {
                    Move(direction);
                }
            }
        }

    }
}
