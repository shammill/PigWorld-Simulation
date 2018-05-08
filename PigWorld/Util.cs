using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;   // Allow Debug.Assert
using System.Media;         // For SoundPlayer

namespace PigWorldNamespace {

    /// <summary>
    /// This class contains various utility methods that may be useful to other classes.
    /// Each method in this class is static, so there is no need to create any
    /// instances of this class. Static methods in this class may be invoked in this way:
    /// 
    ///     SoundPlayer gruntAudio = Util.LoadSound(@"Resources\pig_grunt.wav");
    /// 
    /// Here, Util is the name of the class we are invoking the method on. It is
    /// only possible to invoke a method on a class if the method is static.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class Util {

        public static readonly Random random = new Random();

        /// <summary>
        /// This class only contains static fields and methods, so we would never
        /// use this constructor to make "Util" objects. Making the constructor
        /// private prevents people from trying to create instances of this class.
        /// </summary>
        private Util() { }

        /// <summary>
        /// Returns true when the testType is the same type as the baseType, 
        /// or when the testType is a subclass/subtype of the baseType.
        /// </summary>
        /// <param name="testType"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static bool IsSameTypeOrSubtype(Type testType, Type baseType) {
            return baseType.IsAssignableFrom(testType);
        }

        /// <summary>
        /// Converts degrees into radians.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static double GetRadiansFromDegrees(double degrees) { 
            return Math.PI * degrees / 180.0; 
        }
        
        /// <summary>
        /// Converts radians into degrees.
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double GetDegreesFromRadians(double radians) { 
            return radians * 180.0 / Math.PI; 
        }

        /// <summary>
        /// Interpolates between two integers.
        /// </summary>
        /// <param name="a"> the first integer.  </param>
        /// <param name="b"> the second integer. </param>
        /// <param name="scale"> a value between 0.0 and 1.0.
        /// 0.0 will return the value a,
        /// 1.0 will return the value b, and
        /// 0.5 will return a value half way between a and b. </param>
        /// <returns></returns>
        public static int Interpolate(int a, int b, double scale) {
            int length = b - a;
            return (int)(a + length * scale);
        }

        /// <summary>
        /// Creates a SoundPlayer for playing the sound that is stored 
        /// in the specified soundLocation.
        /// </summary>
        /// <param name="soundLocation"></param>
        /// <returns></returns>
        public static SoundPlayer LoadSound(string soundLocation) {
            SoundPlayer soundPlayer = new SoundPlayer(soundLocation);
            soundPlayer.Load();
            return soundPlayer;
        }
    }
}
