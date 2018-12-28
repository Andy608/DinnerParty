using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleScript : MonoBehaviour
{
    private GameObject setupManager;

    public EnumPlayerRole mRoleType;
    private Toggle mToggle;

    void Start()
    {
        //I dont like having to find the object but it's a quick fix for now.
        setupManager = GameObject.Find("SetupManager");

        mToggle = GetComponent<Toggle>();
        mToggle.onValueChanged.AddListener(OnToggleClicked);
	}
	
	private void OnToggleClicked(bool clicked)
    {
        if (mToggle.isOn)
        {
            setupManager.GetComponent<SetupScript>().AddPlayerRole(mRoleType);
        }
        else
        {
            setupManager.GetComponent<SetupScript>().RemovePlayerRole(mRoleType);
        }

    }

    public EnumPlayerRole GetRoleType()
    {
        return mRoleType;
    }
}
