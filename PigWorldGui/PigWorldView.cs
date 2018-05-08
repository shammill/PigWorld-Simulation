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
    /// The View class for the PigWorld object.
    /// 
    /// The PigWorldView is a visual component that displays everything that is happening
    /// in the pigWorld.  It is a (sub)type of TableLayoutPanel.
    /// 
    /// The key difference between this class and the PigWorldForm is that 
    /// the PigWorldForm displays all the GUI items, including all the user controls (buttons, etc.),
    /// whereas this PigWorldView only displays the squares/cells in the PigWorld.
    /// 
    /// Author: Jim Reye
    /// </summary>
    public class PigWorldView : TableLayoutPanel {

        const int NUM_OF_ROWS = 9;
        const int NUM_OF_COLUMNS = 9;

        private static PigWorld pigWorld = new PigWorld(NUM_OF_ROWS, NUM_OF_COLUMNS);
        public PigWorld PigWorld { get { return pigWorld; } }

        private CellView[,] cellViews;
        // A 2-dimensional array of CellViews, each of which views a cell in the pigWorld.

        private GapView[,] horizontalGapViews;
        // A 2-dimensional array of GapViews, each of which views a horizontal Gap in the pigWorld.

        private GapView[,] verticalGapViews;
        // A 2-dimensional array of GapViews, each of which views a vertical Gap in the pigWorld.

        // The current type of object to add when building the pigWorld, e.g. GirlPig, Tree, etc.
        private Type currentTypeOfObjectToAdd;
        public Type CurrentTypeOfObjectToAdd {
            get { return currentTypeOfObjectToAdd; }
            set { currentTypeOfObjectToAdd = value; }
        }

        /// <summary>
        /// Construct the PigWorldView.
        /// </summary>
        public PigWorldView() {

            InitialiseTableLayoutPanel();
            CreateCellViews();
            CreateGapViews();
            AddViewsToTableLayoutPanel();

            pigWorld.showDebugInfoChangedEvent += ShowDebugInfoChangedEvent;
            pigWorld.worldThingAddedEvent += WorldThingAddedEvent;
            pigWorld.worldThingRemovedEvent += WorldThingRemovedEvent;

            // Set the initial type of object to add when building the pigWorld, e.g. GirlPig, Tree, etc.
            CurrentTypeOfObjectToAdd = typeof(BoyPig);
        }
        
        /// <summary>
        /// Initialises the TableLayoutPanel (the base class of this class).
        /// </summary>
        private void InitialiseTableLayoutPanel() {
            this.BackColor = Color.LightGreen;
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.Dock = DockStyle.Fill;  // Occupy all the available space.
            this.DoubleBuffered = true;  // Reduces flickering when resized.
        }

        /// <summary>
        /// Creates all the CellViews in this PigWorldView.
        /// </summary>
        private void CreateCellViews() {

            cellViews = new CellView[pigWorld.NumOfRows, pigWorld.NumOfColumns];
            for (int row = 0; row < pigWorld.NumOfRows; row++) {
                for (int column = 0; column < pigWorld.NumOfColumns; column++) {
                    Cell cell = pigWorld.GetCell(new Position(row, column));
                    CellView newCellView = new CellView(this, cell);
                    cellViews[row, column] = newCellView;
                }
            }
        }

        /// <summary>
        /// Creates all the GapsViews in this PigWorldView.
        /// </summary>
        private void CreateGapViews() {

            horizontalGapViews = new GapView[pigWorld.NumOfHorizontalGapsPerColumn, pigWorld.NumOfColumns];
            for (int row = 0; row < pigWorld.NumOfHorizontalGapsPerColumn; row++) {
                for (int column = 0; column < pigWorld.NumOfColumns; column++) {
                    Gap gap = pigWorld.GetGap(row, column, GapDirection.Horizontal);
                    GapView gapView = new GapView(this, gap);
                    horizontalGapViews[row, column] = gapView;
                }
            }

            verticalGapViews = new GapView[pigWorld.NumOfRows, pigWorld.NumOfVerticalGapsPerRow];
            for (int row = 0; row < pigWorld.NumOfRows; row++) {
                for (int column = 0; column < pigWorld.NumOfVerticalGapsPerRow; column++) {
                    Gap gap = pigWorld.GetGap(row, column, GapDirection.Vertical);
                    GapView gapView = new GapView(this, gap);
                    verticalGapViews[row, column] = gapView;
                }
            }
        }

        /// <summary>
        /// Add all the CellsViews and GapViews to the TableLayoutPanel (the base class of this class).
        /// </summary>
        private void AddViewsToTableLayoutPanel() {

            // Add the cellViews to the TableLayoutPanel.
            for (int row = 0; row < pigWorld.NumOfRows; row++) {
                for (int column = 0; column < pigWorld.NumOfColumns; column++) {
                    this.Controls.Add(cellViews[row, column], (column * 2), (row * 2));
                }
            }

            // Add the horizontalGapViews to the TableLayoutPanel.
            for (int row = 0; row < pigWorld.NumOfHorizontalGapsPerColumn; row++) {
                for (int column = 0; column < pigWorld.NumOfColumns; column++) {
                    this.Controls.Add(horizontalGapViews[row, column], (column * 2), 1 + (row * 2));
                }
            }

            // Add the verticalGapViews to the TableLayoutPanel.
            for (int row = 0; row < pigWorld.NumOfRows; row++) {
                for (int column = 0; column < pigWorld.NumOfVerticalGapsPerRow; column++) {
                    this.Controls.Add(verticalGapViews[row, column], 1 + (column * 2), (row * 2));
                }
            }
        }

        /// <summary>
        /// When the PigWorldView is resized, recalculate all the CellView and GapView widths & heights.
        /// 
        /// Overrides the OnResize method in the base class, TableLayoutPanel.
        /// </summary>
        /// <param name="eventArgs"></param>
        protected override void OnResize(EventArgs eventArgs) {
            base.OnResize(eventArgs);
            SetHeightsAndWidths();
        }

        /// <summary>
        /// When the PigWorldView is resized, recalculate all the CellView and GapView widths & heights.
        /// </summary>
        private void SetHeightsAndWidths() {

            this.SuspendLayout();

            this.RowStyles.Clear();     // Remove any existing RowStyles, just to be safe.
            this.ColumnStyles.Clear();  // Remove any existing ColumnStyles, just to be safe.

            AddRowOrColumnStyles(true);
            AddRowOrColumnStyles(false);

            this.ResumeLayout();
        }

        /// <summary>
        /// Add row/column styles to the TableLayoutPanel.
        /// </summary>
        /// <param name="forRows"></param>
        private void AddRowOrColumnStyles(bool forRows) {  // If false, then for columns.
            
            const double fractionOfSpaceForAllGaps = 0.10;// I.e. 10%

            int tableLayoutPanelSizeInPixels;
            int numOfRowsOrColumns;
            if (forRows) {
                tableLayoutPanelSizeInPixels = this.Height;
                numOfRowsOrColumns = pigWorld.NumOfRows;
            }
            else {
                tableLayoutPanelSizeInPixels = this.Width;
                numOfRowsOrColumns = pigWorld.NumOfColumns;
            }

            // If we've just been minimised, then don't do anything yet.
            if (tableLayoutPanelSizeInPixels == 0) {
                return;
            }

            int numOfGaps = numOfRowsOrColumns - 1;

            int numOfRowsOrColumns_AndGaps = numOfRowsOrColumns + numOfGaps;

            int pixelsAvailable = tableLayoutPanelSizeInPixels - (1 + numOfRowsOrColumns_AndGaps);  // Subtract for the single-line border style.
            int gapSizeInPixels = (int)(pixelsAvailable * fractionOfSpaceForAllGaps / numOfGaps);
            int remainingPixels = pixelsAvailable - (gapSizeInPixels * numOfGaps);
            int numOfRowsOrColumnsRemaining = numOfRowsOrColumns;

            for (int i = 0; i < numOfRowsOrColumns_AndGaps; i++) {
                // If it's a gap
                if (i % 2 == 1) {
                    if (forRows) {
                        this.RowStyles.Add(new RowStyle(SizeType.Absolute, gapSizeInPixels));
                    }
                    else {
                        this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, gapSizeInPixels));
                    }
                }
                else { // It's a cell
                    int cellSizeInPixels = remainingPixels / numOfRowsOrColumnsRemaining;
                    if (forRows) {
                        this.RowStyles.Add(new RowStyle(SizeType.Absolute, cellSizeInPixels));
                    }
                    else {
                        this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, cellSizeInPixels));
                    }

                    remainingPixels -= cellSizeInPixels;
                    numOfRowsOrColumnsRemaining -= 1;
                }
            }
        }

        /// <summary>
        /// Get a reference to the CellView that is at the given position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public CellView GetCellViewFromPosition(Position position) {
            return cellViews[position.Row, position.Column];
        }

        /// <summary>
        /// Event-handler for when the PigWorld's ShowDebugInfo value changes.
        /// </summary>
        public void ShowDebugInfoChangedEvent() {
            for (int row = 0; row < pigWorld.NumOfRows; row++)
                for (int column = 0; column < pigWorld.NumOfColumns; column++)
                    cellViews[row, column].Cell.Pulse();
        }

        /// <summary>
        /// Event-handler for when a Thing is added to the PigWorld.
        /// </summary>
        public void WorldThingAddedEvent(Thing thing) {

            if (thing is LifeForm) {  
                Position position = thing.Cell.Position;
                LifeFormView view = (LifeFormView)CreateView(thing);
                cellViews[position.Row, position.Column].AddLifeFormView(view);
            }
        }

        /// <summary>
        /// Event-handler for when a Thing is removed from the PigWorld.
        /// </summary>
        public void WorldThingRemovedEvent(Thing thing) {
            // Do nothing, currently.
        }

        /// <summary>
        /// Set-up the first demo, by adding various objects (and walls) to PigWorld.
        /// </summary>
        public void SetupDemo1() {
            BoyPig boyPig = new BoyPig(PigWorld, new Position(5, 2));
            GirlPig girlPig = new GirlPig(PigWorld, new Position(7, 7));

            PigFood pigFood1 = new PigFood(PigWorld, new Position(1, 1));
            PigFood pigFood2 = new PigFood(PigWorld, new Position(1, 5));
            PigFood pigFood3 = new PigFood(PigWorld, new Position(2, 2));
            PigFood pigFood4 = new PigFood(PigWorld, new Position(2, 5));
            PigFood pigFood5 = new PigFood(PigWorld, new Position(3, 3));
            PigFood pigFood6 = new PigFood(PigWorld, new Position(3, 4));
            PigFood pigFood7 = new PigFood(PigWorld, new Position(3, 5));
            PigFood pigFood8 = new PigFood(PigWorld, new Position(4, 4));
            PigFood pigFood9 = new PigFood(PigWorld, new Position(4, 5));
            PigFood pigFood10 = new PigFood(PigWorld, new Position(5, 4));
            PigFood pigFood11 = new PigFood(PigWorld, new Position(5, 5));

            CreateDemoWalls();
        }

        /// <summary>
        /// Set-up the second demo, by adding various objects (and walls) to PigWorld.
        /// </summary>
        public void SetupDemo2() {

            BoyPig boyPig = new BoyPig(PigWorld, new Position(0, 0));
            GirlPig girlPig = new GirlPig(PigWorld, new Position(2, 0));

            Tree tree1 = new Tree(PigWorld, new Position(0, 4));
            Tree tree2 = new Tree(PigWorld, new Position(7, 5));

            Wolf wolf = new Wolf(PigWorld, new Position(8, 8));

            PigFood pigFood1 = new PigFood(PigWorld, new Position(0, 5));
            PigFood pigFood2 = new PigFood(PigWorld, new Position(3, 3));
            PigFood pigFood3 = new PigFood(PigWorld, new Position(6, 0));

            CreateDemoWalls();
        }

        /// <summary>
        /// Set-up the third demo, by adding various objects (and walls) to PigWorld.
        /// </summary>
        public void SetupDemo3() {

            BoyPig boyPig = new BoyPig(PigWorld, new Position(4, 3));
            GirlPig girlPig = new GirlPig(PigWorld, new Position(4, 5));

            Tree tree1 = new Tree(PigWorld, new Position(0, 4));
            Tree tree2 = new Tree(PigWorld, new Position(4, 4));
            Tree tree3 = new Tree(PigWorld, new Position(8, 4));

            Wolf wolf = new Wolf(PigWorld, new Position(0, 0));

            PigFood pigFood1 = new PigFood(PigWorld, new Position(8, 0));
            PigFood pigFood2 = new PigFood(PigWorld, new Position(8, 1));
            PigFood pigFood3 = new PigFood(PigWorld, new Position(7, 0));
            PigFood pigFood4 = new PigFood(PigWorld, new Position(1, 8));
            PigFood pigFood5 = new PigFood(PigWorld, new Position(0, 8));
            PigFood pigFood6 = new PigFood(PigWorld, new Position(0, 7));

            PigWorld.FillVerticalGap(2, 5);
            PigWorld.FillVerticalGap(3, 1);
            PigWorld.FillVerticalGap(3, 6);
            PigWorld.FillVerticalGap(4, 1);
            PigWorld.FillVerticalGap(4, 6);
            PigWorld.FillVerticalGap(5, 1);
            PigWorld.FillVerticalGap(5, 6);
            PigWorld.FillVerticalGap(6, 2);

            PigWorld.FillHorizontalGap(1, 3);
            PigWorld.FillHorizontalGap(1, 4);
            PigWorld.FillHorizontalGap(1, 5);
            PigWorld.FillHorizontalGap(3, 1);
            PigWorld.FillHorizontalGap(3, 7);
            PigWorld.FillHorizontalGap(4, 0);
            PigWorld.FillHorizontalGap(4, 8);
            PigWorld.FillHorizontalGap(6, 3);
            PigWorld.FillHorizontalGap(6, 4);
            PigWorld.FillHorizontalGap(6, 5);
        }

        /// <summary>
        /// PigWorld already has gaps between each pair of cells, 
        /// so put walls in some of them, for demonstration purposes.
        /// </summary>
        private void CreateDemoWalls() {
            PigWorld.FillVerticalGap(1, 2);
            PigWorld.FillVerticalGap(2, 2);
            PigWorld.FillVerticalGap(3, 2);
            PigWorld.FillVerticalGap(4, 2);
            PigWorld.FillVerticalGap(5, 2);
            PigWorld.FillVerticalGap(2, 4);
            PigWorld.FillVerticalGap(3, 4);
            PigWorld.FillVerticalGap(4, 4);
            PigWorld.FillVerticalGap(5, 4);
            PigWorld.FillHorizontalGap(6, 0);
            PigWorld.FillHorizontalGap(6, 1);
            PigWorld.FillHorizontalGap(6, 2);
            PigWorld.FillHorizontalGap(6, 3);

            PigWorld.FillVerticalGap(0, 5);
            PigWorld.FillVerticalGap(1, 5);
            PigWorld.FillVerticalGap(2, 5);
            PigWorld.FillVerticalGap(3, 5);
            PigWorld.FillVerticalGap(4, 5);
            PigWorld.FillVerticalGap(5, 5);

            PigWorld.FillVerticalGap(3, 7);
            PigWorld.FillVerticalGap(4, 7);
            PigWorld.FillVerticalGap(6, 7);
            PigWorld.FillVerticalGap(7, 7);
            PigWorld.FillVerticalGap(8, 7);
        }

        /// <summary>
        /// Creates a view of the object specified in the parameter, e.g. a PigView of a Pig object.
        /// 
        /// This method must never be called directly by any program code, 
        /// apart from the WorldThingAddedEvent method above, and 
        /// CellView's NonlivingThingPutDownEvent method.
        /// </summary>
        /// <param name="model"> the object that we're creating a view of </param>
        /// <returns> the view that is created </returns>
        public Object CreateView(Object thing) {
            Type thingType = thing.GetType();
            string thingTypeName = thingType.Name;

            switch (thingTypeName) {
                case "BoyPig": return new PigView(this, (BoyPig)thing);
                case "GirlPig": return new PigView(this, (GirlPig)thing);
                case "PigFood": return new PigFoodView(this, (PigFood)thing);
                case "RopePiece": return new RopePieceView(this, (RopePiece)thing);
                case "Tree": return new TreeView(this, (Tree)thing);
                case "Wolf": return new WolfView(this, (Wolf)thing);
                default: throw new Exception("Unexpected type name " + thingTypeName);
            }
        }
    }
}
