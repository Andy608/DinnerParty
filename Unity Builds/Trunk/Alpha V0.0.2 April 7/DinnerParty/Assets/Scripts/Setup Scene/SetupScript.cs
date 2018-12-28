using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetupScript : MonoBehaviour
{
    public List<Toggle> mRoleToggles;
    public Slider mPlayerCountSlider;
    public Text mPlayerCountText;
    private List<EnumPlayerRole> mValidUserRoles;

    private int mPlayerCount;
    private bool mIsCustomRoles;

    void Start()
    {
        mValidUserRoles = new List<EnumPlayerRole>();
        mPlayerCount = (int)mPlayerCountSlider.value;
        mPlayerCountSlider.onValueChanged.AddListener(OnChangePlayerCount);
        mIsCustomRoles = false;

        InitToggledRoles();
        InitUserRoles();
    }

    public void AddPlayerRole(EnumPlayerRole role)
    {
        Debug.Log("Adding role!");
        mValidUserRoles.Add(role);
        mIsCustomRoles = true;
    }

    public void RemovePlayerRole(EnumPlayerRole role)
    {
        Debug.Log("Removing role!");
        mValidUserRoles.Remove(role);
        mIsCustomRoles = true;
    }

    public void OnChangePlayerCount(float value)
    {
        mPlayerCount = (int)mPlayerCountSlider.value;
        mPlayerCountText.text = mPlayerCount.ToString();

        if (!mIsCustomRoles)
        {
            InitToggledRoles();
        }
    }

    public void OnNextClicked()
    {
		Debug.Log ("SLIDER COUNT: " + (int)mPlayerCountSlider.value + " | VALID USER ROLES: " + mValidUserRoles.Count);
		if ((int)mPlayerCountSlider.value == mValidUserRoles.Count)
        {
			Debug.Log ("We can start!");
            GameManagerScript.GetInstance().GetComponent<TurnManagerScript>().setPlayerCount(mPlayerCount);
            GameManagerScript.GetInstance().GetComponent<TurnManagerScript>().setValidRoles(mValidUserRoles);
            SceneManager.LoadScene(DinnerPartyScenes.USER_SETUP_PATH);
        }
		else
        {
			Debug.Log ("The number of roles selected doesn't match the amount of players!");
		}
    }

	public void OnBackClicked()
	{
		Debug.Log(DinnerPartyScenes.TITLE_PATH);
		SceneManager.LoadScene(DinnerPartyScenes.SETUP_PATH);
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

    private void InitToggledRoles()
    {
        ToggleOffRoles();
        ToggleOnDefaultRoles();
        mIsCustomRoles = false;
    }

    private void ToggleOffRoles()
    {
        int i;
        for (i = 0; i < mRoleToggles.Count; ++i)
        {
            mRoleToggles[i].isOn = false;
        }
    }

    private void ToggleOnDefaultRoles()
    {
        int i;
        for (i = 0; i < mPlayerCount; ++i)
        {
            mRoleToggles[i].isOn = true;
        }
    }
}
