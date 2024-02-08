using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChanged;//event for the enemy to end its turn.
    }

    void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn())//if it is the players turn the AI will do nothing.
        {
            return;
        }

        timer -= Time.deltaTime;// temp code just so the enemy ends its turn in given time.
        if(timer <= 0)
        {
            TurnSystem.Instance.NextTurn();
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)//listener for enemy to end its turn.
    {
        timer = 2f;
    }
}
