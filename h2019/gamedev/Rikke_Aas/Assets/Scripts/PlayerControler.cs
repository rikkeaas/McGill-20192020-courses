using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int count;
    private int speedBoost;

    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
    }

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed * speedBoost);

        speedBoost = 1;
        if (transform.position.y < 0) 
        {
            Restart ();
        }
    }

    void OnTriggerEnter (Collider other) 
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive (false);
            count = count + 1;
            SetCountText();
        }
        if (other.gameObject.CompareTag("Speed Boost"))
        {
            speedBoost = 40;
        }
    }

    void SetCountText ()
    {
        countText.text = "Count: " + count.ToString ();
        if (count >= 12) {
            winText.text = "You Win!";
        }
    }

    void Restart () {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene (scene.name);
    }

}

