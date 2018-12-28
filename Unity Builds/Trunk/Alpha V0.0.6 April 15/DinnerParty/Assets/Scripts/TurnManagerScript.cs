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
    //private List<EnumPlayerRole> mValidRoles;
    //private List<Button> mPlayerNamecards;
    //private List<Button> mPlayerMeals;

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
            mRestaurantScript.resetGame();
            SceneManager.LoadScene(DinnerPartyScenes.TITLE_PATH);
        }
        else
        {
            ++mCurrentRound;
            ServeFood();
            SceneManager.LoadScene(DinnerPartyScenes.SHOW_ROLE_PATH);
        }
    }

    public void ServeFood()
    {
        Debug.Log("SERVING FOOD! ;D");

        int i;
        for (i = 0; i < mRestaurantScript.getAlivePlayers().Count; ++i)
        {
            //In the future create fun meals here depending on theme/course number.
            mRestaurantScript.addMeal(new Meal());
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

    //private int mPlayerCount;
    //private Player mMarkedPlayer;
    //private Button mPoisonedMeal;

    //public List<EnumPlayerRole> getValidRoles()
    //{
    //    return mValidRoles;
    //}

    //public void setValidRoles(List<EnumPlayerRole> roles)
    //{
    //    mValidRoles = roles;
    //}

    //public void setPlayerCount(int playerCount)
    //{
    //    mPlayerCount = playerCount;
    //}

    //public int getPlayerCount()
    //{
    //    return mPlayerCount;
    //}

    public int GetCurrentPlayerIndex()
    {
        return mCurrentPlayerIndex;
    }

    //public void goToNextPlayer()
    //{
    //    ++mCurrentPlayerIndex;
    //}

    //public void setMarkedPlayer(Player player)
    //{
    //    mMarkedPlayer = player;
    //}

    //public Player getMarkedPlayer()
    //{
    //    return mMarkedPlayer;
    //}

 //   public void setPoisonedMeal(Button meal)
 //   {
 //       //Set the old meal back to white.
 //       mPoisonedMeal.image.color = Color.white;
 //       mPoisonedMeal = meal;

 //       //Set the new meal to red.
 //       mPoisonedMeal.image.color = Color.red;
 //   }

 //   public void setPoisonedMealAtIndex(int i)
 //   {
 //       //Set the old meal back to white.
 //       mPoisonedMeal.image.color = Color.white;
 //       mPoisonedMeal = mPlayerMeals[i];

 //       //Set the new meal to red.
 //       mPoisonedMeal.image.color = Color.red;
 //   }

 //   public Button getPoisnedMeal()
 //   {
 //       return mPoisonedMeal;
 //   }

	//public void setPlayerNamecards(List<Button> playerNamecards)
	//{
	//	mPlayerNamecards = playerNamecards;
	//}

	//public List<Button> getPlayerNamecards()
	//{
	//	return mPlayerNamecards;
	//}

 //   public void setPlayerMeals(List<Button> playerMeals)
 //   {
 //       mPlayerMeals = playerMeals;
 //   }

 //   public List<Button> getPlayerMeals()
 //   {
 //       return mPlayerMeals;
 //   }

 //   public void RotatePlatterLeft()
	//{
	//	List<Button> tempMealList = new List<Button>();

 //       tempMealList.Add(mPlayerMeals[mPlayerMeals.Count - 1]);

	//	for (int i = 0; i < mPlayerMeals.Count - 1; i++) {
	//		tempMealList.Add (mPlayerMeals[i]);
	//	}

	//	mPlayerMeals = tempMealList;
	//}

	//public void RotatePlatterRight()
	//{
 //       List<Button> tempMealList = new List<Button>();

 //       for (int i = 1; i < mPlayerMeals.Count; i++) {
	//		tempMealList.Add (mPlayerMeals[i]);
	//	}

	//	tempMealList.Add(mPlayerMeals[0]);

	//	mPlayerMeals = tempMealList;
	//}
}
