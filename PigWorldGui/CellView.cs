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
    /// The View class for a Cell object.
    /// 
    /// Original (AWT) author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class CellView : PictureBox {

        private Cell cell;  // A reference to the cell being viewed.
        public Cell Cell { get { return cell; } }

        private PigWorldView pigWorldView;  // A reference to the view of the pigWorld.

        private Color soundLevelColor = Color.Yellow;  // The colour we use to display sound-levels when debug info is shown.

        private int numOfObjectsInGrid = 0;  // The number of NonlivingThings that are displayed in a grid (within this CellView).

        private LifeFormView lifeFormView;  // A reference to the LifeFormView for this cell, if any.

        protected static readonly Font soundLevelFont = new Font("Times New Roman", 16, FontStyle.Regular, GraphicsUnit.Pixel);

        // A mapping of each NonlivingThing object to its view.
        private Dictionary<NonlivingThing, NonlivingThingView> nonLivingThingViews = new Dictionary<NonlivingThing, NonlivingThingView>(); // NonlivingThing -> NonLivingThingView

        /// <summary>
        /// Constructs the CellView, to view the specified cell.
        /// </summary>
        /// <param name="pigWorldView"> the view of the pigWorld </param>
        /// <param name="cell"> the cell to view </param>
        public CellView(PigWorldView pigWorldView, Cell cell) {
            this.pigWorldView = pigWorldView;
            this.cell = cell;

            this.cell.nonlivingThingPutDownEvent += NonlivingThingPutDownEvent;
            this.cell.nonlivingThingPickedUpEvent += NonlivingThingPickedUpEvent;
            this.cell.cellChangedEvent += CellChangedEvent;

            // Fill the entire space available to this cell.
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0);
            this.SizeMode = PictureBoxSizeMode.StretchImage;

        }

        /// <summary>
        /// Event-handler for when a NonlivingThing is put down on this CellView's Cell.
        /// </summary>
        /// <param name="nonlivingThing"> the NonlivingThing being put down on the Cell</param>
        public void NonlivingThingPutDownEvent(NonlivingThing nonlivingThing) {
            NonlivingThingView nonLivingThingView = (NonlivingThingView)pigWorldView.CreateView(nonlivingThing);
            nonLivingThingViews.Add(nonlivingThing, nonLivingThingView);

            if (nonLivingThingView.DisplayObjectInGridWithinCell) 
                numOfObjectsInGrid += 1;

            Invalidate();  // Cause the OnPaint method to repaint this CellView.
        }

        /// <summary>
        /// Event-handler for when a NonlivingThing is picked up from this CellView's Cell.
        /// </summary>
        /// <param name="nonlivingThing"> the NonlivingThing being picked up from the Cell</param>
        public void NonlivingThingPickedUpEvent(NonlivingThing nonlivingThing) {
            NonlivingThingView nonLivingThingView = nonLivingThingViews[nonlivingThing];
            nonLivingThingViews.Remove(nonlivingThing);

            if (nonLivingThingView.DisplayObjectInGridWithinCell) {
                numOfObjectsInGrid -= 1;
            }

            Invalidate();  // Cause the OnPaint method to repaint this CellView.
        }

        /// <summary>
        /// Event-handler for when the cell changes in some way,
        /// other than having a nonlivingThing put-down or picked-up.
        /// </summary>
        public void CellChangedEvent() {

            // If there is a LifeFormView, refresh its image.
            if (this.lifeFormView != null)
                this.Image = lifeFormView.PaintImage();

            Invalidate();  // Cause the OnPaint method to repaint this CellView.
        }

        /// <summary>
        /// Adds a LifeFormView to this CellView.
        /// This happens when a LifeForm is added to a Cell, either by the user,
        /// or when an animal moves from one cell to another.
        /// </summary>
        /// <param name="view"> the LifeFormView to add. </param>
        public void AddLifeFormView(LifeFormView lifeFormView) {
            
            // Check that the cell has no life-form already.  (It may have nonLivingThings, e.g. pig food.)
            Debug.Assert(this.lifeFormView == null);
            this.lifeFormView = lifeFormView;

            // Update this cell's image.
            this.Image = lifeFormView.PaintImage();

            // If the life-form changes or is destroyed, tell this CellView about it.
            this.lifeFormView.LifeForm.thingChangedEvent += ThingChangedEvent;
            this.lifeFormView.LifeForm.thingDestroyedEvent += ThingDestroyedEvent;
            this.ContextMenuStrip = this.lifeFormView.contextMenuStrip;  // Assign the context-menu to this CellView.
        }

        /// <summary>
        /// This method is called whenever the internal state of the life-form being
        /// viewed changes. This allows the CellView to reflect those changes
        /// immediately onto the screen.
        /// </summary>
        public void ThingChangedEvent() {
            Debug.Assert(lifeFormView != null);

            // Refresh the image.
            this.Image = lifeFormView.PaintImage();
        }

        /// <summary>
        /// This method will be called when a life-form being viewed is destroyed.
        /// </summary>
        public void ThingDestroyedEvent() {
            RemoveLifeForm();
        }

        /// <summary>
        /// Removes a LifeFormView from this CellView.
        /// This happens when a LifeForm is removed from the Cell, either by the user,
        /// or when an animal moves from one cell to another.
        /// </summary>
        /// <param name="view"> the LifeFormView to remove. </param>
        public void RemoveLifeForm() {

            // When the life-form changes or is destroyed, no longer tell this CellView about it.
            this.lifeFormView.LifeForm.thingChangedEvent -= ThingChangedEvent;
            this.lifeFormView.LifeForm.thingDestroyedEvent -= ThingDestroyedEvent;

            this.lifeFormView = null;
            this.Image = null;
            this.ContextMenuStrip = null;  // Remove the context-menu.
        }

        /// <summary>
        /// This method is called whenever the cell needs displaying.
        ///
        /// Overrides the OnPaint method in the base class, PictureBox.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e) {

            // Let the base class (PictureBox) do its normal painting.
            // If there is a LifeForm in the Cell, that is drawn automatically by the PictureBox.
            // Otherwise, the PictureBox code will paint the background for us.
            base.OnPaint(e);  

            // If the cell doesn't have a LifeForm occupying it, then display the NonlivingThings (if any).
            // (See comments about the GUI, near the top of Cell.cs.)
            //if (this.Image == null) {
            if (this.cell.LifeFormOccupant != null) {
                return;
            }

            Graphics graphics = e.Graphics;

            // If in debug mode, display the sound waves
            if (cell.PigWorld.ShowDebugInfo) {
                DisplayCellNumber(graphics);
                DisplaySoundLevels(graphics);
            }

            PaintNonlivingThings(graphics, e.ClipRectangle);
        }

        /// <summary>
        /// Displays the Cell's position on the screen, 
        /// e.g. "[2,3]" for row 2, column 3.
        /// </summary>
        /// <param name="graphics"></param>
        private void DisplayCellNumber(Graphics graphics) {
            PointF cellNumberTextPoint = new PointF(0, 0);
            graphics.DrawString(cell.Position.ToString(), soundLevelFont, Brushes.Black, cellNumberTextPoint);
        }

        /// <summary>
        /// Displays the Cell's (virtual) sound-level on the screen.
        /// </summary>
        /// <param name="graphics"></param>
        private void DisplaySoundLevels(Graphics graphics) {
            const int maxSound = PigWorld.OINK_SOUND_LEVEL;

            int soundLevel = cell.Air.SoundLevel;
            double scale = (double)soundLevel / maxSound;

            int red = Util.Interpolate(this.BackColor.R, soundLevelColor.R, scale);
            int green = Util.Interpolate(this.BackColor.G, soundLevelColor.G, scale);
            int blue = Util.Interpolate(this.BackColor.B, soundLevelColor.B, scale);

            Color displaySoundColor = Color.FromArgb(red, green, blue);
            SolidBrush displaySoundBrush = new SolidBrush(displaySoundColor);

            int circleSize = 22;
            int circleOffsetX = (this.Width - circleSize) / 2;
            int circleOffsetY = this.Height - circleSize;
            graphics.FillEllipse(displaySoundBrush, circleOffsetX, circleOffsetY, circleSize, circleSize);

            string sndText = soundLevel.ToString();
            SizeF sndTextSize = graphics.MeasureString(sndText, soundLevelFont);
            PointF sndTextPoint = new PointF((this.Width - sndTextSize.Width) / 2, this.Height - sndTextSize.Height);
            graphics.DrawString(sndText, soundLevelFont, Brushes.Black, sndTextPoint);
        }

        /// <summary>
        /// Display the NonlivingThings on the screen.  
        /// Some might be in a grid and some might not be.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="ClipRectangle"></param>
        private void PaintNonlivingThings(Graphics graphics, Rectangle ClipRectangle) {

            int gridItemSize = Math.Min(this.Width, this.Height) / 3;

            int gridOrder = (int)Math.Ceiling(Math.Sqrt(numOfObjectsInGrid));
            int gridSize = gridOrder * gridItemSize;

            int left = (this.Width - gridSize) / 2;
            int top = (this.Height - gridSize) / 2;

            int row = 0;
            int col = 0;

            foreach (NonlivingThingView view in nonLivingThingViews.Values) {

                if (view.DisplayObjectInGridWithinCell) {
                    Image viewImage = view.PaintImage();
                    Rectangle viewRectangle = new Rectangle(left + col * gridItemSize, top + row * gridItemSize, gridItemSize, gridItemSize);
                    graphics.DrawImage(viewImage, viewRectangle);

                    col = (col + 1) % gridOrder;
                    if (col == 0)
                        row += 1;
                } else { // E.g. for a rope.
                    Image viewImage = view.PaintImage();
                    graphics.DrawImage(viewImage, ClipRectangle);
                }
            } // foreach
        }

        /// <summary>
        /// Creates a string representation of this Cell. 
        /// This is useful for debugging purposes and any other times 
        /// when you want to have a string showing what value(s) an object has.
        /// </summary>
        /// <returns> a string representation of this Cell </returns>
        public override string ToString() {
            return "cellView of " + cell.ToString();
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (this.lifeFormView == null) {
                    Type typeOfNewObject = pigWorldView.CurrentTypeOfObjectToAdd;
                    Position position = cell.Position;
                    cell.PigWorld.AddThingOfType(typeOfNewObject, position);
                }
            } else {
                base.OnMouseDown(e);
            }
        }
    }
}
