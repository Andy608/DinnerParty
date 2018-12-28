using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{
    public void OnStartClicked()
    {
        SceneManager.LoadScene(DinnerPartyScenes.SETUP_PATH);
    }
}
