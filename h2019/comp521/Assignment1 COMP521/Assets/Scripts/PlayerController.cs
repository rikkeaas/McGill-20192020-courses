using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;
    public GameObject projectilePrefab;

    private GameController gameController;
    private GameObject newProjectile; // Saving projectile gameObject to know when it has been destroyed to ensure only one projectile at a time
    private Rigidbody rb;
    private bool onGround = true; // State to know if player is currently jumping
    private bool mazeMode = false;
    private bool hasWon = false;
    private List<Vector3> visitedMazeCells;

    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        visitedMazeCells = new List<Vector3>();
    }

    void FixedUpdate ()
    {
        if (hasWon) return; // If player has won, they shouldn't be able to do anything so we return directly
        StraightenPlayer();
        DoMovement();
        if (visitedMazeCells.Count > 0 && Input.GetKeyDown(KeyCode.Escape)) gameController.RestartMaze(); 
        if (mazeMode && onGround) rb.velocity = Vector3.zero; // To avoid forces when crashing into maze walls
        if (newProjectile == null && Input.GetMouseButton(0) && !mazeMode) // Can't fire projectiles in maze
        {
            // Adding to initial position to avoid "collision" between player and projectile
            newProjectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
        }
    }

    void StraightenPlayer()
    {
        // Making sure player can't fall over
        Vector3 temp = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler (0, temp.y, 0);
    }

    void DoMovement() 
    {
        if (mazeMode && !onGround) return; // Can't move while jumping in maze
        // Can use both arrow keys and WASD (because I'm left handed)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-Vector3.right * Time.deltaTime * speed);
        }
        if (onGround && Input.GetKeyDown(KeyCode.Space)) 
        {
            onGround = false; // So we can't jump again while still in air
            rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // To know that we have landed again after jumping
        if (collisionInfo.collider.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish")) // Player has reached end of maze and game is over
        {
            hasWon = true;
            gameController.DisplayWinText();
        }

        if (other.gameObject.CompareTag("Key"))
        {
            Destroy(other.gameObject.transform.parent.gameObject); // Destroying both the "trigger key" and the solid key object
            gameController.RegisterKey();
        }

        if (other.gameObject.CompareTag("MazeCell"))
        {
            onGround = true; // We have landed again after jumping in maze
            if (visitedMazeCells.Count > 0 && visitedMazeCells[visitedMazeCells.Count-1] == other.gameObject.transform.position) 
                // Don't step in time dimension if player has not gone to a new maze cell (ex after jumping)
                return;
            visitedMazeCells.Add(other.gameObject.transform.position);
            transform.position = visitedMazeCells[visitedMazeCells.Count-1];
            gameController.RegisterTime(); // Count one step/time unit, gamecontroller makes maze step into next time dimension

            if (!mazeMode)
            {
                mazeMode = true;
                jumpSpeed *= 1.5f; // Jump higher in maze to be able to observe structure
            }
        }
    }

    public void TeleportToStart()
    {
        transform.position = visitedMazeCells[0]; // Moving back to start position (aka the first maze cell player visited)
        visitedMazeCells.RemoveRange(1, visitedMazeCells.Count - 1);
    }
}
