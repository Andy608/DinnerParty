using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimerScript : MonoBehaviour {

	private RestaurantScript mRestaurantScript;

	const float TURN_TIME = 45;
	float mTimeLeft;
	bool mCountingDown;

	public Text mTimerText;

	void Start () {
		mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();

		mTimeLeft = TURN_TIME;
		mCountingDown = true;
	}

	void Update () {
		if (mCountingDown) {
			mTimeLeft -= Time.deltaTime;
			mTimerText.text = Mathf.RoundToInt(mTimeLeft).ToString ();
			if (mTimeLeft < 0) {
				mCountingDown = false;
				mRestaurantScript.RandomizeTurn ();
			}
		}
	}

	public void StopTimer()
	{
		mCountingDown = false;
	}

	public void ResumeTimer()
	{
		mCountingDown = true;
	}

	public void RestartTimer()
	{
		mCountingDown = true;
		mTimeLeft = TURN_TIME;
		mTimerText.text = mTimeLeft.ToString ();
	}
}
