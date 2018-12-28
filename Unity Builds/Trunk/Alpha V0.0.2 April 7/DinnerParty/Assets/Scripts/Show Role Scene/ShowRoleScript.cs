using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowRoleScript : MonoBehaviour
{
    public Canvas mCanvas;

    public CanvasRenderer mRolePanel;
    public Text mUsernameText;
    public Text mRoleText;
    public Button mActionButton;

    public CanvasRenderer mActionPanel;
    public GameObject mTableCenter;
    private Button mUserButtonPrefab;

    //Todo: Generate these
    public Dictionary<Player, Button> mPlayerButtons;
    private List<Player> players;
    private TurnManagerScript turnManagerScript;
	private EnumPlayerRole mRoleForButtonPresses;

    private Player mPersonWithPoisonedMeal, mMarkedPlayer;

    // Use this for initialization
    void Start ()
    {
        turnManagerScript = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>();
        players = turnManagerScript.getPlayers();

        mUserButtonPrefab = Resources.Load<Button>("UserButton");

        mPlayerButtons = new Dictionary<Player, Button>();

        ShowUsername();
        //In the future we can add cool artwork here for each role and stuff.
        ShowRole();

		//PerformRoleAction();
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

    private void ShowRole()
    {
        EnumPlayerRole role = players[turnManagerScript.getCurrentPlayerIndex()].getRole();
        string roleWithPrefix = players[turnManagerScript.getCurrentPlayerIndex()].getRoleAsStringWithPrefix();
        mRoleText.text = "YOU ARE " + roleWithPrefix.ToUpper() + ".";

        switch (role)
        {
            case EnumPlayerRole.ASSASSIN:
                SetButtonAction("POISON FOOD", ShowPoisonScreen);
                break;

            case EnumPlayerRole.DISTANT_COUSIN:

                Player wealthyCouple1 = new Player("", EnumPlayerRole.PARTY_GOER);
                Player wealthyCouple2 = new Player("", EnumPlayerRole.PARTY_GOER);
                Player assassin = new Player("", EnumPlayerRole.PARTY_GOER);
                bool firstWealthyCoupleFound = false;

                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].getRole() == EnumPlayerRole.WEALTHY_COUPLE)
                    {
                        if (!firstWealthyCoupleFound)
                        {
                            wealthyCouple1 = players[i];
                            firstWealthyCoupleFound = true;
                        }
                        else
                            wealthyCouple2 = players[i];
                    }
                    else if (players[i].getRole() == EnumPlayerRole.ASSASSIN)
                    {
                        assassin = players[i];
                    }
                }

                mRoleText.text += "\n" + assassin.getName().ToUpper() + " IS THE ASSASSIN."
                    + "\nTHE WEALTHY COUPLE IS " + wealthyCouple1.getName().ToUpper()
                    + "\nAND " + wealthyCouple2.getName().ToUpper() + "."
                    + "\nCHOOSE WHO TO MARK.";


                SetButtonAction("MARK PLAYER", ShowMarkScreen);
                break;

            case EnumPlayerRole.WEALTHY_COUPLE:
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].getRole() == EnumPlayerRole.WEALTHY_COUPLE &&
                        players[i] != players[turnManagerScript.getCurrentPlayerIndex()])
                    {
                        mRoleText.text += "\nYOUR PARTNER IS " + (players[i].getName()).ToUpper() + ".";
                    }
                }

                SetButtonAction("GOT IT", OnConfirmButtonClicked);
                break;
            default:
                SetButtonAction("GOT IT", OnConfirmButtonClicked);
                break;
        }
    }

	private void PerformRoleAction()
	{
		EnumPlayerRole role = players[turnManagerScript.getCurrentPlayerIndex()].getRole();

		switch (role)
		{
			case EnumPlayerRole.ASSASSIN:
				ActivateButtons(players.Count);
				mRoleForButtonPresses = EnumPlayerRole.ASSASSIN;

				mRoleText.text	+= "\nCHOOSE WHOSE MEAL WILL START POISONED.";
				break;
			case EnumPlayerRole.DISTANT_COUSIN:
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
				break;
			case EnumPlayerRole.WEALTHY_COUPLE:
				for (int i = 0; i < players.Count; i++)
				{
					if (players [i].getRole () == EnumPlayerRole.WEALTHY_COUPLE &&
						players [i] != players [turnManagerScript.getCurrentPlayerIndex ()])
					{
						mRoleText.text += "\nYOUR PARTNER IS " + (players [i].getName ()).ToUpper() + ".";
					}
				}
				break;
		}
	}

	private void DeactivateButtons()
	{
        foreach (KeyValuePair<Player, Button> entry in mPlayerButtons)
        {
            entry.Value.gameObject.SetActive(false);
        }
	}

	private void ActivateButtons(int fields)
	{
		DeactivateButtons();

		int i = 0;
        foreach (KeyValuePair<Player, Button> entry in mPlayerButtons)
        {
            if (i < fields)
            {
                entry.Value.gameObject.SetActive(true);
                entry.Value.GetComponentInChildren<Text>().text = players[i].getName();
            }
            else
            {
                break;
            }

            ++i;
        }
	}

	public void OnUserSelected(Button pressButton)
	{
        List<Player> players = turnManagerScript.getPlayers();
        foreach (KeyValuePair<Player, Button> entry in mPlayerButtons)
        {
            if (pressButton == entry.Value)
            {
                switch (players[turnManagerScript.getCurrentPlayerIndex()].getRole())
                {
                    case EnumPlayerRole.ASSASSIN:
                        turnManagerScript.setPoisonPlayer(entry.Key);
                        Debug.Log("POISONED: " + entry.Key.getName());
                        break;
                    case EnumPlayerRole.DISTANT_COUSIN:
                        turnManagerScript.setMarkedPlayer(entry.Key);
                        Debug.Log("MARKED: " + entry.Key.getName());
                        break;
                }
            }
        }
	}

    private void SetButtonAction(string buttonText, UnityEngine.Events.UnityAction actionName)
    {
        mActionButton.transform.GetChild(0).GetComponent<Text>().text = buttonText;
        mActionButton.onClick.RemoveAllListeners();
        mActionButton.onClick.AddListener(actionName);
    }

    public void ShowPoisonScreen()
    {
        mRolePanel.gameObject.SetActive(false);
        mActionPanel.gameObject.SetActive(true);

        PlaceButtonsInCircle();

        SetButtonAction("CONFIRM", OnConfirmButtonClicked);
    }

    public void ShowMarkScreen()
    {
        mRolePanel.gameObject.SetActive(false);
        mActionPanel.gameObject.SetActive(true);

        PlaceButtonsInCircle();

        SetButtonAction("CONFIRM", OnConfirmButtonClicked);
    }

    public void OnActionConfirmClicked()
    {
        //Save who the player marked.

        mActionPanel.gameObject.SetActive(false);
        mRolePanel.gameObject.SetActive(true);
    }

    private void PlaceButtonsInCircle()
    {
        List<Player> players = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>().getPlayers();
        float playerCount = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>().getPlayerCount();
        float distanceBetweenAngle = 360.0f / playerCount;

        //Have the current player be at the bottom so it's closest to the user.
        float currentAngle = 270.0f;

        //Scale radius by screen size to keep it consistent.
        float radius = mCanvas.pixelRect.width / 5.0f;

        int i;
        for (i = 0; i < playerCount; ++i)
        {
            Button userButton = Instantiate(mUserButtonPrefab, mTableCenter.transform);
            userButton.onClick.AddListener(delegate { OnUserSelected(userButton); });
            userButton.transform.GetChild(0).GetComponent<Text>().text = players[i].getName();

            Vector3 pos = mTableCenter.transform.position;

            pos.x += radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
            pos.y += radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);

            userButton.transform.position = pos;

            Vector3 rot = userButton.transform.eulerAngles;

            if ((currentAngle > 270 && currentAngle < 360) || (currentAngle < 90 && currentAngle > 0))
            {
                rot.z = currentAngle;
            }
            else if (currentAngle > 90 && currentAngle < 270)
            {
                rot.z = currentAngle - 180;
            }

            userButton.transform.eulerAngles = rot;

            currentAngle = ((currentAngle + distanceBetweenAngle) % 360);

            mPlayerButtons.Add(players[i], userButton);
        }
    }
}
