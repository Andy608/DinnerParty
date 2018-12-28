using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UsernameSetupScript : MonoBehaviour
{
    public Canvas mCanvas;

    private InputField mInputPrefab;
    private List<InputField> mUsernameFields;

    private List<string> mUsernames;
    private List<EnumPlayerRole> mValidUserRoles;

    public GameObject mTableCenter;
    private int mPlayerCount;

    private TurnManagerScript mTurnManager;

    void Start()
    {
        mInputPrefab = Resources.Load<InputField>("UsernameField");
        mTurnManager = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>();

        mUsernames = new List<string>();
        mUsernameFields = new List<InputField>();
        mValidUserRoles = mTurnManager.getValidRoles();
        mPlayerCount = mTurnManager.getPlayerCount();

        PlaceLabelsInCircle();
	}

    public void OnBackClicked()
    {
        SceneManager.LoadScene(DinnerPartyScenes.SETUP_PATH);
    }

    public void OnStartClicked()
    {
        PopulateNamesList();
        RandomizeRoles();
        SceneManager.LoadScene(DinnerPartyScenes.PASS_PATH);
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
        for (i = 0; i < mPlayerCount; ++i)
        {
            players.Add(new Player(mUsernames[i], shuffedRoles[i]));
            Debug.Log(players[i].ToString());
        }

        //set list of valid roles
        mTurnManager.setPlayers(players);
    }

    private void PopulateNamesList()
    {
        int i;

        //Init everyone's name.
        for (i = 0; i < mPlayerCount; ++i)
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

    private void PlaceLabelsInCircle()
    {
        float distanceBetweenAngle = 360.0f / mPlayerCount;

        //Have the current player be at the bottom so it's closest to the user.
        float currentAngle = 270.0f;

        //Scale radius by screen size to keep it consistent.
        float radius = mCanvas.pixelRect.width / 5.0f;

        int i;
        for (i = 0; i < mPlayerCount; ++i)
        {
            InputField userLabel = Instantiate(mInputPrefab, mTableCenter.transform);
            Vector3 pos = mTableCenter.transform.position;

            pos.x += radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
            pos.y += radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);

            userLabel.transform.position = pos;

            Vector3 rot = userLabel.transform.eulerAngles;

            if ((currentAngle > 270 && currentAngle < 360) || (currentAngle < 90 && currentAngle > 0))
            {
                rot.z = currentAngle;
            }
            else if (currentAngle > 90 && currentAngle < 270)
            {
                rot.z = currentAngle - 180;
            }

            userLabel.transform.eulerAngles = rot;

            currentAngle = ((currentAngle + distanceBetweenAngle) % 360);

            mUsernameFields.Add(userLabel);
        }
    }
}
