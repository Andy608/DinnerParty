using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameScript : MonoBehaviour {

	public Canvas mCanvas;

    public GameObject mTableCenter;

    private Button mPlatePrefab;
    private Button mPlayerPrefab;

    //private List<Player> players;
    private TurnManagerScript mTurnManagerScript;
    private RestaurantScript mRestaurantScript;

    private List<Button> mPlayerNamecards;
    private List<Button> mPlayerMeals;

    void Start()
    {
        mTurnManagerScript = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>();
        mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();
        //players = mTurnManagerScript.getPlayers();

        mPlatePrefab = Resources.Load<Button>("Dinner Plate");
        mPlayerPrefab = Resources.Load<Button>("UserButton");

        mPlayerNamecards = new List<Button>();
        mPlayerMeals = new List<Button>();

        PlacePlayersInCircle();
        PlacePlatesInCircle();
        UpdateMealColors();
    }

    public void UpdateMealColors()
    {
        int i;
        for (i = 0; i < mPlayerMeals.Count; ++i)
        {
            mPlayerMeals[i].image.color = Color.white;
        }

        List<int> poisonedMealIndexes = mRestaurantScript.GetPoisonedMealIndexes();
        int markedIndex = mRestaurantScript.GetMarkedPlayerIndex();
        bool markedSet = false;

        for (i = 0; i < poisonedMealIndexes.Count; ++i)
        {
            int poisonedIndex = poisonedMealIndexes[i];

            if (poisonedIndex != markedIndex)
            {
                mPlayerMeals[poisonedMealIndexes[i]].image.color = Color.red;
            }
            else
            {
                mPlayerMeals[poisonedMealIndexes[i]].image.color = new Color(255, 0, 255);
                markedSet = true;
            }
        }

        if (!markedSet)
        {
            mPlayerMeals[mRestaurantScript.GetMarkedPlayerIndex()].image.color = Color.blue;
        }
    }

    private void PlacePlayersInCircle()
	{
        List<Player> players = mRestaurantScript.getAlivePlayers();

        float distanceBetweenAngle = 360.0f / players.Count;

		//Have the current player be at the bottom so it's closest to the user.
		float currentAngle = 270.0f;

		//Scale radius by screen size to keep it consistent.
		float radius = mCanvas.pixelRect.width / 3.0f;

		int i;
		for (i = 0; i < players.Count; ++i)
		{
			Button userButton = Instantiate(mPlayerPrefab, mTableCenter.transform);
			//userButton.onClick.AddListener(delegate { OnUserSelected(userButton); });
			userButton.transform.GetChild(0).GetComponent<Text>().text = players[i].getName();

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

			//userButton.transform.eulerAngles = rot;

			currentAngle = ((currentAngle + distanceBetweenAngle) % 360);

            mPlayerNamecards.Add(userButton);
		}

        //mTurnManagerScript.setPlayerNamecards(mPlayerNamecards);
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
            //userButton.onClick.AddListener(delegate { OnUserSelected(userButton); });
            //userPlate.transform.GetChild(0).GetComponent<Text>().text = players[i].getName();

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

        //mTurnManagerScript.setPlayerMeals(mPlayerMeals);
    }
}
