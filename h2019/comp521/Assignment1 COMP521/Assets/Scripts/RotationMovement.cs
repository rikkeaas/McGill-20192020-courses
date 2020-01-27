using UnityEngine;
using System;

public class RotationMovement : MonoBehaviour
{
    private Vector3 rotation;

    void Start() 
    {
        rotation = new Vector3(TweekRotation(5), TweekRotation(10), TweekRotation(15));
    }

    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }

    
    int TweekRotation(int nb) 
    {
        // Method to make each rotation movement slightly different so that objects using this script won't rotate in unison
        float randomOffset = UnityEngine.Random.Range(0.5f, 1.5f);
        return (int) Math.Round(nb * randomOffset);
    }
}
