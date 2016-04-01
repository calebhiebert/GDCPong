using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// This class contains methods to help with math stuff
    /// </summary>
    public class MathUtils
    {
        /// <summary>
        /// Calculates the angle (in degrees) that this vector represents
        /// </summary>
        /// <param name="vector">The vector to convert</param>
        /// <returns>An angle in degrees</returns>
        public static float VectorToAngle(Vector2 vector)
        {
            // Normalize this vector
            // This changes the vector so that all of its values add up to 1
            var normalized = vector.normalized;

            // Get the angle in radians
            // Convert the angle to degrees
            // Subtract 90 to align with unity coordinates
            var angle = Mathf.Repeat(Mathf.Atan2(normalized.y, normalized.x)*Mathf.Rad2Deg - 90, 360);

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
            // Convert the angle into radians
            var radians = (angle + 90)*Mathf.Deg2Rad;

            var headingVector = new Vector2();

            headingVector.x = Mathf.Cos(radians);

            headingVector.y = Mathf.Sin(radians);

            return headingVector;
        }

        /// <summary>
        /// Rotates a point around an origin point
        /// </summary>
        /// <param name="point">The point to rotate</param>
        /// <param name="angle">The number of degrees to rotate this point by</param>
        /// <param name="origin">The origin to rotate the point around</param>
        /// <returns>The rotated point</returns>
        public static Vector2 RotatePoint(Vector2 point, float angle, Vector2 origin)
        {
            var sin = Mathf.Sin((angle * -1) * Mathf.Deg2Rad);
            var cos = Mathf.Cos((angle * -1) * Mathf.Deg2Rad);

            var p = new Vector2(point.x - origin.x, point.y - origin.y);

            var rotatedPoint = new Vector2(p.x * cos - p.y * sin, p.x * sin + p.y * cos);

            rotatedPoint.x += origin.x;
            rotatedPoint.y += origin.y;

            return rotatedPoint;
        }

        /// <summary>
        /// A helper method used for calculating ball trajectory
        /// Takes a line direction (essentially a velocity) and a x coordinate, and returns where they intersect
        /// </summary>
        /// <param name="lineDirection">The direction the line is pointing in</param>
        /// <param name="xCoordinate">The x coordinate to check for intersections</param>
        /// <returns>The point where these lines intersect</returns>
        public static Vector2 LineIntersectXCoordinate(Vector2 lineDirection, Vector2 lineOriginPosition, float xCoordinate)
        {
            Vector2 a1 = lineOriginPosition;
            Vector2 a2 = lineOriginPosition + lineDirection * 20;

            // Create two line points from the X coordinate
            Vector2 b1 = new Vector2(xCoordinate, 1f);
            Vector2 b2 = new Vector2(xCoordinate, -1f);

            // Calculate the intersection and return it
            return LineIntersectionPoint(a1, a2, b1, b2);
        }

        /// <summary>
        /// Calculates the point where two lines intersect
        /// </summary>
        /// <param name="a1">Point A of line 1</param>
        /// <param name="b1">Point B of line 1</param>
        /// <param name="a2">Point A of line 2</param>
        /// <param name="b2">Point B of line 2</param>
        /// <returns>The point where two given lines intersect</returns>
        public static Vector2 LineIntersectionPoint(Vector2 a1, Vector2 b1, Vector2 a2, Vector2 b2)
        {

            // Get A,B,C of first line - points : a1 to b1
            float A1 = b1.y - a1.y;
            float B1 = a1.x - b1.x;
            float C1 = A1 * a1.x + B1 * a1.y;

            // Get A,B,C of second line - points : a2 to b2
            float A2 = b2.y - a2.y;
            float B2 = a2.x - b2.x;
            float C2 = A2 * a2.x + B2 * a2.y;

            // Get delta and check if the lines are parallel
            float delta = A1 * B2 - A2 * B1;

            if (delta == 0)
                throw new System.Exception("Lines are parallel");

            // Return the Vector2 intersection point
            return new Vector2(
                (B2 * C1 - B1 * C2) / delta,
                (A1 * C2 - A2 * C1) / delta
            );
        }
    }
}
