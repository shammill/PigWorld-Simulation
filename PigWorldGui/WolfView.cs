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
    /// The View class for a Wolf.
    /// 
    /// Original (AWT) author: Raymond Lister 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class WolfView : AnimalView {

        private Wolf wolf;  // A reference to the wolf being viewed.

        private static Image wolfImage = Image.FromFile(@"Resources\wolf.gif");

        /// <summary>
        /// Constructs a WolfView.
        /// </summary>
        /// <param name="wolf"> the wolf to view. </param>
        public WolfView(PigWorldView pigWorldView, Wolf wolf) 
            : base(pigWorldView, wolf) {

            this.wolf = wolf;
            InitialiseContextMenu();
        }

        /// <summary>
        /// Creates the context-menu for the Wolf, and specifies the names of 
        /// the event-handlers in this class, for the items in that menu.
        /// </summary>
        private void InitialiseContextMenu() {
            contextMenuStrip.ShowImageMargin = false;
            contextMenuStrip.Items.Add("Show Id", null, showIdMenuItem_Click);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add("Delete", null, deleteMenuItem_Click);
        }

        /// <summary>
        /// Event-handler for the Show ID Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void showIdMenuItem_Click(object sender, EventArgs e) {
            int wolfId = wolf.Id;
            string wolfIdMessage = wolfId.ToString();
            MessageBox.Show("Id = " + wolfIdMessage);
        }

        /// <summary>
        /// Event-handler for the Delete Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void deleteMenuItem_Click(object sender, EventArgs e) {
            wolf.Delete();
        }

        /// <summary>
        /// Displays the Wolf's image on the screen.
        /// 
        /// Overrides the DisplayLifeFormImage method in the base class, AnimalView.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's image is displayed. </param>
        protected override void DisplayLifeFormImage(Graphics graphics) {
            graphics.DrawImage(wolfImage, animalRectangle);
        }
    }
}
