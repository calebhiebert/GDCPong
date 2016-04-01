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
        
        // How accurate the ai will be when moving the paddle
        public float AiMovementErrorMargin;

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
                var ballPredict = PredictPosition();

                var movementAxis = 0f;

                if (ballPredict.y > transform.position.y + AiMovementErrorMargin)
                    movementAxis = 1;

                if (ballPredict.y < transform.position.y - AiMovementErrorMargin)
                    movementAxis = -1;

                transform.position = Move(Time.deltaTime, transform.position, movementAxis);
            }
        }

        /// <summary>
        /// This method will predict where the paddle has to be by the time the ball will reach it
        /// </summary>
        /// <returns>The position to try and move to</returns>
        Vector2 PredictPosition()
        {
            // TODO replace with a more performant solution
            var bc = _ball.GetComponent<BallControl>();

            var predictionData = new BallControl.BallPredictionData { newVelocity = _ball.GetComponent<Rigidbody2D>().velocity, collisionPoint = _ball.transform.position };

            // Calculate 3 ball bounces in the future
            for (int i = 0; i < 3; i++)
            {
                predictionData = bc.PredictNextCollisionPoint(predictionData.collisionPoint, predictionData.newVelocity, 7);

                if (predictionData.willCollideWith == null)
                {
                    break;
                } else if (predictionData.willCollideWith == gameObject)
                {
                    break;
                }
            }

            return predictionData.collisionPoint;
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
