using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectionScript : MonoBehaviour
{
    private GameObject setupManager;

    public EnumPlayerRole mRoleType;
    public bool mAlwaysSelected;
    private Button mSelected;
    private bool mIsSelected;
    private bool mAlreadySelected;

    void Start()
    {
        //I dont like having to find the object but it's a quick fix for now.
        setupManager = GameObject.Find("SetupManager");

        mSelected = GetComponent<Button>();
        mSelected.onClick.AddListener(OnSelectionClicked);
        mIsSelected = false;
        mAlreadySelected = false;
    }
	
	private void OnSelectionClicked()
    {
        if (!mIsSelected)
        {
            setSelected(true);
        }
        else
        {
            setSelected(false);
        }
    }

    public void setSelected(bool b)
    {
        mIsSelected = b;

        Debug.Log("SELECTED: " + mIsSelected.ToString() + " | ALWAYS SELECTED: " + mAlwaysSelected.ToString());

        if (mIsSelected && !mAlreadySelected)
        {
            mAlreadySelected = true;

            if (mAlwaysSelected)
            {
                //Debug.Log("BUTTON IS NULL: " + (mSelected == null).ToString());
                //Debug.Log("BUTTON IMAGE IS NULL: " + (mSelected.GetComponent<Image>() == null).ToString());
                mSelected.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                mSelected.GetComponent<Image>().color = Color.green;
            }

            Debug.Log("ADDING PLAYER ROLE");
            setupManager.GetComponent<SetupScript>().AddPlayerRole(mRoleType);
        }
        else if (!mAlwaysSelected)
        {
            Debug.Log("REMOVING PLAYER ROLE");
            mSelected.GetComponent<Image>().color = Color.white;
            setupManager.GetComponent<SetupScript>().RemovePlayerRole(mRoleType);
            mAlreadySelected = false;
        }
    }
    
    public bool isButtonSelected()
    {
        return mIsSelected;
    }

    public EnumPlayerRole GetRoleType()
    {
        return mRoleType;
    }
}
