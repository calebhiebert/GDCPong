using UnityEngine;

/// <summary>
/// This is the Game Master
/// This script will control data that has to be managed over multiple scenes
/// </summary>
public class PongMaster : MonoBehaviour {

    private PlayerPrefs Player1;
    private PlayerPrefs Player2;

    /// <summary>
    /// A small struct to hold the data that was selected in the menu scene
    /// </summary>
    public struct PlayerPrefs
    {
        public GameObject SelectedPaddleSkin;
        public bool ComputerControlled;
    }

    /// <summary>
    /// Called When the game is first started up
    /// </summary>
	void Awake ()
    {
        // Make sure this wont get destroyed when the game scene is loaded
        DontDestroyOnLoad(gameObject);
	}

    void OnLevelWasLoaded(int level)
    {
        // If the game was loaded
        if(level == 1)
        {
            Debug.Log("Game Started!");
        }
    }

    // TODO finish this class
}
