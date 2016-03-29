using UnityEngine;
using System.Collections;
using BoxCollider = Assets.Scripts.Physics.BoxCollider;

public class BCTest : MonoBehaviour
{

    private BoxCollider _collider;

	// Use this for initialization
	void Start ()
	{
	    _collider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var point = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

	    if (_collider.ContainsPoint(point))
	    {
	        Debug.Log("WOLOLOLOL");
	    }
	}
}
