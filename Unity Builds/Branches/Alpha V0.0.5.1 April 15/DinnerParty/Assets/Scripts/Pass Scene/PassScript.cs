using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PassScript : MonoBehaviour
{
    public Text mPassText;

    private TurnManagerScript mTurnManagerScript;
    private RestaurantScript mRestaurantScript;

	void Start ()
    {
        mTurnManagerScript = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>();
        mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();

        List<Player> players = mRestaurantScript.getAlivePlayers();

        if (mTurnManagerScript.GetCurrentPlayerIndex() <= players.Count - 1)
        {
            ShowPlayerToPassTo();
        }
	}

    public void OnShowRoleClicked()
    {
        SceneManager.LoadScene(DinnerPartyScenes.SHOW_ROLE_PATH);
    }

    private void ShowPlayerToPassTo()
    {
        List<Player> players = mRestaurantScript.getAlivePlayers();
        Debug.Log("PLAYER COUNT: " + players.Count);
        //In the future the text could be flashing or something :D
        mPassText.text = "PASS TO " + players[mTurnManagerScript.GetCurrentPlayerIndex()].getName().ToUpper() + " SO THEY CAN SEE THEIR ROLE.";
    }
}
