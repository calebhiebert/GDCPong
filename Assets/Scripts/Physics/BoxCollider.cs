﻿using UnityEngine;

namespace Assets.Scripts.Physics
{
    /// <summary>
    /// This is essentially just an info container, this will contain some basic info
    /// This class will also contian some methods for collision detection
    /// </summary>
    [ExecuteInEditMode]
    public class BoxCollider : MonoBehaviour
    {

        // the size of the box in world units
        public Vector2 Size;

        // the local positional offset
        public Vector2 Position;

        /// <summary>
        /// This is called in the unity editor
        /// It allows you to draw lines and other such things in the editor window
        /// </summary>
        void OnDrawGizmos()
        {
            // draw a square in the editor
            Gizmos.DrawWireCube((Vector2)transform.position + Position, Size);

            // draw a line from the origin of the object, to the collider's center
            // this makes it easier to see the square's offset
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Position);
        }

        /// <summary>
        /// Checks whether or not a given point is inside this box
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <returns>Whether or not the input point is inside the box</returns>
        public bool ContainsPoint(Vector2 point)
        {
            return point.x > transform.position.x + Position.x - Size.x/2 &&
                   point.x < transform.position.x + Position.x + Size.x/2 &&
                   point.y > transform.position.x + Position.y - Size.y/2 &&
                   point.y < transform.position.x + Position.y + Size.y/2;
        }

        /// <summary>
        /// Gets the unrotated corner points of this box collider
        /// </summary>
        /// <returns>An array of points in the format: [top right, top left, bottom right, bottom left]</returns>
        public Vector2[] GetPoints()
        {
            var points = new Vector2[4];

            // top right point
            points[0] = new Vector2(
                transform.position.x + Position.x - Size.x/2, 
                transform.position.y + Position.y + Size.y/2
                );

            // top left point
            points[1] = new Vector2(
                transform.position.x + Position.x + Size.x / 2,
                transform.position.y + Position.y + Size.y / 2
                );


            // bottom right point
            points[2] = new Vector2(
                transform.position.x + Position.x - Size.x / 2,
                transform.position.y + Position.y - Size.y / 2
                );

            // bottom left point
            points[3] = new Vector2(
                transform.position.x + Position.x + Size.x / 2,
                transform.position.y + Position.y - Size.y / 2
                );

            return points;
        }
    }
}
