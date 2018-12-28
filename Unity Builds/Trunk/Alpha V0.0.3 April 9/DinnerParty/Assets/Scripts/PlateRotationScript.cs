using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateRotationScript : MonoBehaviour {

	float rotSpeed = 300;
	public float currentAngle = 0;
	public float trackedAngle = 0;

    //private List<Player> mPlayerList;
	public float player = 0;

    private float mAngleBetweenPlayers;

	private TurnManagerScript mTurnManagerScript;
    private RestaurantScript mRestaurantScript;

    public float mSensitivity = 0.4f;
    private Vector3 mMouseReference;
    private Vector3 mMouseOffset;
    private Vector3 mRotation;
    private bool mIsRotating;

    private float mAccumulatedAngle;

    private float mPrevRotation;

    void Start()
    {
		mTurnManagerScript = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>();
        mRestaurantScript = GameManagerScript.GetInstance().GetComponent<RestaurantScript>();
        //mPlayerList = GameManagerScript.GetInstance().GetComponent<TurnManagerScript>().getPlayers();
        mAngleBetweenPlayers = 360.0f / mRestaurantScript.getAlivePlayers().Count;

        mRotation = Vector3.zero;
        mPrevRotation = 0;
        mAccumulatedAngle = 0;
    }

	/*void OnMouseDrag()
	{
		trackedAngle += Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;

		if (Mathf.Abs(trackedAngle) >= mAngleBetweenPlayers)
        {
			//currentAngle += trackedAngle;
			if (trackedAngle > 0)
			{
                mRestaurantScript.RotatePlatterRight();
				trackedAngle = mAngleBetweenPlayers;
                GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
			}
			else
			{
                mRestaurantScript.RotatePlatterLeft();
				trackedAngle = -mAngleBetweenPlayers;
                GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
            }
			
			transform.Rotate(Vector3.forward, -trackedAngle);

			trackedAngle = 0;
			currentAngle = transform.eulerAngles.z;
			//currentAngle = currentAngle * Mathf.Deg2Rad;
		}
	}*/

    void Update()
    {
        if (mIsRotating)
        {
            //mMouseOffset = (Input.mousePosition - mMouseStartPosition);

            //Vector3 startToOrigin = Vector3.Normalize(Camera.main.WorldToScreenPoint(transform.position) - mMouseStartPosition);
            //Vector3 startToEnd = Vector3.Normalize(Input.mousePosition - mMouseStartPosition);

            //Debug.Log(Vector3.Dot(startToOrigin, startToEnd));

            //if (Vector3.Dot(startToOrigin, startToEnd) > 0)
            //{
            //    mAccumulatedAngle += mMouseOffset.x * mSensitivity;
            //    mRotation.z = mMouseOffset.x * mSensitivity;
            //}
            //else
            //{
            //    mAccumulatedAngle += -mMouseOffset.x * mSensitivity;
            //    mRotation.z = -mMouseOffset.x * mSensitivity;
            //}

            //transform.Rotate(mRotation);

            //mMouseStartPosition = Input.mousePosition;

            //if (mAccumulatedAngle > mAngleBetweenPlayers)
            //{
            //    mRestaurantScript.RotatePlatterLeft();
            //    mPrevRotation += mAngleBetweenPlayers;
            //    mAccumulatedAngle = 0;
            //    GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
            //}
            //else if (mAccumulatedAngle < -mAngleBetweenPlayers)
            //{
            //    mRestaurantScript.RotatePlatterRight();
            //    mPrevRotation += mAngleBetweenPlayers;
            //    mAccumulatedAngle = 0;
            //    GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
            //}

            mMouseOffset = (Input.mousePosition - mMouseReference);

            if (Camera.main.WorldToScreenPoint(transform.position).y > Input.mousePosition.y)
            {
                mAccumulatedAngle += mMouseOffset.x * mSensitivity;
                mRotation.z = mMouseOffset.x * mSensitivity;
            }
            else
            {
                mAccumulatedAngle += -mMouseOffset.x * mSensitivity;
                mRotation.z = -mMouseOffset.x * mSensitivity;
            }

            transform.Rotate(mRotation);

            mMouseReference = Input.mousePosition;

            if (mAccumulatedAngle > mAngleBetweenPlayers)
            {
                mRestaurantScript.RotatePlatterLeft();
                mPrevRotation += mAngleBetweenPlayers;
                mAccumulatedAngle = 0;
                GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
            }
            else if (mAccumulatedAngle < -mAngleBetweenPlayers)
            {
                mRestaurantScript.RotatePlatterRight();
                mPrevRotation += mAngleBetweenPlayers;
                mAccumulatedAngle = 0;
                GameObject.Find("GameplayManager").GetComponent<StartGameScript>().UpdateMealColors();
            }
        }
    }

    void OnMouseDown()
    {
        mIsRotating = true;
        mMouseReference = Input.mousePosition;
    }

	void OnMouseUp()
	{
        mIsRotating = false;
		player = currentAngle;

        Vector3 rotation = transform.eulerAngles;
        rotation.z = mPrevRotation;
        transform.eulerAngles = rotation;
    }
}
