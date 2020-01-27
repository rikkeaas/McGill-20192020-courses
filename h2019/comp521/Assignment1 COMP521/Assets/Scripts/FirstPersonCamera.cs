using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position; // We want to keep this distance constant so we save it as field
    }

    void LateUpdate()
    {
        float x = transform.eulerAngles.x;
        float y = transform.eulerAngles.y;
        x -= Input.GetAxis("Mouse Y") * rotationSpeed;
        y += Input.GetAxis("Mouse X") * rotationSpeed;

        transform.eulerAngles = new Vector3(x, y, 0.0f); // We don't ever want camera to rotate in z-direction so this is always 0
        player.transform.eulerAngles = new Vector3(0.0f, y, 0.0f); // Turning player so that moving will be syncronized with view

        transform.position = player.transform.position + offset; // Moving camera so that it's always in the same place relative to player
    }
}
