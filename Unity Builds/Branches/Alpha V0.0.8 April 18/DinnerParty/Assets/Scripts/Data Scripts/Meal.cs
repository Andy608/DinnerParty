using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumSpecialMeal
{
	NORMAL = 0,
	SEER = 1,
	TRICKSTER = 2,
	STOMACHACHE = 3,
	BLINDNESS = 4,
	NUM_OF_SPECIAL_TYPES = 5
}

public class Meal
{
    bool mIsPoisoned;
	bool mIsSpecial;
	bool mIsBugged;
	EnumSpecialMeal mSpecialType;

    public Meal()
    {
        mIsPoisoned = false;
    }

    public bool isPoisoned()
    {
        return mIsPoisoned;
    }

    public void setPoisoned(bool poisoned)
    {
        mIsPoisoned = poisoned;
    }

	public bool isBugged()
	{
		return mIsBugged;
	}

	public void setBugged(bool bugged)
	{
		mIsBugged = bugged;
	}

	public bool isSpecial()
	{
		return mIsSpecial;
	}

	public void setSpecial(bool special)
	{
		mIsSpecial = special;

		//randomly assigns a type of special meal
		mSpecialType = (EnumSpecialMeal)(Random.Range (1, ((int)EnumSpecialMeal.NUM_OF_SPECIAL_TYPES - 1)));
	}

	public EnumSpecialMeal getTypeOfSpecialMeal()
	{
		return mSpecialType;
	}
}
