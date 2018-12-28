using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumSpecialMeal
{
	SEER = 0,
	TRICKSTER = 1,
	STOMACHACHE = 2,
	BLINDNESS = 3,
	NUM_OF_SPECIAL_TYPES = 4
}

public class Meal
{
    bool mIsPoisoned;
	bool mIsSpecial;
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

	public bool isSpecial()
	{
		return mIsSpecial;
	}

	public void setSpecial(bool special)
	{
		mIsSpecial = special;

		//randomly assigns a type of special meal
		mSpecialType = (EnumSpecialMeal)(Random.Range (0, ((int)EnumSpecialMeal.NUM_OF_SPECIAL_TYPES - 1)));
	}

	public EnumSpecialMeal getTypeOfSpecialMeal()
	{
		return mSpecialType;
	}
}
