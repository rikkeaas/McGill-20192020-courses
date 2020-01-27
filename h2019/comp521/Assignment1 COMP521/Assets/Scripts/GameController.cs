using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject key;
    public GameObject winText; // To display message when/if player wins and game ends
    public GameObject resetText; // To display message when maze is reset (makes it clear to player that they are back at start)

    private int floatingObjectsHit = 0;
    private int mazeTimeCount = 0;
    private Maze mazeScript;
    private PlayerController playerScript;

    void Start() 
    {
        mazeScript = GameObject.FindWithTag("Maze").GetComponent<Maze>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void RegisterHit() 
    {
        floatingObjectsHit++;
    }

    public int GetHits()
    {
        return floatingObjectsHit;
    }

    public void CreateKey(Vector3 position) 
    {
        Instantiate(key, position, Quaternion.identity);
    }

    public void RegisterKey()
    {
        GameObject.FindWithTag("UnlockableTree").SetActive(false);
    }

    public void RegisterTime()
    {
        if (mazeTimeCount > 16) // Player has used to many steps and will be asked to try again
        {
            RestartMaze();
            return;
        }
        if (mazeTimeCount > 0) resetText.SetActive(false); // Remove the reset text after player has made one move in resat maze
        mazeScript.TimeStep(mazeTimeCount); // Moving maze into the next time dimension
        mazeTimeCount++;
        mazeScript.IndicateNextStep(mazeTimeCount);
    }

    public void RestartMaze()
    {
        mazeScript.ResetMaze(); // Setting maze back to 0th time dimension
        playerScript.TeleportToStart(); // Moving player back to (0,0)
        mazeTimeCount = 0;
        resetText.SetActive(true); // Displaying message to player that they have been moved back to start
        RegisterTime(); // Regestering first (0th) "move" (moving to start (0,0) counts as a move)
    }

    public bool IsWinningTile((int,int) coords)
    {
        return mazeScript.IsWinningTile(coords.Item1, coords.Item2);
    }

    public void DisplayWinText()
    {
        winText.SetActive(true);
    }
} 
