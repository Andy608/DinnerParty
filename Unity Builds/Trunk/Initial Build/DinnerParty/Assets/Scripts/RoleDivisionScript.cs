using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleDivisionScript : MonoBehaviour {

	//Player object class
	[System.Serializable] public class Player
	{
		string mName;
		string mRole;
		bool mRoleFound;

		public Player(string name) {
			this.mName = name;
			mRole = null;
			mRoleFound = false;
		}

		public void setRole(string role) { mRole = role; }
		public void setRoleFound(bool roleFound) { mRoleFound = roleFound; }

		public string getName() { return mName; }
		public string getRole() { return mRole; }
		public bool getIfRoleFound() { return mRoleFound; }
	};

	//members
	public int mNumberOfPlayers;
	public List<Player> mListOfPlayers;
	public List<string> mListOfRoles;
	public List<InputField> mListOfInputFields;
	public Transform mCenter;

	//role toggling-related members
	public Toggle mAssassinToggle;
	public Toggle mDistantCousinToggle;
	public Toggle mWealthyCouple1Toggle;
	public Toggle mWealthyCouple2Toggle;
	public Toggle mChemistToggle;
	public Toggle mFoodCriticToggle;
	public Toggle mPartygoer1Toggle;
	public Toggle mPartygoer2Toggle;
	public Toggle mPartygoer3Toggle;
	public Toggle mPartygoer4Toggle;
	string mChemistRole = "Chemist";
	string mFoodCriticRole = "Food Critic";
	string mPartygoerRole = "Partygoer";
	bool mChemistToggleBool = true;
	bool mFoodCriticToggleBool = true;
	bool mPartygoer1ToggleBool = true;
	bool mPartygoer2ToggleBool = true;
	bool mPartygoer3ToggleBool = true;
	bool mPartygoer4ToggleBool = true;

	public List<Toggle> mListOfToggles;

	//other UI members
	public Slider mPlayerCountSlider;
	public Text mScreenText;
	public string mScreenTextString;
	public Text mPlayerCountNumberText;
	public Text mNextButtonText;

	public int mNumberOfScreens = 0;

	//sets this class up as a Listener for when the slider's value changes
	void Start()
	{
		//add role toggles to a list so they can be removed easily
		AddTogglesToList ();

		//tie slider/toggles to functions
		mPlayerCountSlider.onValueChanged.AddListener(delegate {ChangePlayerCount(); });
		mChemistToggle.onValueChanged.AddListener(delegate {ToggleChemist(); });
		mFoodCriticToggle.onValueChanged.AddListener(delegate {ToggleFoodCritic(); });
		mPartygoer1Toggle.onValueChanged.AddListener(delegate {TogglePartygoer1(); });
		mPartygoer2Toggle.onValueChanged.AddListener(delegate {TogglePartygoer2(); });
		mPartygoer3Toggle.onValueChanged.AddListener(delegate {TogglePartygoer3(); });
		mPartygoer4Toggle.onValueChanged.AddListener(delegate {TogglePartygoer4(); });

		//only make the  current amount of name inputs = current number of players
		for (int i = mListOfInputFields.Count - 1; i >= mNumberOfPlayers; i--)
		{
			mListOfInputFields [i].gameObject.SetActive (false);
		}

		//RotatePlayerInputs ();
	}

	public void ButtonPress()
	{
		if (mNumberOfScreens == 0)
		{
			if (RandomizeRoles ())
			{
				mNumberOfScreens++;

				//remove the toggles and input fields and slider from the screen
				for (int i = 0; i < mNumberOfPlayers; i++)
				{
					mListOfInputFields [i].gameObject.SetActive (false);
				}
				for (int j = 0; j < mListOfToggles.Count; j++)
				{
					mListOfToggles [j].gameObject.SetActive (false);
				}
				mPlayerCountSlider.gameObject.SetActive (false);

			}
		} 
		else
		{

		}
	}

	//will randomly assign roles to the players when the button is pressed
	bool RandomizeRoles()
	{
		if (mNumberOfPlayers == mListOfRoles.Count) {
			for (int i = 0; i < mNumberOfPlayers; i++) {
				string currentPlayerName;
				if (mListOfInputFields [i].text.Length > 0)
					currentPlayerName = mListOfInputFields [i].text;
				else
					currentPlayerName = "Player " + (i + 1).ToString ();

				Player newPlayer = new Player (currentPlayerName);
				mListOfPlayers.Add (newPlayer);
			}

			for (int j = 0; j < mListOfRoles.Count; j++) {
				int playerNumber;
				do {
					playerNumber = Random.Range (0, mListOfPlayers.Count);
				} while(mListOfPlayers [playerNumber].getIfRoleFound ());
							
				Player tempPlayer = mListOfPlayers [playerNumber];
				tempPlayer.setRole (mListOfRoles [j]);
				tempPlayer.setRoleFound (true);

				if ((j + 1) % 2 == 0)
					mScreenTextString += tempPlayer.getName () + " " + tempPlayer.getRole () + ",  ";
				else
					mScreenTextString += tempPlayer.getName () + " " + tempPlayer.getRole () + "\n";
			}

			mScreenText.text = mScreenTextString;

			return true;
		} 
		else
		{
			mScreenText.text = "The number of roles selected does\nnot match the amount of players!";
			return false;
		}
	}

	//changes the player count along with the slider value
	void ChangePlayerCount()
	{
		int playerCountValue = (int)mPlayerCountSlider.value;

		mListOfInputFields [playerCountValue - 1].gameObject.SetActive (true);
		for (int i = mListOfInputFields.Count - 1; i >= playerCountValue; i--)
		{
			mListOfInputFields [i].gameObject.SetActive (false);
		}

		mNumberOfPlayers = playerCountValue;
		mPlayerCountNumberText.text = playerCountValue.ToString ();

		//RotatePlayerInputs ();
	}

	//toggles the roles in and out of the role list
	void ToggleChemist()
	{
		if (!mChemistToggleBool)
		{
			mListOfRoles.Add (mChemistRole);
			mChemistToggleBool = true;
		}
		else
		{
			mListOfRoles.Remove (mChemistRole);
			mChemistToggleBool = false;
		}
	}
	void ToggleFoodCritic()
	{
		if (!mFoodCriticToggleBool)
		{
			mListOfRoles.Add (mFoodCriticRole);
			mFoodCriticToggleBool = true;
		}
		else
		{
			mListOfRoles.Remove (mFoodCriticRole);
			mFoodCriticToggleBool = false;
		}
	}
	void TogglePartygoer1()
	{
		if (!mPartygoer1ToggleBool)
		{
			mListOfRoles.Add (mPartygoerRole);
			mPartygoer1ToggleBool = true;
		}
		else
		{
			mListOfRoles.Remove (mPartygoerRole);
			mPartygoer1ToggleBool = false;
		}
	}
	void TogglePartygoer2()
	{
		if (!mPartygoer2ToggleBool)
		{
			mListOfRoles.Add (mPartygoerRole);
			mPartygoer2ToggleBool = true;
		}
		else
		{
			mListOfRoles.Remove (mPartygoerRole);
			mPartygoer2ToggleBool = false;
		}
	}
	void TogglePartygoer3()
	{
		if (!mPartygoer3ToggleBool)
		{
			mListOfRoles.Add (mPartygoerRole);
			mPartygoer3ToggleBool = true;
		}
		else
		{
			mListOfRoles.Remove (mPartygoerRole);
			mPartygoer3ToggleBool = false;
		}
	}
	void TogglePartygoer4()
	{
		if (!mPartygoer4ToggleBool)
		{
			mListOfRoles.Add (mPartygoerRole);
			mPartygoer4ToggleBool = true;
		}
		else
		{
			mListOfRoles.Remove (mPartygoerRole);
			mPartygoer4ToggleBool = false;
		}
	}

	void AddTogglesToList()
	{
		Toggle[] mArrayOfToggles = {mAssassinToggle, mDistantCousinToggle,
			mWealthyCouple1Toggle,
			mWealthyCouple2Toggle,
			mChemistToggle,
			mFoodCriticToggle,
			mPartygoer1Toggle,
			mPartygoer2Toggle,
			mPartygoer3Toggle,
			mPartygoer4Toggle
		};

		for (int i = 0; i < mArrayOfToggles.Length; i++) {
			mListOfToggles.Add (mArrayOfToggles [i]);
		}
	}

	/*void RotatePlayerInputs()
	{
		float angle = (float)(360 / mNumberOfPlayers);
		float currentAngle = 0;

		for (int j = 0; j < mNumberOfPlayers; j++)
		{
			mCenter.localRotation = Quaternion.Euler(mCenter.localRotation.x, mCenter.localRotation.y, currentAngle);
			//mListOfInputFields [j].gameObject.transform.localRotation = mCenter.rotation;
			currentAngle += angle;
		}
	}*/
}
