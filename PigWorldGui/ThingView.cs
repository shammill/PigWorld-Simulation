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
    /// The View class for a Thing.
    /// 
    /// A ThingView displays a particular Thing onto the screen. For each type of Thing, 
    /// such as Wolf and Tree, there is a corresponding (sub)type of ThingView, 
    /// such as WolfView and TreeView, that is responsible for displaying it onto the screen.
    /// 
    /// The advantage of separating each thing into a separate model class (e.g. Wolf) 
    /// and view class (e.g. WolfView) is that it allows the programmer to
    /// create alternative ways of viewing the model of the pigWorld, without having
    /// to change the model itself. The model classes, essentially capture only the
    /// behaviour and the state of the entities in the pigWorld, and not the way they
    /// are displayed. So, it would be possible, for example, to create a
    /// 3-dimensional view of the pigWorld simply by creating new view classes, and
    /// this would require no changes at all to the model classes.
    /// 
    /// Each ThingView has a Paint method which is called whenever the Thing being viewed
    /// needs to be displayed on the screen.  Subclasses override this method to define 
    /// how that particular view displays its NonlivingThing onto the screen.
    /// 
    /// Original (AWT) author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    /// <see cref="Thing"/>
    public abstract class ThingView {

        // Dimensions, in pixels. (See also the AnimalView class.)
        protected const int THING_VIEW_WIDTH = 120;
        protected const int THING_VIEW_HEIGHT = 120;

        // The bitmap where the Things on-screen image is drawn.
        private Bitmap bitmap = new Bitmap(THING_VIEW_WIDTH, THING_VIEW_HEIGHT);

        protected readonly Rectangle thingViewRectangle = new Rectangle(0, 0, THING_VIEW_WIDTH, THING_VIEW_HEIGHT);

        protected static readonly Font normalFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);
        protected static readonly Font debugInfoFont = new Font("Times New Roman", 16, FontStyle.Regular, GraphicsUnit.Pixel);

        // Used when displaying the thing's age.  
        // Default colour is Black but may be changed in derived classes, such as PigView.
        protected Brush ageBrush = Brushes.Black;

        /// <summary>
        /// Displays the Things's image and any other information on the screen,
        /// by obtaining a Graphics object for the bitmap above, and then calling 
        /// the Paint method.
        /// </summary>
        /// <returns></returns>
        public Bitmap PaintImage() {

            using (Graphics graphics = Graphics.FromImage(bitmap)) {
                Paint(graphics);
            }

            return bitmap;
        }

        /// <summary>
        /// Displays the Things's image and any other information on the screen.
        /// 
        /// Overriden in classes such as AnimalView, TreeView and PigFoodView.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's image is displayed. </param>
        protected abstract void Paint(Graphics graphics);
    }
}
