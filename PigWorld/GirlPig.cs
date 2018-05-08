using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// GirlPigs override the LookForPig() method to look specifically for BoyPigs.
    /// In addition, they provide a unique method called TryToMakeBaby() which is
    /// invoked by a BoyPig.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class GirlPig : Pig {

        private static readonly Color GIRL_COLOR = Color.DeepPink;  // Or Color.HotPink or Color.Pink

        private const int GRUNT_AGE_MODULUS = 6;
        private const int GRUNT_TIMEOUT = 3;  // The amount of time a GirlPig grunts for.

        // When a GirlPig is not grunting, gruntTimeLeft is set to zero. However,
        // when a GirlPig grunts, this value is set to GRUNT_TIMEOUT, and then
        // decremented as each unit of time passes. When this value returns to
        // zero, the GirlPig stops grunting.
        private int gruntTimeLeft;

        /// <summary>
        /// Constructs a new GirlPig without parents, 
        /// e.g. when a pig is added to the pigWorld by the user.
        /// See the following constructor for further details.
        /// </summary>
        /// <param name="pigWorld"> the pigWorld that this pig is entering </param>
        /// <param name="position"> the preferred position for the new pig </param>
        public GirlPig(PigWorld pigWorld, Position position)
            : this(pigWorld, position, null, null) {  // In this context, "this" is a call to the constructor below.
        }

        /// <summary>
        /// Constructs a new GirlPig at, or near, the specified position. 
        /// If the desired position is already occupied by another LifeForm, 
        /// then the pig will be placed in a nearby Cell. 
        /// If no nearby Cells are free, then the pig will not be added to the pigWorld.
        /// </summary>
        /// <param name="pigWorld"> the pigWorld that this pig is entering </param>
        /// <param name="position"> the preferred position for the new pig </param>
        /// <param name="color"> the pig's colour (blue or pink) </param>
        /// <param name="mother"> the pig's mother (may be null)</param>
        /// <param name="father"> the pig's father (may be null)</param>
        GirlPig(PigWorld pigWorld, Position position, GirlPig mother, BoyPig father)
            : base(pigWorld, position, GIRL_COLOR, mother, father) {
        }

        /// <summary>
        /// Override the base class's DoSomething so that the gruntTimeLeft is reduced,
        /// at each step of the pigWorld.
        /// </summary>
        protected override void DoSomething() {
            base.DoSomething();  // Do what pigs always do -- see this method in the base class, Pig.

            if (gruntTimeLeft > 0)
                gruntTimeLeft -= 1;
        }

        /// <summary>
        /// Returns true if this pig is grunting.
        /// </summary>
        /// <returns> true if this pig is grunting. </returns>
        public bool IsGrunting() {
            return gruntTimeLeft > 0;
        }

        /// <summary>
        /// When conditions are right, the GirlPig starts grunting to attract a BoyPig.
        /// 
        /// Overrides the LookForPig method in the base class, Pig.
        /// </summary>
        protected override void LookForPig() {
            // Do not modify, delete, or move the debugAnimalAction line below, 
            // or the Debug Info will not show correctly in the GUI, and that could be confusing.
            debugAnimalAction = "LookForPig";

            if (Age % GRUNT_AGE_MODULUS == 0) {
                // Start grunting.
                gruntTimeLeft = GRUNT_TIMEOUT;
                Cell.Air.TransmitSound(PigWorld.OINK_SOUND_LEVEL);
                Grunt();

            }
        }

        /// <summary>
        /// This method creates a new baby pig, when conditions are right.
        /// There is an even chance of producing either a GirlPig or a BoyPig.
        /// 
        /// They will not produce a baby if one or the other is (1) too tired, 
        /// (2) not in the mood for love, (3) they are brother-and-sister, or 
        /// (4) they are parent-and-child.
        /// </summary>
        public bool TryToMakeBaby(BoyPig boyFriend) {
            
            if (IsTired() || !IsInTheMoodForLove() || IsSibling(boyFriend) || IsParent(boyFriend) || boyFriend.IsParent(this))
                return false;

            Position position = Cell.Position;
            if (Util.random.NextDouble() >= 0.5)
                new BoyPig(this.PigWorld, position, this, boyFriend);
            else
                new GirlPig(this.PigWorld, position, this, boyFriend);

            UseEnergy(STOMACH_EMPTY_LEVEL);
            IncreaseTiredness(5);
            Shriek();
            return true;
        }

    }
}
