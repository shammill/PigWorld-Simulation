using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// PigFood is a type of NonlivingThing that provides nourishment to Pigs. Each piece of
    /// PigFood has an energy amount that gets transferred to the Pig that eats it.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class PigFood : NonlivingThing {

        public const int ENERGY_IN_PIG_FOOD = 45;

        /// <summary>
        /// Constructs a new piece of PigFood. The PigFood will be placed at a random
        /// location.
        /// </summary>
        /// <param name="pigWorld"> the PigWorld in which to create the PigFood. </param>
        public PigFood(PigWorld pigWorld) {
            Init();
            AddToWorld(pigWorld);
        }

        /// <summary>
        /// Constructs a new piece of PigFood. The PigFood will be placed at, or
        /// nearby, the specified position.
        /// </summary>
        public PigFood(PigWorld pigWorld, Position position) {
            Init();
            AddToWorld(pigWorld, position);
        }

        /// <summary>
        /// This is a common routine used by all PigFood constructors.
        /// </summary>
        private void Init() {
            this.Energy = ENERGY_IN_PIG_FOOD;
        }
    }
}
