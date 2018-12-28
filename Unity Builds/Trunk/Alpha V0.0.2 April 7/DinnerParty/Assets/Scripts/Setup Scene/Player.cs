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
    PARTY_GOER
}


public class Player
{
    string mName;
    EnumPlayerRole mRole;

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
        }

        return roleString;
    }
}
