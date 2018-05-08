using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// An Animal is a (sub)type of LifeForm that eats and moves. There may
    /// be many types of Animals. For example, Pig is a type of Animal which is
    /// programmed to eat PigFood and give birth to new Pigs.
    /// 
    /// An Animal may move to a neighbouring Cell in any Direction using a
    /// Move() method. However, if there is a Wall or another LifeForm in the way,
    /// then a call to this method will have no effect.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public abstract class Animal : LifeForm {

        private const int ENERGY_USED_PER_STEP = 1;

        // When an animal's energy level reaches this amount, 
        // isHungry is set to false.
        public const int STOMACH_FULL_LEVEL = 60;

        // When an animal's energy level drops to this amount, 
        // isHungry is set to true.
        protected const int STOMACH_EMPTY_LEVEL = 20;

        // Indicates whether or not this Animal is hungry. An animal becomes
        // hungry when its energy level drops below the STOMACH_EMPTY_LEVEL, and
        // remains hungry until its energy level reaches the STOMACH_FULL_LEVEL.
        protected bool isHungry = true;
        public bool IsHungry { get { return isHungry; } }

        // Specifies how many more "time slices" this animal will rest (after producing offspring).
        protected int tiredness = 0;

        // This debugAnimalAction is used only to help display information (via the GUI), to the 
        // program developer, about the actions of each animal as each step occurs.
        // As such, this debugAnimalAction should only ever be used in assignment statements
        // that update it to reflect the latest action.
        // This data item is only provided to assist with debugging. So it must NEVER be used to control 
        // the flow of the program, i.e. not used in conditional statements such as "if" and "switch".
        // In other words, if the following two lines were deleted from this program 
        // (along with all statements that refer to these two items), then the program must still
        // run correctly.
        protected string debugAnimalAction = "";
        public string DebugAnimalAction { get { return debugAnimalAction; } }

        /// <summary>
        /// This method replaces LifeForm's HandleTime() to also
        /// use one unit of this animal's energy.
        /// </summary>
        public override void HandleTime() {
            DoSomething();
            UseEnergy();
        }

        /// <summary>
        /// Increases the amount of time this animal needs to rest (after producing
        /// offspring).
        /// 
        /// Why go to all the trouble of having this method?  Why not just
        /// manipulate "tiredness" directly? (i.e. why not break the great golden
        /// rule?)  Answer: so that future modifications to the software are
        /// easier.  Perhaps in the future the value in "tiredness" will be:
        /// (a) manipulated at many places in the program, and 
        /// (b) displayed on the GUI somewhere.  This "set" method means that 
        /// the GUI code for updating the display of "rest" will only need 
        /// to be placed here, in this method, rather than in every place 
        /// where "rest" is set.
        /// </summary>
        /// <param name="amount"></param>
        public void IncreaseTiredness(int amount) {
            if (amount < 0)
                throw new Exception("IncreaseTiredness: amount(" + amount + ") < 0");
            tiredness += amount;

            thingChangedEvent();
        }

        /// <summary>
        /// Reduces the tiredness of this animal by one unit. When the animal's
        /// tiredness finally reaches zero, the animal is no longer tired.
        /// </summary>
        protected void Rest() {
            // Do not modify, delete, or move the debugAnimalAction line below, 
            // or the Debug Info will not show correctly in the GUI, and that could be confusing.
            debugAnimalAction = "Rest";
            
            if (tiredness > 0) {
                tiredness -= 1;
                thingChangedEvent();
            }
        }

        /// <summary>
        /// This method returns true if the animal's tiredness level is above zero.
        /// An animal will stay tired until it has had enough rest.
        /// </summary>
        /// <returns> true if this animal is tired, or false otherwise. </returns>
        public bool IsTired() {
            return tiredness > 0;
        }

        /// <summary>
        /// Resets this animal's tiredness to zero.
        /// </summary>
        public void WakeUp() {
            tiredness = 0;
            thingChangedEvent();
        }

        /// <summary>
        /// The energy of this animal is increased by the amount specified, or
        /// to the STOMACH_FULL_LEVEL, whichever is the smaller. If the energy
        /// level reaches STOMACH_FULL_LEVEL, the animal is set to be not hungry.
        /// </summary>
        /// <param name="amount"></param>
        public void IncreaseEnergy(int amount) {
            Energy += amount;

            if (Energy >= STOMACH_FULL_LEVEL) {
                Energy = STOMACH_FULL_LEVEL;
                isHungry = false;
            }

            thingChangedEvent();
        }

        /// <summary>
        /// The energy in the food is added to this animal's own energy, and the
        /// food is removed from the pigWorld.
        /// </summary>
        /// <param name="food"></param>
        public void Eat(Thing food) {
            // Do not modify, delete, or move the debugAnimalAction line below, 
            // or the Debug Info will not show correctly in the GUI, and that could be confusing.
            debugAnimalAction = "Eat";

            IncreaseEnergy(food.Energy);
            food.Delete();
        }

        /// <summary>
        /// The animal reaches in the specified direction and a reference to whatever
        /// LifeForm (if any) that is on the next cell in that direction is returned.
        /// This enables LifeForms to interact with each other.
        /// </summary>
        /// <param name="direction"> the direction in which to reach. </param>
        /// <returns> whatever LifeForm is on the next Cell. </returns>
        public LifeForm Reach(Direction direction) {
            Cell targetCell = Cell.GetAdjacentCell(direction);
            if (targetCell == null)
                return null;
            else
                return targetCell.LifeFormOccupant;  // Will be null when that cell has no LifeForm occupant.
        }

        /// <summary>
        /// Tests if this animal can move in the indicated direction. This method
        /// may return false if there is some Thing on the next cell obstructing
        /// the path, or if there is a Wall in the way, or if the end of the pigWorld
        /// is in that direction.
        /// </summary>
        /// <param name="direction"> the direction we want to know if we can move. </param>
        /// <returns> true if we can move there, or false otherwise. </returns>
        public bool CanMove(Direction direction) {
            Cell adjacentCell = Cell.GetAdjacentCell(direction);
            if (adjacentCell != null)
                return adjacentCell.IsRoomFor(this);
            else
                return false;
        }

        /// <summary>
        /// Causes this Animal to try to move one step in the direction of the specified targetCell.
        /// </summary>
        /// <param name="targetCell"> the cell to move towards. </param>
        /// <returns> true if the Animal successfully moved, or false otherwise. </returns>
        public bool Move(Cell targetCell) {
            return Move(PigWorld.GetDirection(this.Cell, targetCell));
        }

        /// <summary>
        /// Causes this Animal to try to move one step in the given direction.
        /// </summary>
        /// <param name="direction"> the direction in which to move. </param>
        /// <returns> true if the Animal successfully moved, or false otherwise. </returns>
        protected virtual bool Move(Direction direction) {
            bool success = false;

            Cell targetCell = Cell.GetAdjacentCell(direction);
            if (targetCell != null) {
                success = targetCell.AddThing(this);

                if (success) {
                    lifeFormMovedEvent();
                }
            }

            return success;
        }

        /// <summary>
        /// Causes this Animal to try to move one step in the given direction.
        /// </summary>
        /// <param name="directionInDegrees"> the direction to move, in degrees. </param>
        /// <returns> true if the Animal successfully moved, or false otherwise. </returns>
        public bool Move(double directionInDegrees) {
            return Move(new Direction(directionInDegrees));
        }

        /// <summary>
        /// Cause the energy level of this Animal to decrease.
        /// </summary>
        protected void UseEnergy() {
            UseEnergy(ENERGY_USED_PER_STEP);
        }

        /// <summary>
        /// Cause the energy level of this Animal to decrease by the given amount.
        /// </summary>
        /// <param name="amount"> the amount of energy to use. </param>
        protected void UseEnergy(int amount) {
            Energy = Math.Max(Energy - amount, 0);
            if (Energy < STOMACH_EMPTY_LEVEL) {
                isHungry = true;
            }
            thingChangedEvent();
        }

        /// <summary>
        /// Indicates whether or not this Animal is in the mood for love,
        /// which it always is, when it's not hungry. 
        /// </summary>
        /// <returns> true if the Animal is in the mood, or false otherwise. </returns>
        public bool IsInTheMoodForLove() {
            return !isHungry;
        }

        /// <summary>
        /// Animals may listen, to detect which direction the loudest sound is
        /// coming from. This method works by examining the SoundLevels in the air
        /// in all immediately-surrounding cells and picking the one that has the greatest
        /// sound level.
        /// 
        /// If a number of directions have equal SoundLevels, any one of them may be returned.
        /// 
        /// But in the special case when all the SoundLevels are zero, null is returned.
        /// </summary>
        /// <returns> the Direction of the loudest sound, or null if all the SoundLevels are zero. </returns>
        public Direction Listen() {
            int loudestSound = 0;
            Direction loudestSoundDirection = Direction.NORTH;

            for (int i = 0; i < Direction.NUMBER_POSSIBLE; i++) {
                Direction direction = Direction.GetAdjacentCellDirection(i);
                Cell adjacentCell = Cell.GetAdjacentCell(direction);

                if (adjacentCell != null) {
                    Air adjacentAir = adjacentCell.Air;
                    int adjacentSoundLevel = adjacentAir.SoundLevel;
                    if (loudestSound < adjacentSoundLevel) {
                        loudestSound = adjacentSoundLevel;
                        loudestSoundDirection = direction;
                    }
                }
            }
            if (loudestSound == 0) { //0 being default value
                return null;
            } else {
            return loudestSoundDirection;
            }
        }

        /// <summary>
        /// Put this Animal in the mood for love, by setting its energy level to
        /// STOMACH_FULL_LEVEL.
        /// </summary>
        public void PutInTheMoodForLove() {
            IncreaseEnergy(STOMACH_FULL_LEVEL);
        }

    }
}
