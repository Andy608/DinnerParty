using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetupScript : MonoBehaviour
{
    //private RestaurantScript mRestaurantScript;

    public List<Toggle> mRoleToggles;
    public Slider mPlayerCountSlider;
    public Text mPlayerCountText;

    private int mPlayerCount;
    private bool mIsCustomRoles;

    void Start()
    {
        //mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();

        mPlayerCount = (int)mPlayerCountSlider.value;
        mPlayerCountSlider.onValueChanged.AddListener(OnChangePlayerCount);
        mIsCustomRoles = false;

        InitToggledRoles();
        InitUserRoles();
    }

    public void AddPlayerRole(EnumPlayerRole role)
    {
        Debug.Log("Adding role!");
        Player.sValidRoles.Add(role);
        Debug.Log("ROLE COUNT: " + Player.sValidRoles.Count);
        mIsCustomRoles = true;
    }

    public void RemovePlayerRole(EnumPlayerRole role)
    {
        Debug.Log("Removing role!");
        Player.sValidRoles.Remove(role);
        Debug.Log("ROLE COUNT: " + Player.sValidRoles.Count);
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
		Debug.Log ("SLIDER COUNT: " + (int)mPlayerCountSlider.value + " | VALID USER ROLES: " + Player.sValidRoles.Count);
		if ((int)mPlayerCountSlider.value == Player.sValidRoles.Count)
        {
			Debug.Log ("We can start!");
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
        Player.sValidRoles.Clear();

        int i;
        for (i = 0; i < mRoleToggles.Count; ++i)
        {
            if (mRoleToggles[i].isOn)
            {
                Player.sValidRoles.Add(mRoleToggles[i].GetComponent<UIToggleScript>().GetRoleType());
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
