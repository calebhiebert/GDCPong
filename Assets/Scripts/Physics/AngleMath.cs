using UnityEngine;

namespace Assets.Scripts.Physics
{
    /// <summary>
    /// This class contains methods to help with angle computations
    /// </summary>
    public class AngleMath
    {
        /// <summary>
        /// Calculates the angle (in degrees) that this vector represents
        /// </summary>
        /// <param name="vector">The vector to convert</param>
        /// <returns>An angle in degrees</returns>
        public static float VectorToAngle(Vector2 vector)
        {
            // normalize this vector
            // this changes the vector so that all of its values add up to 1
            var normalized = vector.normalized;

            // get the angle in radians
            // convert the angle to degrees
            var angle = Mathf.Atan2(normalized.y, normalized.x)*Mathf.Rad2Deg;

            return angle;
        }

        /// <summary>
        /// Converts an angle to a heading vector
        /// For example, 90 degrees would be converted to (1, 0)
        /// </summary>
        /// <param name="angle">An angle in degrees</param>
        /// <returns>A vector representing the input angle</returns>
        public static Vector2 AngleToVector(float angle)
        {
            // convert the angle into radians
            var radians = angle*Mathf.Rad2Deg;

            var headingVector = new Vector2();

            headingVector.x = Mathf.Cos(radians);

            headingVector.y = Mathf.Sin(radians);

            return headingVector;
        }
    }
}
