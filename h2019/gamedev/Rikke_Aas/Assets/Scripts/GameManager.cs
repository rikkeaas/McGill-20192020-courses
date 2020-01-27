using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    public Rotator pickup;
    public int nbOfPickUps;
    public int mazeX, mazeY;

    private List<Rotator> pickups;
    private Maze mazeInstance;

    // Start is called before the first frame update
    void Start()
    {
        BeginGame ();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame ();
        }
    }

    void BeginGame () 
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Generate(mazeX, mazeY);
        pickups = new List<Rotator>();

        for (int i = 0; i < nbOfPickUps; i++)
        {
            int x = (Random.Range(0, mazeX) - mazeX/2)*2;
            int y = (Random.Range(0, mazeY) - mazeY/2)*2;
            bool alreadyUsed = false;

            foreach (Rotator pickupBlock in pickups)
            {
                if (pickupBlock.transform.position[0] == x+1f && pickupBlock.transform.position[2] == y+1f) {
                    i--;
                    alreadyUsed = true;
                    break;
                }
            }

            if (!alreadyUsed) {
                Rotator newPickup = Instantiate(pickup) as Rotator;
                newPickup.name = "Pickup " + x + ", " + y;
                newPickup.transform.parent = transform;
                newPickup.transform.localPosition = new Vector3(x + 1f, 0.5f, y + 1f);
                pickups.Add(newPickup);
            }
        }

    }

    void RestartGame () 
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene (scene.name);
        // Destroy(mazeInstance.gameObject);
        // BeginGame ();
    }
}
