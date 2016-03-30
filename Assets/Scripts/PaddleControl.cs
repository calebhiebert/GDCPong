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
        // how fast the paddle will move
        public float Speed;

        // how far the paddle can travel
        public float YMovementConstraint;

        // the name of the axis to get input from
        // for w and s, use "Player1"
        // for up and down arrows, use "Player2"
        public string AxisName = "Player1";

        // should the ai be used
        public bool ComputerControlled;

        // the ball, used if this paddle is being computer controlled
        private GameObject _ball;

        /// <summary>
        /// Called when the object is spawned
        /// </summary>
        void Start()
        {
            // get the ball object
            // this is used for ai paddles only
            _ball = GameObject.FindGameObjectWithTag("Ball");
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        void Update()
        {
            if (!ComputerControlled)
            {
                // move the paddle based on the player's inputs
                transform.position = Move(Time.deltaTime, transform.position, Input.GetAxis(AxisName));
            }
            else
            {
                // get the y position of the ball
                var ballY = _ball.transform.position.y;

                // create a movement axis based on the ball's y position
                var axis = 0.0f;

                // if the paddle is below the ball
                if (ballY > transform.position.y)
                {
                    // set the axis to 'up'
                    axis = 1.0f;
                }

                // if the paddle is above the ball
                else if (ballY < transform.position.y)
                {
                    // set the axis to 'down'
                    axis = -1.0f;
                }

                // Move the paddle and set the new position
                transform.position = Move(Time.deltaTime, transform.position, axis);
            }
        }

        /// <summary>
        /// This will move the paddle
        /// </summary>
        /// <param name="timePassed">The amount of time in seconds since the last update</param>
        /// <param name="currentPosition">The current position of the paddle</param>
        /// <param name="movementAxis">The axis of player inputs, 1 for up, -1 for down</param>
        /// <returns>The new calculated position</returns>
        Vector2 Move(float timePassed, Vector2 currentPosition, float movementAxis)
        {
            // calculate y movement
            var movementAmount = Speed * timePassed * movementAxis;

            // clamp the y position so that the paddle cannot leave the map
            var newY = Mathf.Clamp(movementAmount + currentPosition.y, -YMovementConstraint, YMovementConstraint);

            return new Vector2(currentPosition.x, newY);
        }
    }
}
