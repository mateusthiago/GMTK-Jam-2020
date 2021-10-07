using System;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    Player_State playerState;
    static public event Action shieldBoost;
    private void Awake()
    {
        playerState = GetComponent<Player_State>();
    }


    void Update()
    {
        GetKeyPress();
    }

    void GetKeyPress()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            playerState.inControl = true;
        }
        else
        {
            playerState.inControl = false;
        }        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            shieldBoost.Invoke();
        }

        playerState.xAxis = Input.GetAxis("Horizontal");
        playerState.yAxis = Input.GetAxis("Vertical");
    }
}
