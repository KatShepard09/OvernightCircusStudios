using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
   public void GoToForestLevel1()
    {
        SceneManager.LoadScene("TheGame");
    }

    public void GoToCityLevel1()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
