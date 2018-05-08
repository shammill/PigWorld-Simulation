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
    /// The View class for a Plant.
    /// 
    /// Original (AWT) author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public abstract class PlantView : LifeFormView {

        /// <summary>
        /// Constructs the AnimalView.
        /// </summary>
        /// <param name="pigWorldView"></param>
        /// <param name="plant"></param>
        public PlantView(PigWorldView pigWorldView, Plant plant) 
            : base(pigWorldView, plant) {
        }
    }
}
