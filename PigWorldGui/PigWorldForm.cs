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
    /// This class is the overall form for PigWorld, containing the user controls, etc.
    /// 
    /// Author: Jim Reye
    /// </summary>
    public partial class PigWorldForm : Form {

        private PigWorldView pigWorldView = new PigWorldView();  // The grid of squares/cells.

        /// <summary>
        /// Construct the PigWorldForm.
        /// </summary>
        public PigWorldForm() {
            InitializeComponent();

            this.Controls.Add(pigWorldView);

            // Ensure that the PigWorldView uses only the available space, not the entire form.
            // This must be done AFTER the PigWorldView is added to the form (by the above Add).
            pigWorldView.BringToFront();

            CancelButton = quitButton;  // Allow the Esc key to close the form.

        }

        /// <summary>
        /// Event-handler for the Setup Demo Button.
        /// </summary>
        /// <param name="sender"> the Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void setupDemoButton_Click(object sender, EventArgs e) {
            pigWorldView.PigWorld.RemoveAll();

            switch ((int)demoNumber.Value) {
                case 1: pigWorldView.SetupDemo1(); break;
                case 2: pigWorldView.SetupDemo2(); break;
                case 3: pigWorldView.SetupDemo3(); break;
                default:
                    // This can only happen if the Minimum and Maximum properties of the NumericUpDown control 
                    // have NOT been set correctly, in Design View.
                    MessageBox.Show("Invalid demo number."); 
                    break;  
            }
        }

        /// <summary>
        /// Event-handler for the Quit Button.
        /// </summary>
        /// <param name="sender"> the Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void quitButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        /// <summary>
        /// Event-handler for the Show Debug Info CheckBox.
        /// </summary>
        /// <param name="sender"> the CheckBox where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void showDebugInfoCheckBox_CheckedChanged(object sender, EventArgs e) {
            pigWorldView.PigWorld.ShowDebugInfo = showDebugInfoCheckBox.Checked;
        }

        /// <summary>
        /// Event-handler for the Step Button.
        /// </summary>
        /// <param name="sender"> the Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void stepButton_Click(object sender, EventArgs e) {
            pigWorldView.PigWorld.Step();
        }

        /// <summary>
        /// Event-handler for the Start Button.
        /// </summary>
        /// <param name="sender"> the Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void startButton_Click(object sender, EventArgs e) {
            Timer.Start();
        }

        /// <summary>
        /// Event-handler for the Stop Button.
        /// </summary>
        /// <param name="sender"> the Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void stopButton_Click(object sender, EventArgs e) {
            Timer.Stop();
        }

        /// <summary>
        /// Event-handler for the Timer Tick.
        /// </summary>
        /// <param name="sender"> the Timer where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void Timer_Tick(object sender, EventArgs e) {
            pigWorldView.PigWorld.Step();
        }

        /// <summary>
        /// Event-handler for scrolling the speed trackbar.
        /// </summary>
        /// <param name="sender"> the trackbar where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void speedTrackBar_Scroll(object sender, EventArgs e) {
            int speed = speedTrackBar.Value;
            int interval = (10 - speed) * 100;

            if (interval < 100) {
                interval = 100;
            }
            Timer.Interval = interval;
        }

        /// <summary>
        /// Event-handler for the Boy Pig Radio Button.
        /// </summary>
        /// <param name="sender"> the Radio Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void boyPigRadioButton_CheckedChanged(object sender, EventArgs e) {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked) {
                pigWorldView.CurrentTypeOfObjectToAdd = typeof(BoyPig);
            }
        }

        /// <summary>
        /// Event-handler for the Girl Pig Radio Button.
        /// </summary>
        /// <param name="sender"> the Radio Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void girlPigRadioButton_CheckedChanged(object sender, EventArgs e) {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked) {
                pigWorldView.CurrentTypeOfObjectToAdd = typeof(GirlPig);
            }
        }

        /// <summary>
        /// Event-handler for the Pig Food Radio Button.
        /// </summary>
        /// <param name="sender"> the Radio Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void pigFoodRadioButton_CheckedChanged(object sender, EventArgs e) {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked) {
                pigWorldView.CurrentTypeOfObjectToAdd = typeof(PigFood);
            }
        }

        /// <summary>
        /// Event-handler for the Tree Radio Button.
        /// </summary>
        /// <param name="sender"> the Radio Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void treeRadioButton_CheckedChanged(object sender, EventArgs e) {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked) {
                pigWorldView.CurrentTypeOfObjectToAdd = typeof(Tree);
            }
        }

        /// <summary>
        /// Event-handler for the Wolf Radio Button.
        /// </summary>
        /// <param name="sender"> the Radio Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void wolfRadioButton_CheckedChanged(object sender, EventArgs e) {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked) {
                pigWorldView.CurrentTypeOfObjectToAdd = typeof(Wolf);
            }
        }

        /// <summary>
        /// Event-handler for the Remove All Button.
        /// </summary>
        /// <param name="sender"> the Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void removeAllButton_Click(object sender, EventArgs e) {
            pigWorldView.PigWorld.RemoveAll();
        }

        /// <summary>
        /// Event-handler for the Remove Walls Button.
        /// </summary>
        /// <param name="sender"> the Button where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void removeWallsButton_Click(object sender, EventArgs e) {
            pigWorldView.PigWorld.RemoveAllWalls();
        }

        /// <summary>
        /// Event-handler for the Enable Audio CheckBox.
        /// </summary>
        /// <param name="sender"> the CheckBox where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void enableRealAudioCheckBox_CheckedChanged(object sender, EventArgs e) {
            pigWorldView.PigWorld.EnableRealAudio = enableRealAudioCheckBox.Checked;
        }

    }

}
