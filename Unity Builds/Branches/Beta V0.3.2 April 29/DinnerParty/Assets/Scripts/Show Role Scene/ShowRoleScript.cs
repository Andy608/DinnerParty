using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowRoleScript : MonoBehaviour
{
    public Canvas mCanvas;

    //Character art
	public GameObject mCharacterObject;
	public Sprite mAssassinSprite;
	public Sprite mDistantCousinSprite;
	public Sprite mWealthyCouple1Sprite;
	public Sprite mWealthyCouple2Sprite;
	public Sprite mChemistSprite;
	public Sprite mFoodCriticSprite;
	public Sprite mScapegoatSprite;
	public Sprite mPrivateEyeSprite;
	public Sprite mPartygoerSprite;

	//Role Panel
    public CanvasRenderer mRolePanel;
    public Text mUsernameText;
    public Text mRoleText;
    public Button mActionButton;

	//Start Round Panel
	public CanvasRenderer mStartRoundPanel;
	public Text mCourseTitle;

    //Action Panel
    public CanvasRenderer mActionPanel;
    public GameObject mTableCenter;
    public Button mDoneWithActionButton;
    private Button mUserButtonPrefab;

	public GameObject mLazySusan;

    private List<Button> mPlayerNamecards;
    private TurnManagerScript mTurnManagerScript;
    private RestaurantScript mRestaurantScript;

	public static bool showRolePanel = false;

	private int mClickTracker = 3;
	private Vector3 mActionButtonLocation;

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

		mDoneWithActionButton.onClick.AddListener (OnConfirmButtonClicked);

		mCourseTitle.text = mTurnManagerScript.mCurrentRound.ToString() + " ROUND";

		if (!showRolePanel) {
			mStartRoundPanel.gameObject.SetActive (true);
			mRolePanel.gameObject.SetActive (false);
			mActionPanel.gameObject.SetActive (false);
			mLazySusan.SetActive (false);
		} else {
			mStartRoundPanel.gameObject.SetActive (false);
			mRolePanel.gameObject.SetActive (true);
			mActionPanel.gameObject.SetActive (false);
			mLazySusan.SetActive (false);
		}

		mActionButtonLocation = mActionButton.transform.position;
		//PerformRoleAction();
    }

	public void OnStartRoundClicked()
	{
		SceneManager.LoadScene(DinnerPartyScenes.PASS_PATH);
		showRolePanel = true;
		//mStartRoundPanel.gameObject.SetActive (false);
		//mRolePanel.gameObject.SetActive (true);
		//mActionPanel.gameObject.SetActive (false);
	}

    public void OnConfirmButtonClicked()
    {
		List<Player> players = mRestaurantScript.getAlivePlayers();
		EnumPlayerRole role = players[mTurnManagerScript.GetCurrentPlayerIndex()].getRole();

		//requires the player to click the button three times if their role doesn't have them clicking otherwise
		if ((role == EnumPlayerRole.CHEMIST || role == EnumPlayerRole.FOOD_CRITIC || role == EnumPlayerRole.PARTY_GOER
			|| role == EnumPlayerRole.WEALTHY_COUPLE || role == EnumPlayerRole.SCAPEGOAT)
			&& mClickTracker > 1) {
			mClickTracker--;
			RandomizeActionButtonLocation ();
		}
		else{
			mClickTracker = 3;
			mActionButton.transform.position = mActionButtonLocation;

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
    }
    
    private bool isAssassinAlive()
    {
        List<Player> players = mRestaurantScript.getAlivePlayers();
        bool isAlive = false;
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].getRole() == EnumPlayerRole.ASSASSIN)
            {
                isAlive = true;
            }
        }

        return isAlive;
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
			SetCharacterArt (mAssassinSprite);
			break;
		case EnumPlayerRole.CHEMIST:
			SetCharacterArt (mChemistSprite);
			break;
		case EnumPlayerRole.DISTANT_COUSIN:
			SetCharacterArt (mDistantCousinSprite);
			break;
		case EnumPlayerRole.FOOD_CRITIC:
			SetCharacterArt (mFoodCriticSprite);
			break;
		case EnumPlayerRole.PARTY_GOER:
			SetCharacterArt (mPartygoerSprite);
			break;
		case EnumPlayerRole.PRIVATE_EYE:
			SetCharacterArt (mPrivateEyeSprite);
			break;
		case EnumPlayerRole.SCAPEGOAT:
			SetCharacterArt (mScapegoatSprite);
			break;
		case EnumPlayerRole.WEALTHY_COUPLE:
			if (mRestaurantScript.mWCSpritesAssigned) {
				if (players [mTurnManagerScript.GetCurrentPlayerIndex ()] == mRestaurantScript.mWealthyCouple1Player)
					SetCharacterArt (mRestaurantScript.mWealthyCouple1Sprite);
				else
					SetCharacterArt (mRestaurantScript.mWealthyCouple2Sprite);
			} 
			//assigns sprites if unassigned
			else {
				if (mRestaurantScript.mWealthyCouple1Player == null)
				{
					mRestaurantScript.mWealthyCouple1Player = players [mTurnManagerScript.GetCurrentPlayerIndex ()];
					if (Random.Range (0, 2) == 0)
						mRestaurantScript.mWealthyCouple1Sprite = mWealthyCouple1Sprite;
					else
						mRestaurantScript.mWealthyCouple1Sprite = mWealthyCouple2Sprite;

					SetCharacterArt (mRestaurantScript.mWealthyCouple1Sprite);
				}
				else
				{
					if (Random.Range (0, 2) == 0)
						mRestaurantScript.mWealthyCouple2Sprite = mWealthyCouple1Sprite;
					else
						mRestaurantScript.mWealthyCouple2Sprite = mWealthyCouple2Sprite;
					
					SetCharacterArt (mRestaurantScript.mWealthyCouple2Sprite);

					mRestaurantScript.mWCSpritesAssigned = true;
				}
			}
			break;
		}

        switch (role)
        {
		case EnumPlayerRole.ASSASSIN:
				List<EnumPlayerRole> unusedRoles = mRestaurantScript.getUnusedRoles ();
				Player roleOne = new Player ("r1", unusedRoles [0]);
				Player roleTwo = new Player ("r2", unusedRoles [1]);
				mRoleText.text += "\n" + roleOne.getRoleAsStringWithPrefix().ToUpper() + " AND "
				+ roleTwo.getRoleAsStringWithPrefix().ToUpper() + " DID NOT COME TO THE PARTY.";

				SetButtonAction("POISON FOOD", delegate {
					ShowPoisonScreen(EnumPlayerRole.ASSASSIN);
				});
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

                if (mTurnManagerScript.mCurrentRound == EnumCourse.ENTREE)
                {
					mRoleText.text += "\n" + assassin.getName ().ToUpper () + " IS THE ASSASSIN."
					+ "\nTHE WEALTHY COUPLE HAS " + wealthyCouple1.getName ().ToUpper ();
					if (wealthyCouple1.getRole() == EnumPlayerRole.WEALTHY_COUPLE)
						mRoleText.text += "\nAND " + wealthyCouple2.getName().ToUpper() + ".";
					mRoleText.text += "\nCHOOSE WHO TO MARK.";

                    SetButtonAction("MARK PLAYER", ShowMarkScreen);
                }
                else if (!isAssassinAlive())
                {
                    mRoleText.text += "\nTHE ASSASSIN IS OUT!"
						+ "\nTHE WEALTHY COUPLE HAS " + wealthyCouple1.getName ().ToUpper ();
					if (wealthyCouple1.getRole() == EnumPlayerRole.WEALTHY_COUPLE)
						mRoleText.text += "\nAND " + wealthyCouple2.getName().ToUpper() + ".";
					mRoleText.text += "\nCHOOSE WHO TO POISON.";
					SetButtonAction("POISON FOOD", delegate {
						ShowPoisonScreen(EnumPlayerRole.DISTANT_COUSIN);
						});
                }
                else
                {
					mRoleText.text += "\n" + assassin.getName ().ToUpper () + " IS THE ASSASSIN."
						+ "\nTHE WEALTHY COUPLE HAS " + wealthyCouple1.getName ().ToUpper ();
					if (wealthyCouple1.getRole() == EnumPlayerRole.WEALTHY_COUPLE)
						mRoleText.text += "\nAND " + wealthyCouple2.getName().ToUpper() + ".";
                    SetButtonAction("GOT IT", OnConfirmButtonClicked);
                }

                break;

			case EnumPlayerRole.WEALTHY_COUPLE:
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].getRole() == EnumPlayerRole.WEALTHY_COUPLE &&
                        players[i] != players[mTurnManagerScript.GetCurrentPlayerIndex()])
                    {
                        mRoleText.text += "\nYOUR PARTNER IS " + (players[i].getName()).ToUpper() + "."
						+ "\nTAP THE BUTTON THREE TIMES TO CONTINUE.";
                    }
                }

                SetButtonAction("GOT IT", OnConfirmButtonClicked);
                break;

			case EnumPlayerRole.PRIVATE_EYE:
				Player buggedPlayer = mRestaurantScript.getBuggedPlayer();
                string saying;

                if (buggedPlayer == null)
                {
                    saying = "\nTHE BUGGED MEAL DIDN'T PICK UP ON ANYTHING.";
                }
                else
                {
                    saying = "\nYOU HAVE BUGGED " + (buggedPlayer.getName()).ToUpper() + "."
                             + "\nTHEY ARE " + buggedPlayer.getRoleAsStringWithPrefix().ToUpper() + ".";
                    mRestaurantScript.setBuggedPlayer(null);
                }
				
				switch (mTurnManagerScript.mCurrentRound) {
					case EnumCourse.ENTREE:
						mRoleText.text += "\nCHOOSE WHICH MEAL TO BUG.";
						SetButtonAction("BUG PLAYER", ShowBugScreen);
							break;
					case EnumCourse.MAIN:
						mRoleText.text += saying
                            + "\nBUG A MEAL AGAIN.";
						SetButtonAction("BUG PLAYER", ShowBugScreen);
							break;
					case EnumCourse.DESSERT:
                        mRoleText.text += saying;
						SetButtonAction("GOT IT", OnConfirmButtonClicked);
							break;
				}
				break;

		default:
			{
				mRoleText.text += "\nTAP THE BUTTON THREE TIMES TO CONTINUE.";
				SetButtonAction ("GOT IT", OnConfirmButtonClicked);
			}
                break;
        }

		if (players [mTurnManagerScript.GetCurrentPlayerIndex ()].getLastMealEaten () != EnumSpecialMeal.NORMAL)
			mRoleText.text += "\nYOU ATE THE " + players [mTurnManagerScript.GetCurrentPlayerIndex ()].getSpecialMealAsString ().ToUpper()
			+ " MEAL.";
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

        for (int i = 0; i < mPlayerNamecards.Count; ++i)
        {
            mPlayerNamecards[i].image.color = Color.white;

            if (pressButton == mPlayerNamecards[i])
            {
                Debug.Log("HELLO1: " + mTurnManagerScript.GetCurrentPlayerIndex());
                Debug.Log("PLAYER COUNT: " + players.Count + " | BUTTON COUNT: " + mPlayerNamecards.Count);

                switch (players[mTurnManagerScript.GetCurrentPlayerIndex()].getRole())
                {
                    case EnumPlayerRole.ASSASSIN:
                    {
                        mRestaurantScript.SetPoisonedMealAtIndex(i);
                        mPlayerNamecards[i].image.color = Color.red;
                        Debug.Log("POISONED: " + players[i].getName());
                        break;
                    }
                    case EnumPlayerRole.DISTANT_COUSIN:
                    {
                        if (isAssassinAlive())
                        {
                            mRestaurantScript.MarkPlayerAtIndex(i);
                            mPlayerNamecards[i].image.color = Color.blue;
                            Debug.Log("MARKED: " + players[i].getName());
							players [i].setMarked (true);
                            break;
                        }
                        else
                        {
                            mRestaurantScript.SetPoisonedMealAtIndex(i);
                            mPlayerNamecards[i].image.color = Color.red;
                            Debug.Log("POISONED: " + players[i].getName());
                            break;
                        }
                    }
				    case EnumPlayerRole.PRIVATE_EYE:
                    {
                        mRestaurantScript.setBuggedPlayer(players[i]);
                        mRestaurantScript.SetBuggedMealAtIndex(i);
                        mPlayerNamecards[i].image.color = Color.green;
                        Debug.Log("BUGGED: " + players[i].getName());
                        break;
                    }
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

	public void ShowPoisonScreen(EnumPlayerRole roleToDisable)
    {
		mLazySusan.SetActive (true);
        mRolePanel.gameObject.SetActive(false);
        mActionPanel.gameObject.SetActive(true);
        mDoneWithActionButton.interactable = false;

		PlaceButtonsInCircle(roleToDisable);
        mDoneWithActionButton.transform.GetChild(0).GetComponent<Text>().text = "POISON A MEAL";

        SetButtonAction("CONFIRM", OnConfirmButtonClicked);
    }

    public void ShowMarkScreen()
    {
		mLazySusan.SetActive (true);
        mRolePanel.gameObject.SetActive(false);
        mActionPanel.gameObject.SetActive(true);
        mDoneWithActionButton.interactable = false;

        PlaceButtonsInCircle(EnumPlayerRole.DISTANT_COUSIN);
        mDoneWithActionButton.transform.GetChild(0).GetComponent<Text>().text = "MARK A PLAYER FIRST";

        SetButtonAction("CONFIRM", OnConfirmButtonClicked);
    }

	public void ShowBugScreen()
	{
		mLazySusan.SetActive (true);
		mRolePanel.gameObject.SetActive(false);
		mActionPanel.gameObject.SetActive(true);
		mDoneWithActionButton.interactable = false;

		PlaceButtonsInCircle(EnumPlayerRole.PRIVATE_EYE);
		mDoneWithActionButton.transform.GetChild(0).GetComponent<Text>().text = "BUG A MEAL FIRST";

		SetButtonAction("CONFIRM", OnConfirmButtonClicked);
	}

    public void OnActionConfirmClicked()
    {
		mLazySusan.SetActive (false);
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
        float radius = mCanvas.pixelRect.width / 3.0f;

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

            /*
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
            */

            currentAngle = ((currentAngle + distanceBetweenAngle) % 360);

            if (players[i].getRole().Equals(userRole))
            {
                userButton.interactable = false;
            }

            mPlayerNamecards.Add(userButton);
        }
    }

	private void RandomizeActionButtonLocation()
	{
		int randomX = Random.Range (0, Screen.width);
		int randomY = Random.Range (0, Screen.height);

		mActionButton.transform.position = new Vector3 (randomX, randomY, 1);
		//mActionButton.transform.SetAsLastSibling ();
	}

	private void SetCharacterArt(Sprite theSprite)
	{
		//mCharacterObject.GetComponent<SpriteRenderer> ().sprite = theSprite;
		mCharacterObject.GetComponent<Image> ().sprite = theSprite;
	}
}
