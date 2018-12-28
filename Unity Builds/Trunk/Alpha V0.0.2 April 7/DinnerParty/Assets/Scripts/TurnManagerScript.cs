using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerScript : MonoBehaviour
{
    private List<Player> mActivePlayers;
    private List<EnumPlayerRole> mValidRoles;
    private int mCurrentPlayerIndex;
    private int mPlayerCount;
    private Player mMarkedPlayer;

    //Change later
    private Player mPlayerWithPoisonedMeal;

    public List<Player> getPlayers()
    {
        return mActivePlayers;
    }

    public void setPlayers(List<Player> players)
    {
        mActivePlayers = players;
    }

    public List<EnumPlayerRole> getValidRoles()
    {
        return mValidRoles;
    }

    public void setValidRoles(List<EnumPlayerRole> roles)
    {
        mValidRoles = roles;
    }

    public void setPlayerCount(int playerCount)
    {
        mPlayerCount = playerCount;
    }

    public int getPlayerCount()
    {
        return mPlayerCount;
    }

    public int getCurrentPlayerIndex()
    {
        return mCurrentPlayerIndex;
    }

    public void goToNextPlayer()
    {
        ++mCurrentPlayerIndex;
    }

    public void setMarkedPlayer(Player player)
    {
        mMarkedPlayer = player;
    }

    public Player getMarkedPlayer()
    {
        return mMarkedPlayer;
    }

    public void setPoisonPlayer(Player player)
    {
        mPlayerWithPoisonedMeal = player;
    }

    public Player getPoisonPlayer()
    {
        return mPlayerWithPoisonedMeal;
    }
}
