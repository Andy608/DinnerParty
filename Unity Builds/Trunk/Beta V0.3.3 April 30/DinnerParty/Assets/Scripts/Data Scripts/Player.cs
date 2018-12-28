using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumPlayerRole
{
    WEALTHY_COUPLE,
    ASSASSIN,
    DISTANT_COUSIN,
    CHEMIST,
    FOOD_CRITIC,
    PARTY_GOER,
	SCAPEGOAT,
	PRIVATE_EYE
}

public class Player
{
    public static List<EnumPlayerRole> sValidRoles = new List<EnumPlayerRole>();
    string mName;
    EnumPlayerRole mRole;
	EnumSpecialMeal mLastMealEaten;
	bool mIsMarked = false;
	bool mVotedOut = false;

    public Player(string name, EnumPlayerRole role)
    {
        mName = name;
        mRole = role;
    }

    public void setRole(EnumPlayerRole role)
    {
        mRole = role;
    }

    public string getName()
    {
        return mName;
    }

    public EnumPlayerRole getRole()
    {
        return mRole;
    }

    public override string ToString()
    {
        return "Name: " + mName + " | Role: " + getRoleAsString();
    }

    public string getRoleAsString()
    {
        string roleString = "UNKNOWN";
        switch (mRole)
        {
            case EnumPlayerRole.ASSASSIN:
                roleString = "Assassin";
                break;
            case EnumPlayerRole.CHEMIST:
                roleString = "Chemist";
                break;
            case EnumPlayerRole.DISTANT_COUSIN:
                roleString = "Distant Cousin";
                break;
            case EnumPlayerRole.FOOD_CRITIC:
                roleString = "Food Critic";
                break;
            case EnumPlayerRole.PARTY_GOER:
                roleString = "Party Goer";
                break;
            case EnumPlayerRole.WEALTHY_COUPLE:
                roleString = "Wealthy Couple";
                break;
			case EnumPlayerRole.SCAPEGOAT:
				roleString = "Scapegoat";
				break;
			case EnumPlayerRole.PRIVATE_EYE:
				roleString = "Private Eye";
				break;
        }

        return roleString;
    }

    public string getRoleAsStringWithPrefix()
    {
        string roleString = "UNKNOWN";
        switch (mRole)
        {
            case EnumPlayerRole.ASSASSIN:
                roleString = "the Assassin";
                break;
            case EnumPlayerRole.CHEMIST:
                roleString = "the Chemist";
                break;
            case EnumPlayerRole.DISTANT_COUSIN:
                roleString = "the Distant Cousin";
                break;
            case EnumPlayerRole.FOOD_CRITIC:
                roleString = "the Food Critic";
                break;
            case EnumPlayerRole.PARTY_GOER:
                roleString = "a Party Goer";
                break;
            case EnumPlayerRole.WEALTHY_COUPLE:
                roleString = "part of the Wealthy Couple";
                break;
			case EnumPlayerRole.SCAPEGOAT:
				roleString = "the Scapegoat";
				break;
			case EnumPlayerRole.PRIVATE_EYE:
				roleString = "the Private Eye";
				break;
        }

        return roleString;
    }

	public string getSpecialMealAsString()
	{
		string mealString = "UNKNOWN";
		switch (mLastMealEaten)
		{
		case EnumSpecialMeal.SEER:
			mealString = "Seer";
			break;
		case EnumSpecialMeal.TRICKSTER:
			mealString = "Trickster";
			break;
		case EnumSpecialMeal.BLINDNESS:
			mealString = "Blindness";
			break;
		case EnumSpecialMeal.STOMACHACHE:
			mealString = "Stomach Ache";
			break;
		}

		return mealString;
	}

	public void setLastMealEaten(EnumSpecialMeal specialMeal)
	{
		mLastMealEaten = specialMeal;
	}

	public EnumSpecialMeal getLastMealEaten()
	{
		return mLastMealEaten;
	}

	public bool getMarked()
	{
		return mIsMarked;
	}

	public void setMarked(bool isMarked)
	{
		mIsMarked = isMarked;
	}

	public bool getVotedOut()
	{
		return mVotedOut;
	}

	public void setVotedOut(bool wasVotedOut)
	{
		mVotedOut = wasVotedOut;
	}
}
