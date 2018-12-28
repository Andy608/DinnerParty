using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour {

	public Canvas mCanvas;

    public GameObject mTableCenter;

	public GameObject mMealPeekPanel;
	public Button mGotItButton;

    private Button mPlatePrefab;
    private Button mPlayerPrefab;

	private GameObject mIceCreamPrefab;

    private RestaurantScript mRestaurantScript;

    private List<Button> mPlayerNamecards;
    private List<Button> mPlayerMeals;

    void Start()
    {
        mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();
	
		mRestaurantScript.SetInvestigatePanel (mMealPeekPanel);
		mGotItButton.onClick.AddListener(delegate 
			{
				mRestaurantScript.TurnOffInvestigatePanel();
			});

        mPlatePrefab = Resources.Load<Button>("Dinner Plate");
        mPlayerPrefab = Resources.Load<Button>("UserButton");
		mIceCreamPrefab = Resources.Load<GameObject> ("Skull");

        mPlayerNamecards = new List<Button>();
        mPlayerMeals = new List<Button>();

        PlacePlayersInCircle();
        PlacePlatesInCircle();
        UpdateMealColors();
		UpdateButtonColors ();
    }

    public void UpdateMealColors()
    {
        int i;
        for (i = 0; i < mPlayerMeals.Count; ++i)
        {
            mPlayerMeals[i].image.color = mRestaurantScript.getMeals()[i].getPlateColor();
        }
    }

    public void UpdateButtonColors()
    {
        List<Player> alivePlayers = mRestaurantScript.getAlivePlayers();
		List<Player> playersWithTurns = mRestaurantScript.getPlayersWithTurnsLeft();

        //Reset all colors.
        for (int i = 0; i < mPlayerNamecards.Count; ++i)
        {
            Button b = mPlayerNamecards[i];
            if (b.enabled)
            {
                b.image.color = mPlayerNamecards[i].colors.normalColor;
            }
            else
            {
                b.image.color = mPlayerNamecards[i].colors.disabledColor;
            }
        }

		for (int i = 0; i < mPlayerNamecards.Count; ++i)
		{
			//If the player is out of turns, disable their button
			Button b = mPlayerNamecards[i];

			//will turn selected player yellow and all others as active color
			if (mRestaurantScript.mSelectedPlayer != null) 
			{
				if (alivePlayers [i] == mRestaurantScript.mSelectedPlayer) 
				{
					mPlayerNamecards [i].image.color = Color.yellow;
				}
				else 
				{
					b.image.color = mPlayerNamecards [i].colors.normalColor;
				}
			} 
			//will turn all players their correct color
			else 
			{
				if (playersWithTurns.Contains (alivePlayers [i])) 
				{
					b.image.color = mPlayerNamecards [i].colors.normalColor;
				} 
				else 
				{
					b.image.color = mPlayerNamecards [i].colors.disabledColor;
				}
			}
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
			Player currentPlayer = players [i];

			Button userButton = Instantiate(mPlayerPrefab, mTableCenter.transform);
			userButton.onClick.AddListener(delegate 
            {
                GameManagerScript.GetInstance().GetComponent<RestaurantScript>().ChooseWhoseTurn(currentPlayer, userButton);
            });

			userButton.transform.GetChild(0).GetComponent<Text>().text = players[i].getName();

			Vector3 pos = mTableCenter.transform.position;

			pos.x += radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
			pos.y += radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);

			userButton.transform.position = pos;

			currentAngle = ((currentAngle + distanceBetweenAngle) % 360);

            mPlayerNamecards.Add(userButton);

			if (players [i].getMarked()) {
				Vector3 buttonTransform;

				if (i < players.Count / 2)
					buttonTransform = new Vector3(userButton.transform.position.x + (userButton.GetComponent<RectTransform>().rect.width / 3),
						userButton.transform.position.y + (userButton.GetComponent<RectTransform>().rect.height / 3));
				else
					buttonTransform = new Vector3(userButton.transform.position.x - (userButton.GetComponent<RectTransform>().rect.width / 3),
						userButton.transform.position.y + (userButton.GetComponent<RectTransform>().rect.height / 3));
				GameObject mark = Instantiate(mIceCreamPrefab, buttonTransform, Quaternion.identity);
				mark.transform.SetParent(GameObject.Find ("Canvas").transform);
				mark.transform.localScale = new Vector3 (0.45f, 0.45f, 0.45f);
			}
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
            userPlate.transform.GetChild(0).GetComponent<Image>().color = mRestaurantScript.getMeals()[i].getPlateColor();
            userPlate.transform.GetChild(1).GetComponent<Image>().sprite = mRestaurantScript.getMeals()[i].getFood().sprite;

			Player currentPlayer = players [i];

			//adds triggers to the plates that allows the code to know when the button is held
			EventTrigger eventTrigger = userPlate.gameObject.AddComponent<EventTrigger> () as EventTrigger;
			EventTrigger.Entry downEntry = new EventTrigger.Entry();
			EventTrigger.Entry upEntry = new EventTrigger.Entry();
			downEntry.eventID = EventTriggerType.PointerDown;
			downEntry.callback.AddListener( (eventData) => { GameManagerScript.GetInstance().GetComponent<RestaurantScript>().ClickPlateDown(); } );
			upEntry.eventID = EventTriggerType.PointerUp;
			upEntry.callback.AddListener( (eventData) => { GameManagerScript.GetInstance().GetComponent<RestaurantScript>().ClickPlateUp(currentPlayer); } );
			eventTrigger.triggers.Add(downEntry);
			eventTrigger.triggers.Add(upEntry);

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

	public void GoToRulesDocument()
	{
		Application.OpenURL ("https://goo.gl/nopbH8");
	}

	public void GoToMainMenu()
	{
        mRestaurantScript.resetGame();
		SceneManager.LoadScene(DinnerPartyScenes.TITLE_PATH);
	}
}
