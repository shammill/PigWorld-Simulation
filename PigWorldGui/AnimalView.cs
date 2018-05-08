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
    /// The View class for an Animal.
    /// 
    /// Original (AWT) author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public abstract class AnimalView : LifeFormView {

        private Animal animal;  // A reference to the animal being viewed. 

        // Dimensions, in pixels. (See also the ThingView class.)
        protected const int ANIMAL_RIGHT_MARGIN_WIDTH = 20;
        protected const int ANIMAL_BOTTOM_MARGIN_HEIGHT = 20;

        protected const int ANIMAL_IMAGE_WIDTH = THING_VIEW_WIDTH - ANIMAL_RIGHT_MARGIN_WIDTH;
        protected const int ANIMAL_IMAGE_HEIGHT = THING_VIEW_HEIGHT - ANIMAL_BOTTOM_MARGIN_HEIGHT;

        protected readonly Rectangle animalRectangle = new Rectangle(0, 0, ANIMAL_IMAGE_WIDTH, ANIMAL_IMAGE_HEIGHT);
        protected readonly Rectangle bottomRectangle = new Rectangle(0, ANIMAL_IMAGE_HEIGHT, THING_VIEW_WIDTH, ANIMAL_BOTTOM_MARGIN_HEIGHT);
        protected readonly Rectangle righthandRectangle = new Rectangle(ANIMAL_IMAGE_WIDTH, 0, ANIMAL_RIGHT_MARGIN_WIDTH, ANIMAL_IMAGE_HEIGHT);

        /// <summary>
        /// Constructs the AnimalView.
        /// </summary>
        /// <param name="pigWorldView"></param>
        /// <param name="animal"></param>
        protected AnimalView(PigWorldView pigWorldView, Animal animal) 
            : base(pigWorldView, animal) {

            this.animal = animal;
        }

        /// <summary>
        /// Displays the animal's image and its other information on the screen.
        /// 
        /// Overrides the Paint method in the base class, ThingView.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's information is displayed. </param>
        protected override void Paint(Graphics graphics) {
            DisplayLifeFormImage(graphics);
            DisplayGenderColour(graphics);
            DisplayAge(graphics);
            DisplayEnergyLevel(graphics);
        }

        /// <summary>
        /// Displays the animal's image on the screen.
        /// 
        /// Overridden in derived classes, such as PigView and WolfView.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's image is displayed. </param>
        protected virtual void DisplayLifeFormImage(Graphics graphics) {
        }

        /// <summary>
        /// Displays the animal's gender-colour (if any) on the screen.
        /// 
        /// Overridden in derived classes that have genders, such as PigView, but not WolfView.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's gender-colour is displayed. </param>
        protected virtual void DisplayGenderColour(Graphics graphics) {
            // Display a white rectangle, when this method is not overridden.
            graphics.FillRectangle(Brushes.White, bottomRectangle);  
        }

        /// <summary>
        /// Displays the animal's age on the screen.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's age is displayed. </param>
        private void DisplayAge(Graphics graphics) {
            string ageString = "Age: " + animal.Age;
            SizeF ageStringSize = graphics.MeasureString(ageString, normalFont);
            PointF ageStringPoint = new PointF((bottomRectangle.Width - ageStringSize.Width) / 2, bottomRectangle.Top);
            graphics.DrawString(ageString, normalFont, ageBrush, ageStringPoint);
        }

        /// <summary>
        /// Displays the animal's energy-level on the screen.
        /// </summary>
        /// <param name="graphics"> the Graphics object on which the animal's energy-level is displayed. </param>
        private void DisplayEnergyLevel(Graphics graphics) {
            Brush emptyColorBrush = Brushes.WhiteSmoke;
            Brush energyColorBrush = Brushes.GreenYellow;
            const int fullEnergyLevel = Animal.STOMACH_FULL_LEVEL;

            int energyLevel = Math.Min(animal.Energy, fullEnergyLevel);
            int pixelLevel = energyLevel * righthandRectangle.Height / fullEnergyLevel;

            Rectangle emptyRectangle = righthandRectangle;
            emptyRectangle.Height -= pixelLevel;
            graphics.FillRectangle(emptyColorBrush, emptyRectangle);

            Rectangle energyRectangle = righthandRectangle;
            energyRectangle.Y = emptyRectangle.Height;
            energyRectangle.Height = pixelLevel;
            graphics.FillRectangle(energyColorBrush, energyRectangle);

            PointF energyStringPoint = new PointF(righthandRectangle.Left, righthandRectangle.Height / 2);
            graphics.DrawString(animal.Energy.ToString(), normalFont, Brushes.Black, energyStringPoint);
        }
    }
}
