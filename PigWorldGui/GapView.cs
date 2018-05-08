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

    [System.ComponentModel.DesignerCategory("")]  // Stop Visual Studio from trying to open this class in Design View.

    /// <summary>
    /// The View class for a Gap.  Gaps can be filled with walls 
    /// which are used to block the pathways from one Cell to the next.
    /// 
    /// Original (AWT) author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class GapView : Panel {

        private Gap gap;

        public bool gapHasWall { 
            get { return gap.HasWall; }
            set { gap.HasWall = value; }
        }

        /// <summary>
        /// Constructs the GapView.
        /// </summary>
        /// <param name="gap"> the Gap to view </param>
        public GapView(PigWorldView pigWorldView, Gap gap) {
            this.gap = gap;
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0);  // Fill the entire space available.

            gap.gapChangedEvent += GapChangedEvent;
        }

        /// <summary>
        /// Event-handler for when the Gap changes, e.g. when a wall is added into that Gap.
        /// </summary>
        private void GapChangedEvent() {
            if (gap.HasWall) {
                this.BackColor = Color.Black;
            }
            else {
                this.BackColor = Parent.BackColor;  // Reset the background colour.
            }
        }

        /// <summary>
        /// Handles mouse-click events, in this GapView.
        /// 
        /// Overrides the OnMouseClick method in the base class, Panel.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e) {
            gap.HasWall = !gap.HasWall;  // Toggle the value.
        }
   }
}
