using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VoteScript : MonoBehaviour
{
    public Canvas mCanvas;
    public GameObject mTableCenter;

    private Button mPlatePrefab;
    private Button mPlayerPrefab;
    private Button mTiePrefab;

    private RestaurantScript mRestaurantScript;

    public CanvasRenderer mDeliberationPanel;
    public CanvasRenderer mVotingPanel;

    private List<Button> mPlayerNamecards;
    private List<Button> mPlayerMeals;
    private Button tieButton;

	void Start ()
    {
        mDeliberationPanel.gameObject.SetActive(true);
        mVotingPanel.gameObject.SetActive(false);

        mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();

        mPlatePrefab = Resources.Load<Button>("Dinner Plate");
        mPlayerPrefab = Resources.Load<Button>("UserButton");
        mTiePrefab = Resources.Load<Button>("Tie Button");

        mPlayerNamecards = new List<Button>();
        mPlayerMeals = new List<Button>();

		FindBugged ();
		AssignSpecialMeals ();
		FindDead ();
    }

	private void FindDead()
	{
		List<Meal>mealsList = mRestaurantScript.getMeals ();
		List<Player>alivePlayersList = mRestaurantScript.getAlivePlayers ();

		for (int i = 0; i < mealsList.Count; i++) {
			if (mealsList [i].isPoisoned ()) {
				Debug.Log ("Poisoned meal number is: " + i.ToString());
				if (alivePlayersList[i].getRole() == EnumPlayerRole.ASSASSIN || alivePlayersList[i].getLastMealEaten() == EnumSpecialMeal.STOMACHACHE)
				{
					Debug.Log("No one has been poisoned!");
				}
				else
				{
					Debug.Log(alivePlayersList[i].getName() + " has been poisoned!");
					mRestaurantScript.VotePlayerOffTheIsland (alivePlayersList [i]);
				}
				break;
			}
		}
	}

	private void FindBugged()
	{
		List<Meal>mealsList = mRestaurantScript.getMeals ();
		List<Player>alivePlayersList = mRestaurantScript.getAlivePlayers ();

		for (int i = 0; i < mealsList.Count; i++) {
			if (mealsList [i].isBugged ()) {
				Debug.Log ("Bugged meal number is: " + i.ToString());
				if (alivePlayersList[i].getRole() == EnumPlayerRole.PRIVATE_EYE || alivePlayersList[i].getLastMealEaten() == EnumSpecialMeal.STOMACHACHE)
				{
					Debug.Log("The bugged meal did not pick up one anyone!");
					mRestaurantScript.mBuggedPlayer = null;
				}
				else
				{
					Debug.Log(alivePlayersList[i].getName() + " ate the bugged meal! Their role is " + alivePlayersList[i].getRoleAsStringWithPrefix() + ".");
					mRestaurantScript.mBuggedPlayer = alivePlayersList [i];
				}
				break;
			}
		}
	}

	private void AssignSpecialMeals()
	{
		List<Meal>mealsList = mRestaurantScript.getMeals ();
		List<Player>alivePlayersList = mRestaurantScript.getAlivePlayers ();

		for (int i = 0; i < mealsList.Count; i++) {
			alivePlayersList [i].setLastMealEaten (EnumSpecialMeal.NORMAL);

			if (mealsList [i].isSpecial()) {
				alivePlayersList [i].setLastMealEaten (mealsList[i].getTypeOfSpecialMeal());
				Debug.Log(alivePlayersList [i].getName() + " has been given the meal: " + alivePlayersList[i].getSpecialMealAsString());
			}
		}
	}

    public void OnContinueClicked()
    {
        PlacePlayersInCircle();
        //PlacePlatesInCircle();

        mDeliberationPanel.gameObject.SetActive(false);
        mVotingPanel.gameObject.SetActive(true);
    }

    private void PlacePlayersInCircle()
    {
        tieButton = Instantiate(mTiePrefab, mTableCenter.transform);
        tieButton.onClick.AddListener(delegate
        {
            VoteForPlayer(tieButton);
        });

        List<Player> players = mRestaurantScript.getAlivePlayers();

        float distanceBetweenAngle = 360.0f / players.Count;

        //Have the current player be at the bottom so it's closest to the user.
        float currentAngle = 270.0f;

        //Scale radius by screen size to keep it consistent.
        float radius = mCanvas.pixelRect.width / 3.0f;

        int i;
        for (i = 0; i < players.Count; ++i)
        {
            Player currentPlayer = players[i];

            Button userButton = Instantiate(mPlayerPrefab, mTableCenter.transform);
            userButton.transform.GetChild(0).GetComponent<Text>().text = players[i].getName();
            userButton.onClick.AddListener(delegate 
            {
                VoteForPlayer(userButton);
            });

            Vector3 pos = mTableCenter.transform.position;

            pos.x += radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
            pos.y += radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);

            userButton.transform.position = pos;

            Vector3 rot = userButton.transform.eulerAngles;

            if ((currentAngle > 270 && currentAngle < 360) || (currentAngle < 90 && currentAngle > 0))
            {
                rot.z = currentAngle;
            }
            else if (currentAngle > 90 && currentAngle < 270)
            {
                rot.z = currentAngle - 180;
            }

            currentAngle = ((currentAngle + distanceBetweenAngle) % 360);

            mPlayerNamecards.Add(userButton);
        }
    }

    private void PlacePlatesInCircle()
    {
        List<Player> players = mRestaurantScript.getAlivePlayers();

        float distanceBetweenAngle = 360.0f / players.Count;

        //Have the current player be at the bottom so it's closest to the user.
        float currentAngle = 270.0f;

        //Scale radius by screen size to keep it consistent.
        float radius = mCanvas.pixelRect.width / 4.2f;

        int i;
        for (i = 0; i < players.Count; ++i)
        {
            Button userPlate = Instantiate(mPlatePrefab, mTableCenter.transform);
            userPlate.enabled = false;
            userPlate.image.color = userPlate.colors.normalColor;
            Vector3 pos = mTableCenter.transform.position;

            pos.x += radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
            pos.y += radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);

            userPlate.transform.position = pos;

            Vector3 rot = userPlate.transform.eulerAngles;

            if ((currentAngle > 270 && currentAngle < 360) || (currentAngle < 90 && currentAngle > 0))
            {
                rot.z = currentAngle;
            }
            else if (currentAngle > 90 && currentAngle < 270)
            {
                rot.z = currentAngle - 180;
            }

            userPlate.transform.eulerAngles = rot;

            currentAngle = ((currentAngle + distanceBetweenAngle) % 360);

            mPlayerMeals.Add(userPlate);
        }
    }

    private void VoteForPlayer(Button voteButton)
    {
        if (voteButton == tieButton)
        {
            Debug.Log("It was a tie!");
        }
        else
        {
            for (int i = 0; i < mPlayerNamecards.Count; ++i)
            {
                if (voteButton == mPlayerNamecards[i])
                {
                    Player player = mRestaurantScript.getAlivePlayers()[i];
                    Debug.Log(player.getName() + " was voted out! :o");
                    mRestaurantScript.VotePlayerOffTheIsland(player);
                    break;
                }
            }
        }

        GameManagerScript.GetInstance().GetComponent<TurnManagerScript>().GoToNextRound();
    }
}
