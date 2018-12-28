using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowRoleScript : MonoBehaviour
{
    public Text mUsernameText;
    public Text mRoleText;
    private List<Player> players;
    private TurnManagerScript turnManagerScript;

    // Use this for initialization
    void Start ()
    {
        turnManagerScript = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>();
        players = turnManagerScript.getPlayers();

        ShowUsername();
        //In the future we can add cool artwork here for each role and stuff.
        ShowRoleText();

		PerformRoleAction ();
    }

    public void OnConfirmButtonClicked()
    {
        if (turnManagerScript.getCurrentPlayerIndex() < players.Count - 1)
        {
            turnManagerScript.goToNextPlayer();
            SceneManager.LoadScene(DinnerPartyScenes.PASS_PATH);
        }
        else
        {
            SceneManager.LoadScene(DinnerPartyScenes.START_GAME_PATH);
        }
    }

    private void ShowUsername()
    {
        mUsernameText.text = players[turnManagerScript.getCurrentPlayerIndex()].getName();
    }

    private void ShowRoleText()
    {
        string roleWithPrefix = players[turnManagerScript.getCurrentPlayerIndex()].getRoleAsStringWithPrefix();
        mRoleText.text = "YOU ARE " + roleWithPrefix.ToUpper() + ".";
    }

	private void PerformRoleAction()
	{
		EnumPlayerRole role = players [turnManagerScript.getCurrentPlayerIndex ()].getRole ();
		switch (role)
		{
			case EnumPlayerRole.ASSASSIN:
				
				break;
			case EnumPlayerRole.DISTANT_COUSIN:
			{
				
			}
				break;
			case EnumPlayerRole.WEALTHY_COUPLE:
			{
				for (int i = 0; i < players.Count; i++)
				{
					if (players [i].getRole () == EnumPlayerRole.WEALTHY_COUPLE &&
						players [i] != players [turnManagerScript.getCurrentPlayerIndex ()])
					{
						mRoleText.text += "\nYOUR PARTNER IS " + (players [i].getName ()).ToUpper() + ".";
					}
				}
			}
				break;
		}
	}
}
