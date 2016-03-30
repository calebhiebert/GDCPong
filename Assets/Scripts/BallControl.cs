using UnityEngine;

namespace Assets.Scripts
{
    public class BallControl : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _velocity;

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
                return Vector2.Reflect(_velocity, Vector2.down);
            } else
            {
                return Vector2.Reflect(_velocity, Vector2.left);
            }
        }
    }
}
