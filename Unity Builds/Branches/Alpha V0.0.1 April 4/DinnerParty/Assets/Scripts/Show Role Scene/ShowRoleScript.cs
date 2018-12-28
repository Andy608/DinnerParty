using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowRoleScript : MonoBehaviour
{
    public Text mUsernameText;
    public Text mRoleText;
	public List<Button> mPlayerButtons;
    private List<Player> players;
    private TurnManagerScript turnManagerScript;
	private EnumPlayerRole mRoleForButtonPresses;
	private Player mMarkedPlayer, mPersonWithPoisonedMeal;

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
		DeactivateButtons ();

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
			{
				ActivateButtons (players.Count);
				mRoleForButtonPresses = EnumPlayerRole.ASSASSIN;

				mRoleText.text	+= "\nCHOOSE WHOSE MEAL WILL START POISONED.";
			}
				break;
			case EnumPlayerRole.DISTANT_COUSIN:
			{
				ActivateButtons (players.Count);
				mRoleForButtonPresses = EnumPlayerRole.DISTANT_COUSIN;
					
				Player wealthyCouple1 = new Player("", EnumPlayerRole.PARTY_GOER);
				Player wealthyCouple2 = new Player("", EnumPlayerRole.PARTY_GOER);
				Player assassin = new Player("", EnumPlayerRole.PARTY_GOER);
				bool firstWealthyCoupleFound = false;

				for (int i = 0; i < players.Count; i++)
				{
					if (players [i].getRole () == EnumPlayerRole.WEALTHY_COUPLE) 
					{
						if (!firstWealthyCoupleFound)
						{
							wealthyCouple1 = players [i];
							firstWealthyCoupleFound = true;
						}
						else
							wealthyCouple2 = players [i];
					}
					else if (players [i].getRole () == EnumPlayerRole.ASSASSIN) 
					{
						assassin = players [i];
					}
				}

				mRoleText.text += "\n" + assassin.getName().ToUpper() + " IS THE ASSASSIN."
					+ "\nTHE WEALTHY COUPLE IS " + wealthyCouple1.getName().ToUpper()
					+ "\nAND " + wealthyCouple2.getName().ToUpper() + "."
					+ "\nCHOOSE WHO TO MARK.";
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

	private void DeactivateButtons()
	{
		int i;
		for (i = 0; i < mPlayerButtons.Count; ++i)
		{
			mPlayerButtons[i].gameObject.SetActive(false);
		}
	}

	private void ActivateButtons(int fields)
	{
		DeactivateButtons();

		int i;
		for (i = 0; i < mPlayerButtons.Count && i < fields; i++)
		{
			mPlayerButtons[i].gameObject.SetActive(true);
			mPlayerButtons [i].GetComponentInChildren<Text> ().text = players [i].getName ();
		}
	}

	public void ChooseThisSpot(Button pressButton)
	{
		for (int i = 0; i < players.Count; i++)
		{
			if (pressButton == mPlayerButtons [i])
			{
				switch (mRoleForButtonPresses)
				{
				case EnumPlayerRole.ASSASSIN:
					mPersonWithPoisonedMeal = players [i];
					break;
				case EnumPlayerRole.DISTANT_COUSIN:
					mMarkedPlayer = players [i];
					break;
				}
			}
		}
	}
}
