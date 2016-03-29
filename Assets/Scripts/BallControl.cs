using UnityEngine;

namespace Assets.Scripts
{
    public class BallControl : MonoBehaviour
    {
        public float Speed;

        private Vector2 _velocity;

        void Start () {
	
        }
	
        void Update () {
	
        }

        Vector2 StepPosition(float timePassed)
        {
            //TODO impliment stepping

            return Vector2.zero;
        }

        /// <summary>
        /// Steps the position n amount of step
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="timePerStep"></param>
        /// <returns></returns>
        Vector2[] StepPosition(int steps, float timePerStep)
        {
            var positions = new Vector2[steps];

            for (int i = 0; i < steps; i++)
            {
                positions[i] = StepPosition(timePerStep);
            }

            return positions;
        }
    }
}
