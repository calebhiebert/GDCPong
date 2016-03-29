using System;
using UnityEngine;

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
            Gizmos.DrawWireCube((Vector2)transform.position + Position, Size);
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
    }
}
