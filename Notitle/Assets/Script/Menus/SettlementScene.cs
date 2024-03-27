using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettlementScene : MonoBehaviour
{
    public void GoToSettlement()
    {
        SceneManager.LoadScene("SettlementPhase");
    }
}
