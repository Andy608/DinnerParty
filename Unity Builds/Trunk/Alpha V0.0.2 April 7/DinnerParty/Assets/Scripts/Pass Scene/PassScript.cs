using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PassScript : MonoBehaviour
{
    public Text mPassText;
    private List<Player> players;
    private TurnManagerScript turnManagerScript;

	void Start ()
    {
        turnManagerScript = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>();
        players = turnManagerScript.getPlayers();

        if (turnManagerScript.getCurrentPlayerIndex() <= players.Count - 1)
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
        //In the future the text could be flashing or something :D
        mPassText.text = "PASS TO " + players[turnManagerScript.getCurrentPlayerIndex()].getName().ToUpper() + " SO THEY CAN SEE THEIR ROLE.";
    }
}
