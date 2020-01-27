using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject player;
    public GameObject thief;

    private bool gamePaused = false;
    private float gameSpeed = 2f;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log("game paused");
            gamePaused = !gamePaused;
            player.GetComponent<Player>().ChangeState(gamePaused, gameSpeed);
            thief.GetComponent<Thief>().ChangeState(gamePaused, gameSpeed);

        }
        else if (!gamePaused)
        {
            // Plus is actually shift + equals (on my keyboard at least)
            if (Input.GetKeyDown(KeyCode.Equals) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                gameSpeed *= 2;
                Debug.Log("game sped up");
            }
            if (Input.GetKeyDown(KeyCode.Minus))
            {
                gameSpeed /= 2;
                Debug.Log("game slowed down");
            }
            player.GetComponent<Player>().ChangeSpeed(gameSpeed);
            thief.GetComponent<Thief>().ChangeSpeed(gameSpeed);
        }
    }

    public bool getGameState() 
    {
        return gamePaused;
    }

    public float getGameSpeed()
    {
        return gameSpeed;
    }
}
