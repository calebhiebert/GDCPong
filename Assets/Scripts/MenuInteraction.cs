using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuInteraction : MonoBehaviour {

    public PongMaster GameMaster;

    /// <summary>
    /// Changes to the game scene
    /// The PongMaster script will handle setting skins and ai paddles
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // TODO write this class
}
