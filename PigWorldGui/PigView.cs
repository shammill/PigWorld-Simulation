using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// The View class for a Pig. The same class PigView is used for displaying both
    /// GirlPigs and BoyPigs. PigView has no subclasses.
    /// 
    /// Original (AWT) author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class PigView : AnimalView {

        private Pig pig;  // A reference to the pig being viewed.

        private static Image hungryImage = Image.FromFile(@"Resources\pig_hungry.gif");
        private static Image inLoveImage = Image.FromFile(@"Resources\pig_in_love.gif");

        /// <summary>
        /// Constructs a PigView for the specified Pig.
        /// </summary>
        /// <param name="pigWorldView"> the PigWorldView that this PigView is part of. </param>
        /// <param name="pig"> the pig to view. </param>
        public PigView(PigWorldView pigWorldView, Pig pig) 
            : base(pigWorldView, pig) {

            this.pig = pig;
            this.ageBrush = Brushes.White;
            InitialiseContextMenu();
        }

        /// <summary>
        /// Creates the context-menu for the Pig, and specifies the names of 
        /// the event-handlers in this class, for the items in that menu.
        /// </summary>
        private void InitialiseContextMenu() {
            contextMenuStrip.ShowImageMargin = false;
            contextMenuStrip.Items.Add("Sleep", null, sleepMenuItem_Click);
            contextMenuStrip.Items.Add("Wake Up", null, wakeUpMenuItem_Click);
            contextMenuStrip.Items.Add("Eat", null, eatMenuItem_Click);
            contextMenuStrip.Items.Add("Put in Mood for Love", null, moodForLoveMenuItem_Click);
            contextMenuStrip.Items.Add("Clear Rope", null, clearRopeMenuItem_Click);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add("Show Id", null, showIdMenuItem_Click);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add("Delete", null, deleteMenuItem_Click);
        }

        /// <summary>
        /// Event-handler for the Sleep Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void sleepMenuItem_Click(object sender, EventArgs e) {
            const int INCREASE_AMOUNT = 100;
            pig.IncreaseTiredness(INCREASE_AMOUNT);
        }

        /// <summary>
        /// Event-handler for the Wake Up Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void wakeUpMenuItem_Click(object sender, EventArgs e) {
            pig.WakeUp();
        }

        /// <summary>
        /// Event-handler for the Eat Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void eatMenuItem_Click(object sender, EventArgs e) {
            pig.IncreaseEnergy(PigFood.ENERGY_IN_PIG_FOOD);
        }

        /// <summary>
        /// Event-handler for the Put in the Mood for Love Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void moodForLoveMenuItem_Click(object sender, EventArgs e) {
            pig.PutInTheMoodForLove();
        }

        /// <summary>
        /// Event-handler for the Clear Rope Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void clearRopeMenuItem_Click(object sender, EventArgs e) {
            pig.ClearRope();
        }

        /// <summary>
        /// Event-handler for the Show ID Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void showIdMenuItem_Click(object sender, EventArgs e) {
            int pigId = pig.Id;
            string pigIdMessage = pigId.ToString();
            MessageBox.Show("Id = " + pigIdMessage);
        }

        /// <summary>
        /// Event-handler for the Delete Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void deleteMenuItem_Click(object sender, EventArgs e) {
            pig.Delete();
        }

        /// <summary>
        /// Displays the Pig's image on the screen.
        /// 
        /// Overrides the DisplayLifeFormImage method in the base class, AnimalView.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's image is displayed. </param>
        protected override void DisplayLifeFormImage(Graphics graphics) {

            if (pig.IsInTheMoodForLove())
                graphics.DrawImage(inLoveImage, animalRectangle);
            else
                graphics.DrawImage(hungryImage, animalRectangle);

            if (pig.IsTired()) {
                PointF snoringPoint = new PointF(animalRectangle.Width / 3, animalRectangle.Height / 3);
                graphics.DrawString("Zzz...", normalFont, Brushes.Blue, snoringPoint);
            }

            if (pig is GirlPig && ((GirlPig)pig).IsGrunting()) {
                PointF gruntingPoint = new PointF(animalRectangle.Width / 3, animalRectangle.Height / 3);
                graphics.DrawString("OINK!", normalFont, Brushes.Red, gruntingPoint);
            }
            
            if (PigWorldView.PigWorld.ShowDebugInfo) {
                PointF debugPigActionPoint = new PointF(1, 0);
                graphics.DrawString(pig.DebugAnimalAction, debugInfoFont, Brushes.Black, debugPigActionPoint);
            }
        }

        /// <summary>
        /// Displays the animal's gender-colour (blue or pink) on the screen.
        /// 
        /// Overrides the DisplayGenderColour method in the base class, AnimalView.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's gender-colour is displayed. </param>
        protected override void DisplayGenderColour(Graphics graphics) {

            using (SolidBrush pigColorBrush = new SolidBrush(pig.Color)) {
                // Display the boy/girl colour rectangle.
                graphics.FillRectangle(pigColorBrush, bottomRectangle);
            }
        }
    }
}
