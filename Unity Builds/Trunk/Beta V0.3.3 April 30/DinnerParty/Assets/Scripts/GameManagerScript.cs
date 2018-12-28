using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private static GameObject gameManagerPrefab;
    private static GameObject gameManager;

    public static GameManagerScript GetInstance()
    {
        if (gameManager == null && (gameManager = GameObject.Find("GameManager")) == null)
        {
            gameManagerPrefab = Resources.Load<GameObject>("GameManager");
            gameManager = Instantiate(gameManagerPrefab) as GameObject;

            //If the game is started from any scene that is not the title scene, go to title scene.
            if (SceneManager.GetActiveScene().name != DinnerPartyScenes.TITLE_SCENE_NAME)
            {
                SceneManager.LoadScene(DinnerPartyScenes.TITLE_PATH);
            }
        }

        return gameManager.GetComponent<GameManagerScript>();
    }

    void Start ()
    {
		if (gameManager == null && (gameManager = GameObject.Find ("GameManager")) == null) {
			gameManager = GetInstance ().gameObject;

			//If the game is started from any scene that is not the title scene, go to title scene.
			/*if (SceneManager.GetActiveScene().name != DinnerPartyScenes.TITLE_SCENE_NAME)
            {
                SceneManager.LoadScene(DinnerPartyScenes.TITLE_PATH);
            }*/
		} else {
			//if (gameManager.GetComponent<AudioSource> ().isPlaying) {
				//gameManager.GetComponent<AudioSource> ().Stop ();
			//}
		}

        DontDestroyOnLoad(this);
    }
}
