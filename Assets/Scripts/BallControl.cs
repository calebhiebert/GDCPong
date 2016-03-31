using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// This script goes on the ball GameObject
    /// It handles moving of the ball
    /// It also handles ball collision and bounce angles
    /// </summary>
    public class BallControl : MonoBehaviour
    {
        // A delegate that will be called when the ball goes out of bounds and is reset
        public delegate void BallOutOfBoundsEvent(Direction scoreDirection);

        // An event that will be called when a player scores
        public event BallOutOfBoundsEvent OnScore;

        // The speed the ball will move (in unity-units per second)
        [SerializeField] private Vector2 _velocity;

        // The ball's velocity will be increased by this amount every time it hits a paddle
        [SerializeField] private float _speedIncrease;

        // The amount of curve to be added to the ball
        // Relative to its distance from the center of a paddle
        // In testing --Daniel: I'm not sure if this is what this object was intended to do but I used it aanyways--
        [SerializeField] private AnimationCurve _reflectCurve;

        // The speed of the ball when it is reset
        [SerializeField] private float _startSpeed;

        // How long to wait after a point is scored before moving the ball
        [SerializeField] private float _ballStartTimer;

        //The degree to which the player can affect the balls rebound
        //A value of 0 yeilds no control and a value of 1 is 180 degrees of control
        //Use if _reflectCurve is not working
        //[SerializeField] [Range(0,1)] private float _controlDamp;

        // the unity physics rigidbody for this ball
        private Rigidbody2D _rigidBody;

        public float GoalBounds;

        [SerializeField] private float _serveOffset = 1;

        [SerializeField] private float _serveFollowSmoothing = 7;

        /// <summary>
        /// The different directions the ball can go in at the beginning of the game
        /// </summary>
        public enum Direction
        {
            Left, Right
        }

        public enum Paddle
        {
            PaddleL, PaddleR
        }

        /// <summary>
        /// Called when the object is spawned, or when the game is started
        /// </summary>
        void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            StartCoroutine(ResetBall(Direction.Left));
        }
	
        /// <summary>
        /// Called once per physics-frame
        /// </summary>
        void FixedUpdate ()
        {
            _rigidBody.velocity = _velocity;
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        void Update() {

            if (Input.GetButtonDown("Reset")) 
            {
                //Kind of brutish but it works
                //I havent figured out how to stop a coroutine that has a parameter yet
                StopAllCoroutines();
                         
                StartCoroutine(ResetBall((Random.value < .5)? Direction.Left : Direction.Right));
            }
            if (transform.position.x < -GoalBounds || Mathf.Abs(transform.position.y) > 9) {
                Debug.Log("Player2 Scores!");
                StartCoroutine(AttachToPaddle(Paddle.PaddleL, 1.5f, _serveFollowSmoothing));

                // Call the score event for any subscribed classes
                if (OnScore != null)
                    OnScore(Direction.Left);
            }
            else if (transform.position.x > GoalBounds || Mathf.Abs(transform.position.y) > 9) {
                Debug.Log("Player1 Scores!");
                StartCoroutine(AttachToPaddle(Paddle.PaddleR, 1.5f, _serveFollowSmoothing));

                // Call the score event for any subscribed classes
                if (OnScore != null)
                    OnScore(Direction.Right);
            }
        }

        /// <summary>
        /// Called when the ball collides with something
        /// </summary>
        /// <param name="other">The object that the ball collided with</param>
        void OnCollisionEnter2D(Collision2D other)
        {
            _velocity = ReflectVelocity(other.gameObject) * -1;

            _velocity *= _speedIncrease + 1;
        }

        /// <summary>
        /// Reflects the ball off of a given surface
        /// </summary>
        /// <param name="bounceObject">The object that is being bounced off of</param>
        /// <returns>The new velocity angle</returns>
        Vector2 ReflectVelocity(GameObject bounceObject)
        {
            
            if(bounceObject.tag.Contains("Paddle"))
            {
                //Find distance from center of paddle
                float controlAngle = Mathf.Clamp(transform.position.y - bounceObject.transform.position.y, -1f, 1f);

                Debug.Log("Curved\t" + controlAngle * _reflectCurve.Evaluate(Mathf.Abs(controlAngle)));
                Debug.Log("Uncurved\t" + controlAngle);

                //could potentially cause some weird behaviour if ball is past the paddle
                if (bounceObject.transform.position.x <= transform.position.x) 
                {
                    //use if reflectCurve isn't working
                    //Vector2 reflectAngle = new Vector2(controlAngle * _controlDamp, -1).normalized;

                    Vector2 reflectAngle = new Vector2(controlAngle * _reflectCurve.Evaluate(Mathf.Abs(controlAngle)), -1).normalized;
                    return Vector2.Reflect(_velocity, reflectAngle);
                }
                else {
                    //use if reflectCurve isn't working
                    //Vector2 reflectAngle = new Vector2(-controlAngle * _controlDamp, -1).normalized;

                    Vector2 reflectAngle = new Vector2(-controlAngle * _reflectCurve.Evaluate(Mathf.Abs(controlAngle)), -1).normalized;
                    return Vector2.Reflect(_velocity, reflectAngle);
                }
                
            } else
            {
                return Vector2.Reflect(_velocity, Vector2.left);
            }
        }

        /// <summary>
        /// Returns a random velocity pointing in a given direction
        /// </summary>
        /// <param name="direction">The direction to point the velocity in</param>
        /// <returns>A random velocity pointing in a certain direction</returns>
        private Vector2 GetRandomVelocity(Direction direction)
        {
            var randVelocity = Vector2.zero;

            if(direction == Direction.Left)
            {
                randVelocity = MathUtils.AngleToVector(Random.Range(30, 151)) * _startSpeed;
            }
            
            else if (direction == Direction.Right)
            {
                randVelocity = MathUtils.AngleToVector(Random.Range(210, 331)) * _startSpeed;
            }

            return randVelocity;
        }

        /// <summary>
        /// Resets the ball's position to the center of the game area
        /// and imparts a velocity of _startSpeed in a random direction toward one of the players
        /// </summary>
        /// <param name="direction">The side of the game area to send the ball to.</param>
        IEnumerator ResetBall(Direction direction) {
            
            // Reset the ball's position and velocity
            _velocity = Vector2.zero;
            transform.position = Vector2.zero;

            float timer = _ballStartTimer;

            // Loop while updating a timer until the timer runs out
            while(timer > 0)
            {
                timer -= Time.deltaTime;

                // TODO update on screen timer with timer variable
                Debug.Log("Time remaining: " + timer);

                // wait until the next frame to run again
                yield return null;
            }

            _velocity = GetRandomVelocity(direction);
        }

        /// <summary>
        /// Will temporarily attach the ball to a given paddle
        /// </summary>
        /// <param name="paddle">The paddle to attach to</param>
        /// <param name="attachDuration">How long the ball will stay attached to the paddle</param>
        /// <param name="smoothing">How smooth</param>
        /// <returns>Coroutine stuff</returns>
        IEnumerator AttachToPaddle(Paddle paddle, float attachDuration, float smoothing) {

            _velocity = Vector2.zero;

            var selectedPaddle = GameObject.FindGameObjectWithTag(paddle.ToString());

            var offset = 0f;

            if (paddle == Paddle.PaddleR)
                offset = -_serveOffset;

            else if (paddle == Paddle.PaddleL)
                offset = _serveOffset;

            transform.position = new Vector2(selectedPaddle.transform.position.x + offset, selectedPaddle.transform.position.y);

            var finishVelocity = (paddle == Paddle.PaddleR ? Vector2.right : Vector2.left) * _startSpeed;

            while (attachDuration > 0)
            {
                attachDuration -= Time.deltaTime;

                transform.position = new Vector2(transform.position.x, Mathf.Lerp(transform.position.y, selectedPaddle.transform.position.y, smoothing * Time.deltaTime));

                yield return null;
            }


            _velocity = finishVelocity;
        }
    }
}
