using UnityEngine;
using System.Collections;

public class PaddleControl : MonoBehaviour {

    // how fast the paddle will move
    public float Speed;

    // how far the paddle can travel
    public float YMovementConstraint;

    // the name of the axis to get input from
    // for w and s, use "Player1"
    // for up and down arrows, use "Player2"
    public string AxisName = "Player1";
	
	// Update is called once per frame
	void Update () {
        transform.position = Move(Time.deltaTime, transform.position, Input.GetAxis(AxisName));
	}

    /// <summary>
    /// This will move the paddle
    /// </summary>
    /// <param name="timePassed">The amount of time in seconds since the last update</param>
    /// <param name="currentPosition">The current position of the paddle</param>
    /// <param name="up">If the user is pressing the up key</param>
    /// <param name="down">If the user is pressing the down key</param>
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
