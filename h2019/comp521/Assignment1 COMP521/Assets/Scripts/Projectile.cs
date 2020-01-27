using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed; // Speed of projectile is set in Unity

    private Rigidbody rb;
    private GameController gameController; // Need this so that we can count the number of boxes hit by projectiles
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed); // Speed for the projectile is constant so we set it here

        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        // Destroying projectile if it is outside the boundaries of the game space
        if (transform.position.y <= 0.1 || transform.position.y > 30) 
        {
            Destroy(transform.gameObject);
        }
        if (transform.position.x <= -40 || transform.position.x > 40) 
        {
            Destroy(transform.gameObject);
        }
        if (transform.position.z <= -40 || transform.position.z > 40)
        {
            Destroy(transform.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FloatingObject"))
        {
            gameController.RegisterHit(); // GameController keeps track of how many floating objects player has hit
            if (gameController.GetHits() == 3) 
            {
                gameController.CreateKey(transform.position); // Creates key at the position of the third box hit
            }
            Destroy(transform.gameObject);
            Destroy(other.transform.gameObject);
        }
    }

    void OnCollisionEnter()
    {
        Destroy(transform.gameObject);
    }
}
