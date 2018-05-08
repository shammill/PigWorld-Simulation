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
    /// The View class for a PigFood object.
    /// 
    /// Original (AWT) author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class PigFoodView : NonlivingThingView {

        private PigFood pigFood;  // A reference to the pig food being viewed.

        /// <summary>
        /// Constructs a PigFoodView.
        /// 
        /// The constructor must include a PigWorldView parameter so that all views
        /// created by the CreateView method(s) have similar signatures.
        /// </summary>
        /// <param name="pigFood"> the pig food to view. </param>
        public PigFoodView(PigWorldView pigWorldView, PigFood pigFood) : base(pigFood) {
            this.pigFood = pigFood;
        }

        /// <summary>
        /// Draws the RopePiece on the screen, scaled to the specified size.
        /// (Doesn't use an image from a file).
        /// 
        /// Overrides the Paint method in the base class, ThingView.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's image is displayed. </param>
        protected override void Paint(Graphics graphics) {
            int cellSize = Math.Min(thingViewRectangle.Width, thingViewRectangle.Height);
            int ovalWidth = cellSize * 5 / 7;
            int ovalHeight = cellSize / 3;
            int cakeHeight = ovalHeight * 2 / 3;
            int totalHeight = cakeHeight + ovalHeight;
            int x = (thingViewRectangle.Width - ovalWidth) / 2;
            int y1 = (thingViewRectangle.Height - totalHeight) / 2;
            int y2 = y1 + cakeHeight;

            graphics.FillRectangle(Brushes.Yellow, x, y2, ovalWidth, cakeHeight);
            graphics.FillEllipse(Brushes.Yellow, x, y2, ovalWidth, ovalHeight);
            graphics.FillEllipse(Brushes.Pink, x, y1, ovalWidth, ovalHeight);
        }
    }
}
