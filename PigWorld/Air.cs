using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// Air is used to transmit (virtual) sound from Cell to Cell. 
    /// Air is divided up between the Cells so that every Cell has its own Air object. 
    /// The constructor for Air receives a reference to the Cell that creates this instance of Air. 
    /// The Air object stores this reference, so that it can refer to the Cell later.
    /// 
    /// There are two situations when an Air object may need to refer to its Cell.
    /// 1) In order to transmit sound, this air needs to use its TransmitSound() 
    /// method to the sound level in surrounding Cells, and thus needs to use 
    /// its Cell to get references to the surrounding Cells.
    /// 2) When the soundLevel measured in this Cell increases or decreases, the
    /// Cell is notified via its AirChanged() method, allowing the Cell to
    /// update its visual display.
    /// 
    /// When sound is transmitted to the air of surrounding Cells, the intensity of
    /// the sound weakens. The soundLevel in each cell also persists for a while
    /// and fades away gradually over time. This gives animals a chance to follow
    /// the path of sound to the source before the sound disappears.
    /// 
    /// An Animal may initiate a sound by using its current Cell to obtain access
    /// to the Air in that Cell, and then calling the TransmitSound method.
    ///     E.g. Cell.Air.TransmitSound(PigWorld.OINK_SOUND_LEVEL);
    /// where that method's parameter value is the initial intensity of the sound. 
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class Air {

        private Cell cell;  // A reference to the Cell where this air is situated.
        public Cell Cell { get { return cell; } }

        private int soundLevel = 0;  // The amount of sound in this air. Default is zero.
        public int SoundLevel { get { return soundLevel; } }

        /// <summary>
        /// The constructor for initialising a new Air object. 
        /// This constructor is only called by code in the Cell class. The Cell
        /// that creates this Air passes a reference to itself as the parameter.
        /// </summary>
        /// <param name="cell"> the Cell that is creating this Air. </param>
        public Air(Cell cell) {
            this.cell = cell;
        }

        /// <summary>
        /// The PigWorld calls this method periodically on every Air object,
        /// so that all sound fades gradually over time.
        /// </summary>
        public void DecrementSoundLevel() {

            if (soundLevel > 0) {
                soundLevel -= 1;
                cell.AirChanged();
            }
        }

        /// <summary>
        /// This method is available to all Things that wish to make a sound in the
        /// pigWorld. Calling this method starts a chain reaction in which this air
        /// object sends a TransmitSound message to all surrounding air objects,
        /// and each of those in turn send a TransmitSound message to their own
        /// surrounding air objects, until the sound dies out or reaches a dead-end.
        /// 
        /// Sound does not pass THROUGH walls, but it will propagate AROUND a wall
        /// whenever there is a path from one Cell to another.
        /// 
        /// Each time sound is transmitted to surrounding air objects, the
        /// passed-on soundLevel is decreased by one.
        /// </summary>
        /// <param name="soundLevel"> the intensity of sound to transmit to this particular
        /// piece of air. </param>
        public void TransmitSound(int soundLevel) {

            if (soundLevel <= this.soundLevel)
                return;

            this.soundLevel = soundLevel;

            cell.AirChanged();

            for (int i = 0; i < Direction.NUMBER_POSSIBLE; i++) {
                Direction direction = Direction.GetAdjacentCellDirection(i);
                Cell adjacentCell = Cell.GetAdjacentCell(direction);
                if (adjacentCell != null) {
                    Air adjacentAir = adjacentCell.Air;
                    adjacentAir.TransmitSound(soundLevel - 1);
                }
            }
        }
    }
}
