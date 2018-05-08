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
    /// The View class for a Tree.
    /// 
    /// Original (AWT) author: Lizveth Robles 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class TreeView : PlantView {

        private Tree tree;  // A reference to the tree we are viewing.

        private static Image treeImage = Image.FromFile(@"Resources\tree.gif");

        /// <summary>
        /// Constructs a TreeView for the specified Tree.
        /// </summary>
        /// <param name="pigWorldView"></param>
        /// <param name="tree"> the tree to view </param>
        public TreeView(PigWorldView pigWorldView, Tree tree) 
            : base(pigWorldView, tree) {

            this.tree = tree;
            InitialiseContextMenu();
        }

        /// <summary>
        /// Creates the context-menu for the Tree, and specifies the names of 
        /// the event-handlers in this class, for the items in that menu.
        /// </summary>
        private void InitialiseContextMenu() {
            contextMenuStrip.ShowImageMargin = false;
            contextMenuStrip.Items.Add("Drop Food", null, dropFoodMenuItem_Click);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add("Show Id", null, showIdMenuItem_Click);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add("Delete", null, deleteMenuItem_Click);
        }

        /// <summary>
        /// Event-handler for the Drop Food Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void dropFoodMenuItem_Click(object sender, EventArgs e) {
            tree.DropFood();
        }

        /// <summary>
        /// Event-handler for the Show ID Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void showIdMenuItem_Click(object sender, EventArgs e) {
            int treeId = tree.Id;
            string treeIdMessage = treeId.ToString();
            MessageBox.Show("Id = " + treeIdMessage);
        }

        /// <summary>
        /// Event-handler for the Delete Context Menu Item.
        /// </summary>
        /// <param name="sender"> the menu where this event occurred </param>
        /// <param name="e"> extra information (if any) about the event </param>
        private void deleteMenuItem_Click(object sender, EventArgs e) {
            tree.Delete();
        }

        /// <summary>
        /// Displays the Tree's image on the screen.
        /// 
        /// Overrides the Paint method in the base class, ThingView.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's image is displayed. </param>
        protected override void Paint(Graphics graphics) {
            graphics.DrawImage(treeImage, thingViewRectangle);
        }
    }
}
