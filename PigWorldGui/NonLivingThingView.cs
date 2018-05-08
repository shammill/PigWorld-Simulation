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
    /// The View class for a NonlivingThing. 
    /// 
    /// Original (AWT) author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public abstract class NonlivingThingView : ThingView {

        private NonlivingThing nonlivingThing;  // A reference to the nonlivingThing that this view displays.

        // Normally, NonlivingThings are displayed in a grid (i.e. a non-visible grid that is used *within* a CellView), 
        // so that we can see them all when there is more than one.
        // But for objects such as ropes (for which OnlyOneObjectOfThisTypePerCell is false),
        // we don't use a grid. (See CellView's OnPaint method for the actual algorithm.)
        public bool DisplayObjectInGridWithinCell { get { return nonlivingThing.OnlyOneObjectOfThisTypePerCell; } }

        /// <summary>
        /// Constructs a NonLivingThingView.
        /// 
        /// NonlivingThing model the nonlivingThing to view.
        /// </summary>
        protected NonlivingThingView(NonlivingThing nonlivingThing) {
            this.nonlivingThing = nonlivingThing;

        }

        public void Delete() {
            nonlivingThing.thingChangedEvent = delegate { };  //JDR: Is this really needed? I.e. is the nonlivingThing accessible anymore?
            nonlivingThing.thingDestroyedEvent = delegate { };
        }
//endNew
    }
}
