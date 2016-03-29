using UnityEngine;

namespace Assets.Scripts.Physics
{
    [ExecuteInEditMode]
    public class CircleCollider : MonoBehaviour
    {
        // The radius of the circle
        public float Radius;

        // The local offset of the collider
        public Vector2 Position;

        /// <summary>
        /// Draw a circle in the unity editor for easier editing
        /// </summary>
        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere((Vector2) transform.position + Position, Radius);

            // draw a line from the origin of the object, to the collider's center
            // this makes it easier to see the circles offset
            Gizmos.DrawLine(transform.position, (Vector2) transform.position + Position);
        }

        /// <summary>
        /// Checks whether or not a given point is inside this circle
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <returns>Whether or not the point is inside the circle</returns>
        public bool ContainsPoint(Vector2 point)
        {
            // Check that the distance from the centre of the circle to the point is less than the radius
            return Vector2.Distance(point, (Vector2) transform.position + Position) <= Radius;
        }
    }
}
