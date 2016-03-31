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
        // The speed the ball will move (in unity-units per second)
        [SerializeField] private Vector2 _velocity;

        // The ball's velocity will be increased by this amount every time it hits a paddle
        [SerializeField] private float _speedIncrease;

        // The amount of curve to be added to the ball
        // Relative to its distance from the center of a paddle
        // In testing --Daniel: I'm not sure if this is what this object was intended to do but I used it aanyways--
        [SerializeField] private AnimationCurve _reflectCurve;

        //The speed of the ball when it is reset
        [SerializeField] private float _startSpeed;

        //The degree to which the player can affect the balls rebound
        //A value of 0 yeilds no control and a value of 1 is 180 degrees of control
        //Use if _reflectCurve is not working
        //[SerializeField] [Range(0,1)] private float _controlDamp;

        // the unity physics rigidbody for this ball
        private Rigidbody2D _rigidBody;

        //How fast the ball follows the sserving paddle
        [SerializeField] private float _serveFollowSmoothing = 7;

        // True while right paddle is serving
        private static bool _rightIsServing = false;

        // True while left paddle is serving
        private static bool _leftIsServing = false;

        //how far away the ball is from the serving paddle
        [SerializeField] private float _serveOffset;

        public float GoalBounds;

        /// <summary>
        /// Called when the object is spawned, or when the game is started
        /// </summary>
        void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            StartCoroutine(ResetBall(Random.Range(0,2) == 0));
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
            var serveDelay = 1.5f;

            if (Input.GetButtonDown("Reset")) 
            {
                //Kind of brutish but it works
                //I havent figured out how to stop a coroutine that has a parameter yet
                StopAllCoroutines();                
                StartCoroutine(ResetBall(Random.value < .5));
            }

            //Checks if ball is outside the scoring bounds
            if (transform.position.x < -GoalBounds || Mathf.Abs(transform.position.y) > 9) {
               Debug.Log("Player2 Scores!");
                StartCoroutine(ServeBall(leftPaddle, serveDelay));
                // TODO call score method from game master for player 2
            }
            else if (transform.position.x > GoalBounds || Mathf.Abs(transform.position.y) > 9) {
                Debug.Log("Player1 Scores!");
                StartCoroutine(ServeBall(rightPaddle, serveDelay));
                // TODO call score method from game master for player 1
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
        /// Resets the ball's position to the center of the game area
        /// and imparts a velocity of _startSpeed in a random direction toward one of the players
        /// </summary>
        /// <param name="direction">The side of the game area to send the ball to. True is left, False is Right</param>
        IEnumerator ResetBall(bool direction) {
            
            _velocity = Vector2.zero;
            transform.position = Vector2.zero;

            yield return new WaitForSeconds(1.5f);

            if (direction) {
                _velocity = MathUtils.AngleToVector(Random.Range(30, 151)) * _startSpeed;
            }
            else {
                _velocity = MathUtils.AngleToVector(Random.Range(210, 331)) * _startSpeed;
            }
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

                //Make ball follow paddle
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
