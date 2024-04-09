using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScene : MonoBehaviour
{
   public void GoToMap()
    {
        SceneManager.LoadScene("MAP");
    }

    public void GoToExploration()
    {
        int randomSceneIndex = Random.Range(7, 9); // This will generate a random number between 7 (inclusive) and 9 (exclusive)
        string sceneName = "Exploration" + randomSceneIndex;
        SceneManager.LoadScene(sceneName);
    }
}
