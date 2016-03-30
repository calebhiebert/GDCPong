using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// This script goes on the paddle GameObject
    /// It allows a player to move the paddle around based on up and down inputs
    /// It also allows the paddle to be controlled by the computer
    /// </summary>
    public class PaddleControl : MonoBehaviour
    {
        // The maximum speed the paddle can move at (in unity-units per second)
        public float Speed;

        // How far the paddle can travel on the y axis
        public float YMovementConstraint;

        // The name of the axis to get input from
        // For w and s, use "Player1"
        // For up and down arrows, use "Player2"
        public string AxisName = "Player1";

        // Should the ai be used
        public bool ComputerControlled;

        // A reference to the ball object, only used for ai calculations
        private GameObject _ball;

        /// <summary>
        /// Called when the object is spawned
        /// </summary>
        void Start()
        {
            // Get the ball object
            // This is used for ai paddles only
            _ball = GameObject.FindGameObjectWithTag("Ball");
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        void Update()
        {
            if (!ComputerControlled)
            {
                // Move the paddle based on the player's inputs
                transform.position = Move(Time.deltaTime, transform.position, Input.GetAxis(AxisName));
            }
            else
            {
                // Get the y position of the ball
                var ballY = _ball.transform.position.y;

                // Create a movement axis based on the ball's y position
                var axis = 0.0f;

                // If the paddle is below the ball
                if (ballY > transform.position.y)
                {
                    // Set the axis to 'up'
                    axis = 1.0f;
                }

                // If the paddle is above the ball
                else if (ballY < transform.position.y)
                {
                    // Set the axis to 'down'
                    axis = -1.0f;
                }

                // Move the paddle and set the new position
                transform.position = Move(Time.deltaTime, transform.position, axis);
            }
        }

        /// <summary>
        /// This will move the paddle based on user input and the amount of time passed since last moving the paddle
        /// </summary>
        /// <param name="timePassed">The amount of time in seconds since the last update</param>
        /// <param name="currentPosition">The current position of the paddle</param>
        /// <param name="movementAxis">The axis of player inputs, 1 for up, -1 for down</param>
        /// <returns>The new calculated position</returns>
        Vector2 Move(float timePassed, Vector2 currentPosition, float movementAxis)
        {
            // Calculate y movement
            var movementAmount = Speed * timePassed * movementAxis;

            // Clamp the y position so that the paddle cannot leave the map
            var newY = Mathf.Clamp(movementAmount + currentPosition.y, -YMovementConstraint, YMovementConstraint);

            return new Vector2(currentPosition.x, newY);
        }
    }
}
