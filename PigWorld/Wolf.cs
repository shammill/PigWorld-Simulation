using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary> 
    /// A Wolf is a type of Animal that eats Pigs, and does little else. 
    /// 
    /// Wolves simply move toward the nearest pig, and eat that pig if they catch it. 
    /// Wolves don’t sleep.  Wolves are never in the mood for love. They just keep coming after pigs. 
    /// They are the Arnold Schwarzenegger “I’ll be back” Terminators of the PigWorld.
    /// 
    /// Original author: Raymond Lister 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class Wolf : Animal {

        /// <summary>
        /// Constructs a new Wolf.
        /// </summary>
        /// <param name="pigWorld"> the PigWorld in which to create the Wolf. </param>
        public Wolf(PigWorld pigWorld) 
            : this(pigWorld, Position.any) {  // In this context, "this" is a call to the constructor below.
        }

        /// <summary>
        /// Constructs a new Wolf at the specified position. If the desired position is
        /// already occupied by another LifeForm, then the Wolf will be placed in a
        /// nearby Cell. If there are no nearby Cells that are free, then the Wolf
        /// will not be added to the pigWorld.
        /// </summary>
        /// <param name="pigWorld"></param>
        /// <param name="position"></param>
        public Wolf(PigWorld pigWorld, Position position) {
            AddToWorld(pigWorld, position);
        }

        /// <summary>
        /// This method is called every unit of time by the PigWorld to give the Wolf
        /// a chance to do something.
        /// 
        /// First the Wolf looks around for the nearest pig, using the
        /// method "FindNearest". If there isn't a pig, the Wolf does nothing.
        /// If there is, then the Wolf either:
        /// (1) tries to moves one cell/square in the direction of the nearest pig, or
        /// (2) if a pig is in fact only one move away, the wolf eats the pig.
        /// 
        /// Note that if there is a wall between the Wolf and the nearest pig,
        /// then the Wolf won't move at all.
        /// </summary>
        protected override void DoSomething() {
            // This will detect any type of pig (both BoyPigs and GirlPigs).

            Echo nearestPig = FindNearest(typeof(Pig));
            if (nearestPig != null) {
                Cell targetCell = Cell.GetAdjacentCell(nearestPig.direction);
                if (targetCell != null) {
                    LifeForm adjacentLifeForm = targetCell.LifeFormOccupant;
                    if (adjacentLifeForm == null) {
                        Move(targetCell);
                    } else if (adjacentLifeForm is Pig) {
                        Eat(adjacentLifeForm);
                        Move(targetCell);
                    }
                }
            }
        }
    }
}
