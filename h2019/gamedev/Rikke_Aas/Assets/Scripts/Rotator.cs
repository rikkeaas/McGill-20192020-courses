using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    private Vector3 rotation;

    void Start() 
    {
        rotation = new Vector3 (TweekRotation(15), TweekRotation(30), TweekRotation(45));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (rotation * Time.deltaTime);
    }

    int TweekRotation (int nb) {
        float randomOffset = UnityEngine.Random.Range(0.5f, 1.5f);
        return (int) Math.Round (nb * randomOffset);
    }
}

