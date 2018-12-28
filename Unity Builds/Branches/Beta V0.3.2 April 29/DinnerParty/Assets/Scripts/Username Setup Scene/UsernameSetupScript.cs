using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UsernameSetupScript : MonoBehaviour
{
    private TurnManagerScript mTurnManagerScript;
    private RestaurantScript mRestaurantScript;

    public Canvas mCanvas;

    private InputField mInputPrefab;
    private List<InputField> mUsernameFields;

    private List<string> mUsernames;
    private List<EnumPlayerRole> mValidUserRoles;

    public GameObject mTableCenter;

    public int mExtraPlayerAmount = 2;

    void Start()
    {
        mTurnManagerScript = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>();
        mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();
        mInputPrefab = Resources.Load<InputField>("UsernameField");

        mUsernames = new List<string>();
        mUsernameFields = new List<InputField>();

        mValidUserRoles = Player.sValidRoles;

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

        //START THE GAME!
        Debug.Log("Let's start the dinner party!");
        mTurnManagerScript.StartGame();
		SceneManager.LoadScene(DinnerPartyScenes.SHOW_ROLE_PATH);
    }

    private void RandomizeRoles()
    {
		//removes 3 random unnecessary roles from the pool
		List<EnumPlayerRole> nonNecessaryRoles = new List<EnumPlayerRole>();
		for (int j = 0; j < mValidUserRoles.Count; j++) {
			if (mValidUserRoles[j] != EnumPlayerRole.ASSASSIN && mValidUserRoles[j] != EnumPlayerRole.WEALTHY_COUPLE && mValidUserRoles[j] != EnumPlayerRole.DISTANT_COUSIN)
				nonNecessaryRoles.Add (mValidUserRoles [j]);
		}

		List<int> randomNums = new List<int>();
		for (int k = 0; k < mExtraPlayerAmount; k++) {
			int rand = -1;

			do {
				rand = Random.Range (0, nonNecessaryRoles.Count);
			} while(randomNums.Contains(rand));

			randomNums.Add (rand);
			mRestaurantScript.addUnusedRole (nonNecessaryRoles [rand]);
			mValidUserRoles.Remove (nonNecessaryRoles[rand]);
		}

        //shuffles the roles around into a new list
        List<EnumPlayerRole> shuffedRoles = new List<EnumPlayerRole>();
        int randomIndex;

        while (mValidUserRoles.Count > 0)
        {
            randomIndex = Random.Range(0, mValidUserRoles.Count);
            shuffedRoles.Add(mValidUserRoles[randomIndex]);
            mValidUserRoles.RemoveAt(randomIndex);
        }

        Debug.Log("ROLE COUNT: " + shuffedRoles.Count);

        for (int i = 0; i < shuffedRoles.Count; ++i)
        {
            Player player = new Player(mUsernames[i], shuffedRoles[i]);
            mRestaurantScript.addPlayer(player);
            Debug.Log("PLAYER COUNT: " + mRestaurantScript.getAlivePlayers().Count);
            Debug.Log(player.ToString());
        }
    }

    private void PopulateNamesList()
    {
        int i;
		for (i = 0; i < (Player.sValidRoles.Count - mExtraPlayerAmount); ++i)
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
		float distanceBetweenAngle = 360.0f / (Player.sValidRoles.Count - mExtraPlayerAmount);

        //Have the current player be at the bottom so it's closest to the user.
        float currentAngle = 270.0f;

        //Scale radius by screen size to keep it consistent.
        float radius = mCanvas.pixelRect.width / 3.0f;

        int i;
		for (i = 0; i < (Player.sValidRoles.Count - mExtraPlayerAmount); ++i)
        {
            InputField userLabel = Instantiate(mInputPrefab, mTableCenter.transform);
            Vector3 pos = mTableCenter.transform.position;

            pos.x += radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
            pos.y += radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);

            userLabel.transform.position = pos;

            /*
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
            */

            currentAngle = ((currentAngle + distanceBetweenAngle) % 360);

            mUsernameFields.Add(userLabel);
        }
    }
}
