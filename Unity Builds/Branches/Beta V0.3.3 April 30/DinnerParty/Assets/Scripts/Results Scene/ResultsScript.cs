using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsScript : MonoBehaviour {

	public Text mSurvivorsText;
	public Text mWinnersText;

	private List<Player> mWinnersList = new List<Player> ();

	RestaurantScript mRestaurantScript;

	void Start () {
		mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();

		PopulateSurvivorsList ();
		PopulateWinnersList ();
	}
	
	public void OnExitClicked()
	{
		mRestaurantScript.resetGame();
		SceneManager.LoadScene(DinnerPartyScenes.TITLE_PATH);
	}

	void PopulateSurvivorsList()
	{
		List<Player> alivePlayers = mRestaurantScript.getAlivePlayers ();

		for (int i = 0; i < alivePlayers.Count; i++)
			mSurvivorsText.text += alivePlayers [i].getName () + "\n";
	}

	void PopulateWinnersList()
	{
		List<Player> allPlayers = mRestaurantScript.getAllPlayers ();
		List<Player> alivePlayers = mRestaurantScript.getAlivePlayers ();

		//finds the Wealthy Couple members
		Player WC1 = new Player("", EnumPlayerRole.PARTY_GOER);
		Player WC2 = new Player("", EnumPlayerRole.PARTY_GOER);

		for (int i = 0; i < allPlayers.Count; i++) {
			if (allPlayers [i].getRole () == EnumPlayerRole.WEALTHY_COUPLE) {
				if (WC1.getName() == "")
					WC1 = allPlayers [i];
				else
					WC2 = allPlayers [i];
			}
		}

		//find who won
		for (int i = 0; i < allPlayers.Count; i++) {
			switch (allPlayers [i].getRole ()) {
			case EnumPlayerRole.ASSASSIN:
				//if a WC died or kicked out
				if (!alivePlayers.Contains(WC1) || !alivePlayers.Contains(WC2))
					mWinnersList.Add (allPlayers [i]);
				break;
			case EnumPlayerRole.DISTANT_COUSIN:
				//if a WC died or kicked out
				if ((!alivePlayers.Contains(WC1) || !alivePlayers.Contains(WC2)) && alivePlayers.Contains(allPlayers[i]))
					mWinnersList.Add (allPlayers [i]);
				break;
			case EnumPlayerRole.WEALTHY_COUPLE:
				//if they survived
				if (alivePlayers.Contains(allPlayers[i]))
					mWinnersList.Add (allPlayers [i]);
				break;
			case EnumPlayerRole.CHEMIST:
				//if the WC survived and they survived
				if (alivePlayers.Contains(WC1) && alivePlayers.Contains(WC2) && alivePlayers.Contains(allPlayers[i]))
					mWinnersList.Add (allPlayers [i]);
				break;
			case EnumPlayerRole.FOOD_CRITIC:
				//if the WC survived and they survived
				if (alivePlayers.Contains(WC1) && alivePlayers.Contains(WC2) && alivePlayers.Contains(allPlayers[i]))
					mWinnersList.Add (allPlayers [i]);
				break;
			case EnumPlayerRole.SCAPEGOAT:
				//if they got voted out
				if (allPlayers [i].getVotedOut())
					mWinnersList.Add (allPlayers [i]);
				break;
			case EnumPlayerRole.PRIVATE_EYE:
				//if the assassin got voted out and they survived
				for (int j = 0; j < allPlayers.Count; j++) {
					if (allPlayers[j].getRole() == EnumPlayerRole.ASSASSIN && allPlayers[j].getVotedOut())
						mWinnersList.Add (allPlayers [i]);
				}
				break;
			}
		}

		for (int i = 0; i < mWinnersList.Count; i++) {
			mWinnersText.text += mWinnersList [i].getName () + " - " + mWinnersList[i].getRoleAsString() + "\n";
		}
	}
}
