using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnumCourse
{
    ENTREE,
    MAIN,
    DESSERT
}

public class TurnManagerScript : MonoBehaviour
{
    public EnumCourse mCurrentRound;
    private int mCurrentPlayerIndex;

    private RestaurantScript mRestaurantScript;

    void Start()
    {
        mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();
    }

    public void StartGame()
    {
        mCurrentRound = EnumCourse.ENTREE;
        mCurrentPlayerIndex = 0;
        ServeFood();
    }

    private void ResetRound()
    {
        mRestaurantScript.resetRound();
        mCurrentPlayerIndex = 0;
    }

    public void GoToNextRound()
    {
        ResetRound();

        if (mCurrentRound == EnumCourse.DESSERT)
        {
            Debug.Log("THE GAME IS OVER!");
            mCurrentRound = 0;
            SceneManager.LoadScene(DinnerPartyScenes.RESULTS_PATH);
        }
        else
        {
            ++mCurrentRound;
            ServeFood();
			mRestaurantScript.CheckForStomachAche ();
            SceneManager.LoadScene(DinnerPartyScenes.SHOW_ROLE_PATH);
        }
    }

    private int getRandomNumber(int min, int max)
    {
        return Random.Range(min, max);
    }

    public void ServeFood()
    {
        Debug.Log("SERVING FOOD! ;D");
        List<Color> plateColors = new List<Color>(mRestaurantScript.mPossibleColors);

        for (int i = 0; i < mRestaurantScript.getAlivePlayers().Count; ++i)
        {
            Debug.Log("HELLOOOO: " + mRestaurantScript.mPossibleColors.Length + " | " + i.ToString());

            int randomIndex = getRandomNumber(0, plateColors.Count);

            //In the future create fun meals here depending on theme/course number.
            mRestaurantScript.addMeal(new Meal(mRestaurantScript.mMenuItems[(int)mCurrentRound], plateColors[randomIndex]));
            plateColors.RemoveAt(randomIndex);
        }

        List<Meal> mealsList = mRestaurantScript.getMeals ();

		//sets # of special meals depending on player count
		int numOfSpecialMeals = 0;
		switch (mRestaurantScript.getAlivePlayers ().Count) {
		case 5:
			numOfSpecialMeals = 1;
			break;
		case 6:
		case 7:
			numOfSpecialMeals = 2;
			break;
		case 8:
		case 9:
		case 10:
			numOfSpecialMeals = 3;
			break;
		}

		//finds what meals to set as special
		List<int> specialMealIndices = new List<int>();
		for (int j = 0; j < numOfSpecialMeals; j++)
		{
			int randomNum;
			do {
				randomNum = Random.Range (0, mRestaurantScript.getAlivePlayers ().Count);
			} while(specialMealIndices.Contains (randomNum));
			specialMealIndices.Add (randomNum);
		}

		//sets the randomly-selected meals
		for (int k = 0; k < mealsList.Count; k++) {
			if (specialMealIndices.Contains (k))
				mealsList [k].setSpecial (true);
			else
				mealsList [k].setSpecial (false);
		}
    }

    public bool GoToNextPlayer()
    {
        bool isNotLastPlayer;

        Debug.Log(mRestaurantScript == null);

        if (mCurrentPlayerIndex < mRestaurantScript.getAlivePlayers().Count - 1)
        {
            ++mCurrentPlayerIndex;
            isNotLastPlayer = true;
        }
        else
        {
            isNotLastPlayer = false;
        }

        return isNotLastPlayer;
    }

    public int GetCurrentPlayerIndex()
    {
        return mCurrentPlayerIndex;
    }
}
