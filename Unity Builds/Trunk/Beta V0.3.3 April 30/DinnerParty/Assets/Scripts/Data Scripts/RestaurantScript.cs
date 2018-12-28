using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestaurantScript : MonoBehaviour
{
    //Should be 10 in size.
    public Color[] mPossibleColors;
    public Image[] mMenuItems;

    private List<Player> mAllPlayers = new List<Player>();

    private List<Player> mAlivePlayers = new List<Player>();
    private List<Meal> mMeals = new List<Meal>();

	private List<EnumPlayerRole> mUnusedRoles = new List<EnumPlayerRole>();

    private int mMarkedPlayerIndex;

	//used when doing actions
	private Player mFirstClickedMeal;
	private bool mHeldDown = false;
	private bool mCanDoAction = false;
	private List<Color> mTricksterColors = new List<Color>();
	private bool mTricksterColorsSet = false;
	private bool mTricksterTurn = false;

	//used when selecting player
	private List<Player> mPlayersWithTurnsLeft = new List<Player>();
	public Player mSelectedPlayer;
	public bool mIsAPlayerSelected = false;
	public bool mPlayerRandomlySelected = false;
	private bool mPlayerReceivedTurn = false;

	//used to store who is bugged
	private Player mBuggedPlayer;

	//stores the sprites chosen for the wealthy couple
	public Sprite mWealthyCouple1Sprite;
	public Sprite mWealthyCouple2Sprite;
	public Player mWealthyCouple1Player;
	public bool mWCSpritesAssigned = false;

	public GameObject mMealPeekPanel;

	void Start()
	{
		mFirstClickedMeal = null;
    }

    public void resetRound()
    {
        mMarkedPlayerIndex = 0;
        mFirstClickedMeal = null;
        mHeldDown = false;
        mCanDoAction = false;
        mPlayersWithTurnsLeft.Clear();
        mPlayersWithTurnsLeft = new List<Player>(mAlivePlayers);
        mSelectedPlayer = null;
        mIsAPlayerSelected = false;
        mMeals.Clear();
		ShowRoleScript.showRolePanel = false;
    }

    public void resetGame()
    {
        mBuggedPlayer = null;
        resetRound();
        mAlivePlayers.Clear();
        mAllPlayers.Clear();
        mMeals.Clear();
        mPlayersWithTurnsLeft.Clear();
        Player.sValidRoles.Clear();
    }

    public void setBuggedPlayer(Player buggedPlayer)
    {
        mBuggedPlayer = buggedPlayer;
    }

    public Player getBuggedPlayer()
    {
        return mBuggedPlayer;
    }

    public void addPlayer(Player newGuest)
    {
        mAllPlayers.Add(newGuest);
        mAlivePlayers.Add(newGuest);

		mPlayersWithTurnsLeft.Add (newGuest);
    }
    
    public void addMeal(Meal meal)
    {
        mMeals.Add(meal);
    }

	public void addUnusedRole(EnumPlayerRole role)
	{
		mUnusedRoles.Add (role);
	}

    public List<Player> getAllPlayers()
    {
        return mAllPlayers;
    }

    public List<Player> getAlivePlayers()
    {
        return mAlivePlayers;
    }

    public List<Player> getPlayersWithTurnsLeft()
    {
        return mPlayersWithTurnsLeft;
    }

    //This needs to be tested
    public List<Player> getOutOfTurnPlayers()
    {
        List<Player> noTurnPlayers = new List<Player>(mAlivePlayers);

        foreach (Player player in mPlayersWithTurnsLeft)
        {
            if (noTurnPlayers.Contains(player))
            {
                noTurnPlayers.Remove(player);
            }
        }

        return noTurnPlayers;
    }

    public List<Meal> getMeals()
    {
        return mMeals;
    }

	public List<EnumPlayerRole> getUnusedRoles()
	{
		return mUnusedRoles;
	}

    public void clearEveryone()
    {
        clearAllPlayers();
        clearAlivePlayers();
        mPlayersWithTurnsLeft.Clear();
    }

    private void clearAllPlayers()
    {
        mAllPlayers.Clear();
    }

    private void clearAlivePlayers()
    {
        mAlivePlayers.Clear();
    }

    public void clearMeals()
    {
        mMeals.Clear();
    }

	public void CheckForStomachAche()
	{
		for (int i = 0; i < mAlivePlayers.Count; i++) {
			if (mAlivePlayers [i].getLastMealEaten () == EnumSpecialMeal.STOMACHACHE)
				mPlayersWithTurnsLeft.Remove (mAlivePlayers [i]);
		}
	}

    public void RotatePlatterLeft()
    {
        if (mMeals.Count == 0) return;

        setTricksterColors();

        List<Meal> tempMealList = new List<Meal>();

        tempMealList.Add(mMeals[mMeals.Count - 1]);

    	for (int i = 0; i < mMeals.Count - 1; i++)
        {
    		tempMealList.Add (mMeals[i]);
    	}

    	mMeals = tempMealList;

        //mMarkedPlayerIndex = (mMarkedPlayerIndex + 1) % mAlivePlayers.Count;

		//for (int i = 0; i < mMeals.Count; i++)
		//	mMeals [i].setPlateColor (mTricksterColors[i]);

        GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
    }

    public void setTricksterColors()
    {
        if (mSelectedPlayer.getLastMealEaten() == EnumSpecialMeal.TRICKSTER && !mTricksterColorsSet)
        {
            mTricksterColors.Clear();

            for (int i = 0; i < mMeals.Count; i++)
            {
                mTricksterColors.Add(mMeals[i].getPlateColor());
            }

            mTricksterColorsSet = true;
        }

        Debug.Log("TRICKSTER COLORS: " + mTricksterColors.Count);
    }

    public void RotatePlatterRight()
    {
        if (mMeals.Count == 0) return;

        setTricksterColors();

        List<Meal> tempMealList = new List<Meal>();

        for (int i = 1; i < mMeals.Count; i++)
        {
    	    tempMealList.Add(mMeals[i]);
    	}

    	tempMealList.Add(mMeals[0]);

    	mMeals = tempMealList;

        GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
    }

    public void SetPoisonedMealAtIndex(int mealIndex)
    {
        int i;
        for (i = 0; i < mMeals.Count; ++i)
        {
            mMeals[i].setPoisoned(false);
        }

        mMeals[mealIndex].setPoisoned(true);
    }

	public void SetBuggedMealAtIndex(int mealIndex)
	{
		int i;
		for (i = 0; i < mMeals.Count; ++i)
		{
			mMeals[mealIndex].setBugged(false);
		}

		mMeals[mealIndex].setBugged(true);
	}

    public void MarkPlayerAtIndex(int playerIndex)
    {
        mMarkedPlayerIndex = playerIndex;
    }

    public int GetMarkedPlayerIndex()
    {
        return mMarkedPlayerIndex;
    }

	public void SetInvestigatePanel(GameObject panel)
	{
		mMealPeekPanel = panel;
	}

    public List<int> GetPoisonedMealIndexes()
    {
        List<int> poisonedMealIndexes = new List<int>();

        int i;
        for (i = 0; i < mMeals.Count; ++i)
        {
            if (mMeals[i].isPoisoned())
            {
                poisonedMealIndexes.Add(i);
            }
        }

        return poisonedMealIndexes;
    }

	public void ChooseWhoseTurn(Player chosenPlayer, Button chosenPlayerButton)
	{
		//if a player's already been selected
		if (!mIsAPlayerSelected)
		{
			if (mPlayersWithTurnsLeft.Contains (chosenPlayer))
			{
				mIsAPlayerSelected = true;
				mSelectedPlayer = chosenPlayer;
				Debug.Log ("Player selected: " + chosenPlayer.getName());
				GameObject.Find ("GameplayManager").GetComponent<TurnTimerScript>().StopTimer ();

				if (mSelectedPlayer.getLastMealEaten () == EnumSpecialMeal.TRICKSTER) {
					mTricksterTurn = true;
					TricksterNoticePanel ();
				}
					
			}
			//player couldn't be found in list so they're out of turns
			else
				Debug.Log ("Player is out of turns!");
		}
		else
		{
			//resets stuff if the player's name was deselected
			if (chosenPlayer == mSelectedPlayer)
			{
				if (!mPlayerRandomlySelected && !mPlayerReceivedTurn) {
					mIsAPlayerSelected = false;
					mSelectedPlayer = null;
					mFirstClickedMeal = null;
					Debug.Log ("Player has been deselected.");
					GameObject.Find ("GameplayManager").GetComponent<TurnTimerScript>().ResumeTimer ();
				}
			}
			//gives turn away to someone else if their name is pressed
			else if (chosenPlayer.getLastMealEaten() != EnumSpecialMeal.STOMACHACHE && !mPlayerReceivedTurn)
			{
				mPlayersWithTurnsLeft.Remove (mSelectedPlayer);
				mPlayersWithTurnsLeft.Add (chosenPlayer);
				mSelectedPlayer = chosenPlayer;
				mPlayerReceivedTurn = true;
				Debug.Log (chosenPlayer.getName () + " has been given another turn! You must take it now!");
			}
		}

        GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
        GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateButtonColors();
    }
		
	public void EndWhoseTurn()
	{
        //resets all the selected player stuff once an action has been made
        mPlayersWithTurnsLeft.Remove(mSelectedPlayer);

        Debug.Log("MEALS COUNT: " + mMeals.Count + "TRICKSTER COLOR COUNT: " + mTricksterColors.Count);

        if (mSelectedPlayer.getLastMealEaten () == EnumSpecialMeal.TRICKSTER)
        {
			for (int i = 0; i < mMeals.Count; i++)
            {
				mMeals [i].setPlateColor (mTricksterColors[i]);
            }

			mTricksterColorsSet = false;
		}

        mIsAPlayerSelected = false;
		mSelectedPlayer = null;
		mFirstClickedMeal = null;
		mPlayerRandomlySelected = false;
		mPlayerReceivedTurn = false;

		GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
        GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateButtonColors();

		GameObject.Find ("GameplayManager").GetComponent<TurnTimerScript>().RestartTimer ();
		
        //does something when all turns have been used
		if (!mMealPeekPanel.gameObject.activeSelf && mPlayersWithTurnsLeft.Count <= 0)
        {
			Debug.Log ("Round over!");
            SceneManager.LoadScene(DinnerPartyScenes.VOTE_PATH);
        }
    }

	private void SwitchPlates(Player clickedMeal)
	{
        if (!mIsAPlayerSelected)
        {
            return;
        }

		if (mFirstClickedMeal == null)
        {
			mFirstClickedMeal = clickedMeal;

			Debug.Log ("Pick another meal.");
		}
		else
        {
            setTricksterColors();

            //finds index of the 2 selected meals
            int firstClickedIndex = -1, secondClickedIndex = -1;
			firstClickedIndex = mAlivePlayers.IndexOf (mFirstClickedMeal);
			secondClickedIndex = mAlivePlayers.IndexOf (clickedMeal);

			//find the two meals clicked in the Meals array
			//int firstClicked = -1, secondClicked = -1;
			//for (int i = 0; i < mAllPlayers.Count; i++) {
			//	if (mFirstClickedMeal == mAlivePlayers [i]) {
			//		firstClicked = i;
			//	}
			//	if (clickedMeal == mAlivePlayers [i]) {
			//		secondClicked = i;
			//	}
			//}
				
			//switch the two meals
			if ((firstClickedIndex != -1 && secondClickedIndex != -1)) {
				if (firstClickedIndex != secondClickedIndex) {
					Meal tempMeal = mMeals [firstClickedIndex];
					mMeals [firstClickedIndex] = mMeals [secondClickedIndex];
					mMeals [secondClickedIndex] = tempMeal;

					//won't switch the colors if the trickster does the action
					if (mSelectedPlayer.getLastMealEaten () == EnumSpecialMeal.TRICKSTER) {
						Color firstPlateColor = mMeals [firstClickedIndex].getPlateColor ();
						mMeals [firstClickedIndex].setPlateColor(mMeals [secondClickedIndex].getPlateColor());
						mMeals [secondClickedIndex].setPlateColor (firstPlateColor);
					}

					Debug.Log ("Meals switched!");
					//if (mSelectedPlayer.getLastMealEaten() != EnumSpecialMeal.TRICKSTER)


                    //EITHER THIS ERRORED OR EndWhoseTurn() ERRORED
					GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
                    Debug.Log("FUCK ME IN THE ASS RANDY");
                    EndWhoseTurn();
                } else {
					Debug.Log ("Meal unselected.");
				}
			}

			mFirstClickedMeal = null;
		}
    }

	private void InvestigatePlate(Player clickedMeal)
	{
			//.transform.GetChild(0).GetComponent<Text>().text
		mMealPeekPanel.SetActive(true);
		Text panelText = mMealPeekPanel.transform.GetChild (0).GetComponent<Text> ();

		int clicked = -1;
		for (int i = 0; i < mAlivePlayers.Count; i++) {
			if (clickedMeal == mAlivePlayers [i]) {
				clicked = i;
			}
		}

        if (clicked == -1 || mSelectedPlayer == null)
        {
            Debug.Log("ERROR INSPECTING MEAL...");
			panelText.text = "ERROR INSPECTING MEAL...";
            return;
        }
		
		Debug.Log ("Special: " + mMeals [clicked].isSpecial());
		panelText.text = "Special: " + mMeals [clicked].isSpecial();

		if (mMeals [clicked].isSpecial () && (mSelectedPlayer.getRole () == EnumPlayerRole.FOOD_CRITIC || mSelectedPlayer.getLastMealEaten () == EnumSpecialMeal.SEER)) {
			Debug.Log ("Special Type: " + mMeals [clicked].getTypeOfSpecialMeal ().ToString ());
			panelText.text += "\nSpecial Type: " + mMeals [clicked].getTypeOfSpecialMeal ().ToString ().ToLower();
		}

		if (mSelectedPlayer.getRole () == EnumPlayerRole.CHEMIST || mSelectedPlayer.getLastMealEaten () == EnumSpecialMeal.SEER) {
			Debug.Log ("Poisoned: " + mMeals [clicked].isPoisoned ());
			panelText.text += "\nPoisoned: " + mMeals [clicked].isPoisoned ();
		}

        //GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
    }

	//used to determine if the meal is held down or not
	public void ClickPlateDown()
	{
		if (mIsAPlayerSelected) {
			StartCoroutine (HoldDownTimer ());
			mHeldDown = true;
		}

       //GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
    }

	public void ClickPlateUp(Player clickedMeal)
	{
		StopAllCoroutines ();

		if (mCanDoAction && mFirstClickedMeal == null && clickedMeal != null) {
			if (mSelectedPlayer.getLastMealEaten() != EnumSpecialMeal.BLINDNESS)
				InvestigatePlate (clickedMeal);
        }
		else {
			SwitchPlates (clickedMeal);
		}

		mHeldDown = false;
		mCanDoAction = false;

        //GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
    }

	IEnumerator HoldDownTimer()
	{
		yield return new WaitForSeconds (1.0f);

		if (mHeldDown)
			mCanDoAction = true;
	}

    public void VotePlayerOffTheIsland(Player player)
    {
        mAlivePlayers.Remove(player);
    }

	public void TurnOffInvestigatePanel ()
	{
		mMealPeekPanel.SetActive (false);

		if (!mTricksterTurn) {
			EndWhoseTurn ();

			if (mPlayersWithTurnsLeft.Count <= 0) {
				Debug.Log ("Round over!");
				SceneManager.LoadScene (DinnerPartyScenes.VOTE_PATH);
			}
		} else {
			mTricksterTurn = false;
		}
    }

	public void RandomizeTurn()
	{
		int randomPlayer = Random.Range (0, mPlayersWithTurnsLeft.Count);
		mIsAPlayerSelected = true;
		mSelectedPlayer = mPlayersWithTurnsLeft [randomPlayer];
		mPlayerRandomlySelected = true;
		GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateButtonColors();
	}

	public void TricksterNoticePanel()
	{
		mMealPeekPanel.SetActive (true);
		Text panelText = mMealPeekPanel.transform.GetChild (0).GetComponent<Text> ();

		panelText.text = "This player ate a Trickster\nMeal last turn! Pick up\nthe tablet and perform your\naction from there.";
	}

	public void PoisonPassPanel(int numOfRotatedSpaces)
	{
		mMealPeekPanel.SetActive(true);
		Text panelText = mMealPeekPanel.transform.GetChild (0).GetComponent<Text> ();

		//create bool that will turn true if a checked player is a wealth couple
		bool passedWC = false;
		int poisonedMealIndex = -1;
			
		if (numOfRotatedSpaces > 0)
		{
			int rotatedSpaces = numOfRotatedSpaces;

			if (rotatedSpaces > mAlivePlayers.Count)
				rotatedSpaces = mAlivePlayers.Count;
				
			//find num of meal that is poisoned
			for (int i = 0; i < mMeals.Count; i++) {
				if (mMeals [i].isPoisoned ()) {
					//int temp = (i - rotatedSpaces) % mAlivePlayers.Count;
					int temp = modulo((i - rotatedSpaces), mAlivePlayers.Count);
					poisonedMealIndex = temp;
					break;
				}
			}
				
			//loop through the ppl based on the number of meals that were passed
			for (int j = 1; j < rotatedSpaces; j++) {
				//int temp = (poisonedMealIndex + j) % mAlivePlayers.Count;
				int temp = modulo((poisonedMealIndex + j), mAlivePlayers.Count);
				if (mAlivePlayers [temp].getRole() == EnumPlayerRole.WEALTHY_COUPLE)
					passedWC = true;
			}
		}
		else
		{
			int rotatedSpaces = numOfRotatedSpaces;

			if (rotatedSpaces > mAlivePlayers.Count)
				rotatedSpaces = mAlivePlayers.Count;

			//find num of meal that is poisoned
			for (int i = 0; i < mMeals.Count; i++) {
				if (mMeals [i].isPoisoned ()) {
					//int temp = (i - rotatedSpaces) % mAlivePlayers.Count;
					int temp = modulo((i + rotatedSpaces), mAlivePlayers.Count);
					poisonedMealIndex = temp;
					break;
				}
			}

			//loop through the ppl based on the number of meals that were passed
			for (int j = -1; j > rotatedSpaces; j--) {
				//int temp = (poisonedMealIndex + j) % mAlivePlayers.Count;
				int temp = modulo((poisonedMealIndex + j), mAlivePlayers.Count);
				if (mAlivePlayers [temp].getRole() == EnumPlayerRole.WEALTHY_COUPLE)
					passedWC = true;
			}
		}

		//output based on if it passed one or not
		if (passedWC) {
			panelText.text = "The poisoned meal DID\npass a member of\nthe Wealthy Couple!";
		}
		else {
			panelText.text = "The poisoned meal DID NOT\npass a member of\nthe Wealthy Couple!";
		}

	}

	int modulo (int a, int b)
	{
		//return a - b * Mathf.FloorToInt(a / b);
		//return (a%b + b)%a;
		int result = a % b;
		if ((result < 0 && b > 0) || (result > 0 && b < 0)) {
			result += b;
		}
		return result;
	}
}
