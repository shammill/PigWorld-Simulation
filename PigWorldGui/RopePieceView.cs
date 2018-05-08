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
    /// The View class for a RopePiece.
    /// 
    /// Original (AWT) author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class RopePieceView : NonlivingThingView {

        private RopePiece ropePiece;  // The ropePiece being displayed.

        /// <summary>
        /// Constructs a RopeView.
        /// 
        /// The constructor must include a PigWorldView parameter so that all views
        /// created by the CreateView method(s) have similar signatures.
        /// </summary>
        /// <param name="ropePiece"> the ropePiece to view. </param>
        public RopePieceView(PigWorldView pigWorldView, RopePiece ropePiece)
            : base(ropePiece) {
            this.ropePiece = ropePiece;
        }

        /// <summary>
        /// Draws the RopePiece on the screen.  
        /// (Doesn't use an image from a file).
        /// 
        /// Overrides the Paint method in the base class, ThingView.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's image is displayed. </param>
        protected override void Paint(Graphics graphics) {
            PigWorld pigWorld = ropePiece.PigWorld;
            Position thisPos = ropePiece.Cell.Position;
            int indexOfThisRopePiece = ropePiece.GetIndex();

            // Get RopePiece dropped just before this one (if any).
            RopePiece previousRopePiece = ropePiece.OwnerPig.ropePieces.ElementAtOrDefault(indexOfThisRopePiece - 1);  
                                                                // Is null when this element does not exist.

            // Get RopePiece dropped just after this one (if any).
            RopePiece nextRopePiece = ropePiece.OwnerPig.ropePieces.ElementAtOrDefault(indexOfThisRopePiece + 1);
                                                                // Is null when this element does not exist.

            int viewWidth  = thingViewRectangle.Width;
            int viewHeight = thingViewRectangle.Height;

            Point midPoint = new Point(viewWidth / 2, viewHeight / 2);

            // Set the starting point for the line to be drawn.
            Point startPoint = new Point();
            // If there is a previous piece of rope, use its position.
            if (previousRopePiece != null) {
                Position previousPos = previousRopePiece.Cell.Position;
                startPoint.X = midPoint.X + (viewWidth / 2) * (previousPos.Column - thisPos.Column);
                startPoint.Y = midPoint.Y + (viewHeight / 2) * (previousPos.Row - thisPos.Row);
            } else {
                // Else, start in the middle of the cell.
                startPoint = midPoint;
            }

            // Set the ending point for the line to be drawn.
            Point endPoint = new Point();
            Position nextPos;
            // If there is a next piece of rope, use its position.
            if (nextRopePiece != null) {
                nextPos = nextRopePiece.Cell.Position;
            } else {
                // Else, use the pig's current position.
                nextPos = ropePiece.OwnerPig.Cell.Position;
            }
            endPoint.X = midPoint.X + (viewWidth / 2) * (nextPos.Column - thisPos.Column);
            endPoint.Y = midPoint.Y + (viewHeight / 2) * (nextPos.Row - thisPos.Row);

            // Use a rope width of 2 because the default width of 1 isn't always visible in small windows.
            const float penWidth = 2f;  

            using (Pen ropePen = new Pen(ropePiece.Color, penWidth)) {
                // If the line would be only along the edge of the cell, then draw two lines to make the rope clearer.
                if ( (startPoint.X == endPoint.X && (startPoint.X == 0 || startPoint.X == viewWidth))
                  || (startPoint.Y == endPoint.Y && (startPoint.Y == 0 || startPoint.Y == viewHeight)) ) {
                    graphics.DrawLine(ropePen, startPoint.X, startPoint.Y, midPoint.X, midPoint.Y);
                    graphics.DrawLine(ropePen, midPoint.X, midPoint.Y, endPoint.X, endPoint.Y);
                } else {
                    // Else draw just one line.
                    graphics.DrawLine(ropePen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                }
            }
        }
    }
}
