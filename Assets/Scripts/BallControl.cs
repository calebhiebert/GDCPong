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
        private bool _rightIsServing = false;
        private bool _leftIsServing = false;
        [SerializeField] private float _serveFollowSmoothing = 7;

        /// <summary>
        /// The different directions the ball can go in at the beginning of the game
        /// </summary>
        public enum Direction
        {
            Left, Right
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
            Transform rightPaddle = GameObject.Find("PaddleR").transform;
            Transform leftPaddle = GameObject.Find("PaddleL").transform;

            if (Input.GetButtonDown("Reset")) 
            {
                //Kind of brutish but it works
                //I havent figured out how to stop a coroutine that has a parameter yet
                StopAllCoroutines();
                
                // TODO get this working randomly          
                StartCoroutine(ResetBall((Random.value < .5)? Direction.Left : Direction.Right));
            }
            if (transform.position.x < -GoalBounds || Mathf.Abs(transform.position.y) > 9) {
                Debug.Log("Player2 Scores!");
                StartCoroutine(ResetBall(Direction.Right));

                // Call the score event for any subscribed classes
                if (OnScore != null)
                    OnScore(Direction.Left);
            }
            else if (transform.position.x > GoalBounds || Mathf.Abs(transform.position.y) > 9) {
                Debug.Log("Player1 Scores!");
                StartCoroutine(ResetBall(Direction.Left));

                // Call the score event for any subscribed classes
                if (OnScore != null)
                    OnScore(Direction.Right);
            }
            else if (_rightIsServing) {
                //make the ball follow the right paddle
                transform.position = new Vector2(transform.position.x, Mathf.Lerp(transform.position.y, rightPaddle.position.y, _serveFollowSmoothing * Time.deltaTime));
            }
            else if (_leftIsServing) {
                transform.position = new Vector2(transform.position.x, Mathf.Lerp(transform.position.y, leftPaddle.position.y, _serveFollowSmoothing * Time.deltaTime));
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
            
            if(bounceObject.tag == "Paddle")
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
        /// Coroutine that starts and controls serve duration
        /// </summary>
        /// <param name="paddle">Transform of serving paddle</param>
        /// <returns></returns>
        IEnumerator ServeBall(Transform paddle, float delay) {
            _velocity = Vector2.zero;

            if (paddle.name.Contains("R")) {
                //Move ball in front of paddle
                transform.position = new Vector2(paddle.position.x - _serveOffset, paddle.position.y);

                //Make ball start following paddle
                _rightIsServing = true;

                //Wait for delay
                yield return new WaitForSeconds(delay);

                //Make ball stop following paddle
                _rightIsServing = false;

                //Give ball starting velocity toward paddle
                _velocity = Vector2.right * _startSpeed;
            }
            else {
                transform.position = new Vector2(paddle.position.x + _serveOffset, paddle.position.y);
                _leftIsServing = true;

                yield return new WaitForSeconds(delay);

                _leftIsServing = false;
                _velocity = Vector2.left * _startSpeed;
            }
        }
    }
}
