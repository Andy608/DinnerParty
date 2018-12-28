using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoteTimerScript : MonoBehaviour {

	const float FULL_TIME = 60;
	float mTimeLeft;
	bool mCountingDown;
	bool mVotingEnabled;

	public Text mTimerText;

	void Start () {
		mTimeLeft = FULL_TIME;
		mCountingDown = false;
		mVotingEnabled = false;
	}

	void Update () {
		if (mCountingDown) {
			mTimeLeft -= Time.deltaTime;
			mTimerText.text = Mathf.RoundToInt(mTimeLeft).ToString ();
			if (!mVotingEnabled && mTimeLeft <= 30) {
				gameObject.GetComponent<VoteScript> ().ActivateAllMeals ();
				mVotingEnabled = true;
			}

			if (mTimeLeft <= 0) {
				GameManagerScript.GetInstance().GetComponent<TurnManagerScript>().GoToNextRound();
			}
		}
	}

	public void TurnOnTimer()
	{
		mCountingDown = true;
	}
}
