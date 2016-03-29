using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Physics
{
    /// <summary>
    /// This is essentially just an info container, this will contain some basic info
    /// This class will also contian some methods for collision detection
    /// </summary>
    [ExecuteInEditMode]
    public class EdgeCollider : MonoBehaviour
    {
        // Keep a list of all active edge colliders for easy access from other places
        public static List<EdgeCollider> Colliders = new List<EdgeCollider>();

        // the size of the box in world units
        public float Length = 1;

        // the rotation of the line (in degrees)
        public float Rotation = 0;

        // the local positional offset
        public Vector2 Position;

        /// <summary>
        /// Called when the object is spawned, or when the game starts
        /// </summary>
        void Awake()
        {
            // add this collider to the list of colliders
            Colliders.Add(this);
        }

        /// <summary>
        /// Called when the object is deleted, or when the game stops
        /// </summary>
        void OnDestroyed()
        {
            // remove this collider from the list of colliders
            Colliders.Remove(this);
        }

        /// <summary>
        /// This is called in the unity editor
        /// It allows you to draw lines and other such things in the editor window
        /// </summary>
        void OnDrawGizmos()
        {
            // Draw the line in the editor for easy editing
            Gizmos.DrawLine(PointA, PointB);
        }

        /// <summary>
        /// Returns the first point of the line
        /// </summary>
        public Vector2 PointA
        {
            get
            {
                // get the center point of the line
                var centerPoint = (Vector2)transform.position + Position;

                // add half the length 
                var unrotatedPoint = centerPoint + Vector2.up * Length / 2;

                // rotate and return
                return MathUtils.RotatePoint(unrotatedPoint, Rotation, centerPoint);
            }
        }

        /// <summary>
        /// Returns the second point of the line
        /// </summary>
        public Vector2 PointB
        {
            get
            {
                // get the center point of the line
                var centerPoint = (Vector2)transform.position + Position;

                // subtract half the length
                var unrotatedPoint = centerPoint + Vector2.down * Length / 2;

                // rotate and return
                return MathUtils.RotatePoint(unrotatedPoint, Rotation, centerPoint);
            }
        }
    }
}
