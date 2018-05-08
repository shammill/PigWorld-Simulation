using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// A Plant is a type of LifeForm that is stationary, but still has behaviour. 
    /// For example, a Tree is one kind of Plant whose behaviour is to produce PigFood.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public abstract class Plant : LifeForm {
    }
}
