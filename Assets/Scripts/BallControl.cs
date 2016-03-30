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
        // This is not yet functional
        [SerializeField] private AnimationCurve _reflectCurve;

        // the unity physics rigidbody for this ball
        private Rigidbody2D _rigidBody;

        /// <summary>
        /// Called when the object is spawned, or when the game is started
        /// </summary>
        void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }
	
        /// <summary>
        /// Called once per physics-frame
        /// </summary>
        void FixedUpdate ()
        {
            _rigidBody.velocity = _velocity;
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
            // TODO make the reflect angle dynamic instead of hard coded

            if(bounceObject.tag == "Paddle")
            {
                return Vector2.Reflect(_velocity, Vector2.down);
            } else
            {
                return Vector2.Reflect(_velocity, Vector2.left);
            }
        }
    }
}
