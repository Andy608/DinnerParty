using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantScript : MonoBehaviour
{
    private List<Player> mAllPlayers = new List<Player>();

    private List<Player> mAlivePlayers = new List<Player>();
    private List<Meal> mMeals = new List<Meal>();

    private int mMarkedPlayerIndex;

    public void addPlayer(Player newGuest)
    {
        mAllPlayers.Add(newGuest);
        mAlivePlayers.Add(newGuest);
    }
    
    public void addMeal(Meal meal)
    {
        mMeals.Add(meal);
    }

    public List<Player> getAllPlayers()
    {
        return mAllPlayers;
    }

    public List<Player> getAlivePlayers()
    {
        return mAlivePlayers;
    }

    public List<Meal> getMeals()
    {
        return mMeals;
    }

    public void clearEveryone()
    {
        clearAlivePlayers();
        clearAlivePlayers();
    }

    private void clearAllPlayers()
    {
        mAllPlayers.Clear();
    }

    private void clearAlivePlayers()
    {
        mAlivePlayers.Clear();
    }

    public void clearMeals()
    {
        mMeals.Clear();
    }

    public void RotatePlatterLeft()
    {
        if (mMeals.Count == 0) return;

    	List<Meal> tempMealList = new List<Meal>();

        tempMealList.Add(mMeals[mMeals.Count - 1]);

    	for (int i = 0; i < mMeals.Count - 1; i++)
        {
    		tempMealList.Add (mMeals[i]);
    	}

    	mMeals = tempMealList;

        mMarkedPlayerIndex = (mMarkedPlayerIndex + 1) % mAlivePlayers.Count;
    }

    public void RotatePlatterRight()
    {
        if (mMeals.Count == 0) return;

        List<Meal> tempMealList = new List<Meal>();

        for (int i = 1; i < mMeals.Count; i++)
        {
    	    tempMealList.Add(mMeals[i]);
    	}

    	tempMealList.Add(mMeals[0]);

    	mMeals = tempMealList;

        mMarkedPlayerIndex--;

        if (mMarkedPlayerIndex < 0)
        {
            mMarkedPlayerIndex = mAlivePlayers.Count - 1;
        }
    }

    public void SetPoisonedMealAtIndex(int mealIndex, bool poisoned)
    {
        mMeals[mealIndex].setPoisoned(poisoned);
    }

    public void MarkPlayerAtIndex(int playerIndex)
    {
        mMarkedPlayerIndex = playerIndex;
    }

    public int GetMarkedPlayerIndex()
    {
        return mMarkedPlayerIndex;
    }

    public List<int> GetPoisonedMealIndexes()
    {
        List<int> poisonedMealIndexes = new List<int>();

        int i;
        for (i = 0; i < mMeals.Count; ++i)
        {
            if (mMeals[i].isPoisoned())
            {
                poisonedMealIndexes.Add(i);
            }
        }

        return poisonedMealIndexes;
    }
}
