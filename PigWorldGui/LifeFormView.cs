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
    /// The View class for a LifeForm.
    /// 
    /// Original (AWT) author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public abstract class LifeFormView : ThingView {

        private LifeForm lifeForm;
        public LifeForm LifeForm { get { return lifeForm; } }

        private CellView containingCellView;  // A reference to the view of the cell containing this LifeForm.
        protected CellView ContainingCellView { get { return containingCellView; }
                                                set { containingCellView = value; } }

        private PigWorldView pigWorldView;  // The view of the pigWorld.
        protected PigWorldView PigWorldView { get { return pigWorldView; } }

        public ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

        /// <summary>
        /// Constructs the LifeFormView.
        /// </summary>
        /// <param name="pigWorldView"> the view of the pigWorld. </param>
        /// <param name="lifeForm"> the LifeForm to view. </param>
        protected LifeFormView(PigWorldView pigWorldView, LifeForm lifeForm) {
            this.pigWorldView = pigWorldView;
            this.lifeForm = lifeForm;

            Position position = lifeForm.Cell.Position;
            CellView cellView = pigWorldView.GetCellViewFromPosition(position);
            this.containingCellView = cellView;

            this.lifeForm.lifeFormMovedEvent += LifeFormMovedEvent;
        }

        /// <summary>
        /// This method is called whenever an Animal being viewed moves by itself,
        /// (or when drag&drop is used to move an Animal or a Plant, when implemented).
        /// </summary>
        public void LifeFormMovedEvent() {
            ContainingCellView.RemoveLifeForm();

            Position targetPos = lifeForm.Cell.Position;
            CellView targetCellView = PigWorldView.GetCellViewFromPosition(targetPos);
            targetCellView.AddLifeFormView(this);
            ContainingCellView = targetCellView;
        }
    }
}
