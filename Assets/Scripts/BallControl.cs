using UnityEngine;
using Assets.Scripts.Physics;

namespace Assets.Scripts
{
    public class BallControl : MonoBehaviour
    {
        public float InitialSpeed;

        private Vector2 _velocity;
	
        /// <summary>
        /// Called once per frame
        /// </summary>
        void Update () {

            // step the physics and set the ball's new position
            transform.position = StepPosition(Time.deltaTime, transform.position);
        }

        /// <summary>
        /// Calculates one physics step for the position of the ball
        /// </summary>
        /// <param name="timePassed">The amount of time that has passed since the last step</param>
        /// <param name="currentPosition">The current position to make calculations from</param>
        /// <returns>The new calculated position</returns>
        Vector2 StepPosition(float timePassed, Vector2 currentPosition)
        {
            // move the object
            var newPosition = currentPosition + _velocity * timePassed;

            return newPosition;
        }

        /// <summary>
        /// Steps the position n amount of step
        /// </summary>
        /// <param name="steps">The number of steps to make in the simulation</param>
        /// <param name="timePerStep">The amount of time (in seconds) that each step will take</param>
        /// <returns>An array containing the position calculated from each step</returns>
        Vector2[] StepPosition(int steps, float timePerStep, Vector2 currentPosition)
        {
            var positions = new Vector2[steps];

            // set the initial position
            positions[0] = currentPosition;

            for (int i = 1; i < steps; i++)
            {
                positions[i] = StepPosition(timePerStep, positions[i - 1]);
            }

            return positions;
        }
    }
}
