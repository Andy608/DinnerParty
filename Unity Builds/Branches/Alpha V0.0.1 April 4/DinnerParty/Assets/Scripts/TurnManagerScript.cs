using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerScript : MonoBehaviour
{
    private List<Player> mActivePlayers;
    private int mCurrentPlayerIndex;

    public List<Player> getPlayers()
    {
        return mActivePlayers;
    }

    public void setPlayers(List<Player> players)
    {
        mActivePlayers = players;
    }

    public int getCurrentPlayerIndex()
    {
        return mCurrentPlayerIndex;
    }

    public void goToNextPlayer()
    {
        ++mCurrentPlayerIndex;
    }
}
