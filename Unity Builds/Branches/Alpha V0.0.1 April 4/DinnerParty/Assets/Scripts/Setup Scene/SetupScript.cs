using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetupScript : MonoBehaviour
{
    //This is used when randomly generating players and their roles.
    public List<InputField> mUsernameFields;
    public List<Toggle> mRoleToggles;
    public Slider mPlayerCountSlider;
    public Text mPlayerCountText;

    private List<string> mUsernames;
    private List<EnumPlayerRole> mValidUserRoles;
	private bool mOnToNameInput;

    void Start()
    {
        mUsernames = new List<string>();
        mValidUserRoles = new List<EnumPlayerRole>();
		mOnToNameInput = false;

        InitUserRoles();
		
        mPlayerCountSlider.onValueChanged.AddListener(OnChangePlayerCount);
    }

    public void AddPlayerRole(EnumPlayerRole role)
    {
        Debug.Log("Adding role!");
        mValidUserRoles.Add(role);
    }

    public void RemovePlayerRole(EnumPlayerRole role)
    {
        Debug.Log("Removing role!");
        mValidUserRoles.Remove(role);
    }

    public void OnChangePlayerCount(float value)
    {
        int playerCount = (int)mPlayerCountSlider.value;
        //ActivateUsernameFields(playerCount);
        mPlayerCountText.text = playerCount.ToString();
    }

    public void OnNextClicked()
    {
		//after button press on role toggle screen
		if (!mOnToNameInput)
		{
			//deactives everything but the Next/Back buttons, activates the username fields
			Debug.Log ("SLIDER COUNT: " + (int)mPlayerCountSlider.value + " | VALID USER ROLES: " + mValidUserRoles.Count);
			if ((int)mPlayerCountSlider.value == mValidUserRoles.Count) {
				DeactivateRoleToggles ();
				LockPlayerCountSlider ();
				ActivateUsernameFields ((int)mPlayerCountSlider.value);
				mOnToNameInput = true;
				Debug.Log ("We can start!");
			}
			else {
				Debug.Log ("The number of roles selected doesn't match the amount of players!");
			}
		} 
		//after button press on name input screen
		else
		{
			//randomizes roles and runs the game
			PopulateNamesList ();
			RandomizeRoles ();
		}
    }

	public void OnBackClicked()
	{
		//after button button on role toggle screen
		if (!mOnToNameInput)
		{
			//go back to the title screen
			Debug.Log(DinnerPartyScenes.TITLE_PATH);
			//Go to Player Pass Scene
			SceneManager.LoadScene(DinnerPartyScenes.TITLE_PATH);
		}
		//after button press on name input screen
		else
		{
			//reactivates the slider/role toggles, deactivates the username fields
			ActivateRoleToggles ();
			UnlockPlayerCountSlider ();
			DeactivateUsernameFields ();
			mOnToNameInput = false;
		}
	}

    private void DeactivateUsernameFields()
    {
        int i;
        for (i = 0; i < mUsernameFields.Count; ++i)
        {
            mUsernameFields[i].gameObject.SetActive(false);
        }
    }

    private void ActivateUsernameFields(int fields)
    {
        DeactivateUsernameFields();

        int i;
        for (i = 0; i < mUsernameFields.Count && i < fields; i++)
        {
            mUsernameFields[i].gameObject.SetActive(true);
        }
    }

	private void DeactivateRoleToggles()
	{
		int i;
		for (i = 0; i < mRoleToggles.Count; ++i)
		{
			mRoleToggles[i].gameObject.SetActive(false);
		}
	}

	private void ActivateRoleToggles()
	{
		int i;
		for (i = 0; i < mRoleToggles.Count; ++i)
		{
			mRoleToggles[i].gameObject.SetActive(true);
		}
	}

	private void LockPlayerCountSlider()
	{
		mPlayerCountSlider.interactable = false;
	}

	private void UnlockPlayerCountSlider()
	{
		mPlayerCountSlider.interactable = true;
	}

    private void RandomizeRoles()
    {
		//shuffles the roles around into a new list
        List<EnumPlayerRole> shuffedRoles = new List<EnumPlayerRole>();
        int randomIndex;

        while (mValidUserRoles.Count > 0)
        {
            randomIndex = Random.Range(0, mValidUserRoles.Count);
            shuffedRoles.Add(mValidUserRoles[randomIndex]);
            mValidUserRoles.RemoveAt(randomIndex);
        }

		//gives a new players list their roles
        List<Player> players = new List<Player>();

        int i;
        for (i = 0; i < (int)mPlayerCountSlider.value; ++i)
        {
            players.Add(new Player(mUsernames[i], shuffedRoles[i]));
            Debug.Log(players[i].ToString());
        }

        Debug.Log(GameManagerScript.GetInstance().GetComponent<TurnManagerScript>() == null);
        GameManagerScript.GetInstance().GetComponent<TurnManagerScript>().setPlayers(players);

        Debug.Log(DinnerPartyScenes.PASS_PATH);
        //Go to Player Pass Scene
        SceneManager.LoadScene(DinnerPartyScenes.PASS_PATH);
    }

    private void InitUserRoles()
    {
        mValidUserRoles.Clear();

        int i;
        for (i = 0; i < mRoleToggles.Count; ++i)
        {
            if (mRoleToggles[i].isOn)
            {
                mValidUserRoles.Add(mRoleToggles[i].GetComponent<UIToggleScript>().GetRoleType());
            }
        }
    }

    private void PopulateNamesList()
    {
        int i;
        int playerCount = (int)mPlayerCountSlider.value;

        //Init everyone's name.
        for (i = 0; i < playerCount; ++i)
        {
            string username = "PLAYER " + (i + 1);

            //If the input field is not empty, update name.
            if (!string.IsNullOrEmpty(mUsernameFields[i].text))
            {
                username = mUsernameFields[i].text.ToUpper();
            }

            mUsernames.Add(username);
        }
    }
}
