using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowRoleScript : MonoBehaviour
{
    public Canvas mCanvas;

    //Role Panel
    public CanvasRenderer mRolePanel;
    public Text mUsernameText;
    public Text mRoleText;
    public Button mActionButton;

    //Action Panel
    public CanvasRenderer mActionPanel;
    public GameObject mTableCenter;
    public Button mDoneWithActionButton;
    private Button mUserButtonPrefab;

    //private List<Player> players;

    private List<Button> mPlayerNamecards;
    private TurnManagerScript mTurnManagerScript;
    private RestaurantScript mRestaurantScript;

    // Use this for initialization
    void Start ()
    {
        mTurnManagerScript = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>();
        mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();
        //players = GameManagerScript.sRestaurant.getAllPlayers();

        mUserButtonPrefab = Resources.Load<Button>("UserButton");

        mPlayerNamecards = new List<Button>();

        ShowUsername();
        //In the future we can add cool artwork here for each role and stuff.
        ShowRole();

		//PerformRoleAction();
    }

    public void OnConfirmButtonClicked()
    {
		DeactivateButtons();

        if (mTurnManagerScript.GoToNextPlayer())
        {
            SceneManager.LoadScene(DinnerPartyScenes.PASS_PATH);
        }
        else
        {
            SceneManager.LoadScene(DinnerPartyScenes.START_GAME_PATH);
        }
    }

    private void ShowUsername()
    {
        List<Player> players = mRestaurantScript.getAlivePlayers();
        Debug.Log("PLAYER COUNT: " + players.Count);

        mUsernameText.text = players[mTurnManagerScript.GetCurrentPlayerIndex()].getName();
    }

    private void ShowRole()
    {
        List<Player> players = mRestaurantScript.getAlivePlayers();

        EnumPlayerRole role = players[mTurnManagerScript.GetCurrentPlayerIndex()].getRole();
        string roleWithPrefix = players[mTurnManagerScript.GetCurrentPlayerIndex()].getRoleAsStringWithPrefix();
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
                        players[i] != players[mTurnManagerScript.GetCurrentPlayerIndex()])
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

	private void DeactivateButtons()
	{
        int i;
        for (i = 0; i < mPlayerNamecards.Count; ++i)
        {
            mPlayerNamecards[i].gameObject.SetActive(false);
        }
	}

	private void ActivateButtons(int fields)
	{
        List<Player> players = mRestaurantScript.getAlivePlayers();

        DeactivateButtons();

		int i = 0;
        for (i = 0; i < mPlayerNamecards.Count && i < fields; ++i)
        {
            mPlayerNamecards[i].gameObject.SetActive(true);
            mPlayerNamecards[i].GetComponentInChildren<Text>().text = players[i].getName();
            ++i;
        }
	}

	public void OnUserSelected(Button pressButton)
	{
        List<Player> players = mRestaurantScript.getAlivePlayers();

        Debug.Log("PLAYER COUNT: " + players.Count + " | BUTTON COUNT: " + mPlayerNamecards.Count);

        int i;
        for (i = 0; i < mPlayerNamecards.Count; ++i)
        {
            mPlayerNamecards[i].image.color = Color.white;
            mRestaurantScript.SetPoisonedMealAtIndex(i, false);

            Debug.Log("HELLO");
            if (pressButton == mPlayerNamecards[i])
            {
                Debug.Log("HELLO1: " + mTurnManagerScript.GetCurrentPlayerIndex());
                Debug.Log("PLAYER COUNT: " + players.Count + " | BUTTON COUNT: " + mPlayerNamecards.Count);
                switch (players[mTurnManagerScript.GetCurrentPlayerIndex()].getRole())
                {
                    case EnumPlayerRole.ASSASSIN:
                        mRestaurantScript.SetPoisonedMealAtIndex(i, true);
                        mPlayerNamecards[i].image.color = Color.red;
                        Debug.Log("POISONED: " + players[i].getName());
                        Debug.Log("HELLO2");
                        break;
                    case EnumPlayerRole.DISTANT_COUSIN:
                        mRestaurantScript.MarkPlayerAtIndex(i);
                        mPlayerNamecards[i].image.color = Color.blue;
                        Debug.Log("MARKED: " + players[i].getName());
                        break;
                }
            }
        }

        mDoneWithActionButton.transform.GetChild(0).GetComponent<Text>().text = "DONE";
        mDoneWithActionButton.interactable = true;
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
        mDoneWithActionButton.interactable = false;

        PlaceButtonsInCircle(EnumPlayerRole.ASSASSIN);
        mDoneWithActionButton.transform.GetChild(0).GetComponent<Text>().text = "POISON A MEAL FIRST";

        SetButtonAction("CONFIRM", OnConfirmButtonClicked);
    }

    public void ShowMarkScreen()
    {
        mRolePanel.gameObject.SetActive(false);
        mActionPanel.gameObject.SetActive(true);
        mDoneWithActionButton.interactable = false;

        PlaceButtonsInCircle(EnumPlayerRole.DISTANT_COUSIN);
        mDoneWithActionButton.transform.GetChild(0).GetComponent<Text>().text = "MARK A PLAYER FIRST";

        SetButtonAction("CONFIRM", OnConfirmButtonClicked);
    }

    public void OnActionConfirmClicked()
    {
        mActionPanel.gameObject.SetActive(false);
        mRolePanel.gameObject.SetActive(true);
    }

    private void PlaceButtonsInCircle(EnumPlayerRole userRole)
    {
        //mPlayerNamecards.Clear();

        List<Player> players = mRestaurantScript.getAlivePlayers();
        float distanceBetweenAngle = 360.0f / players.Count;

        //Have the current player be at the bottom so it's closest to the user.
        float currentAngle = 270.0f;

        //Scale radius by screen size to keep it consistent.
        float radius = mCanvas.pixelRect.width / 5.0f;

        int i;
        for (i = 0; i < players.Count; ++i)
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

            if (players[i].getRole().Equals(userRole))
            {
                userButton.interactable = false;
            }

            mPlayerNamecards.Add(userButton);
        }
    }
}
