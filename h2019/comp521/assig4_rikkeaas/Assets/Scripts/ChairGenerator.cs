using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairGenerator : MonoBehaviour
{

    public GameObject chair;

    private List<GameObject> chairs;

    void Start()
    {
        chairs = new List<GameObject>();

        double radiansPerChair = ( Math.PI * 2) / 10f;

        for (int i = 0; i < 10; i++)
        {
            float prob = UnityEngine.Random.Range(0f, 1f);
            if (prob < 0.5) // 50% change of generating a chair for each slot around the table
            {
                float chairX = transform.position.x + (float) Math.Cos(radiansPerChair * i) * 2.5f;
                float chairZ = transform.position.z + (float) Math.Sin(radiansPerChair * i) * 2.5f;
                GameObject nextChair = Instantiate(chair, new Vector3(chairX, 0.25f, chairZ), Quaternion.identity);
                nextChair.transform.parent = transform;
                chairs.Add(nextChair);
            }
            
        }
        if (chairs.Count == 0) // If by some miracle chance no chairs have been generated, we generate one in the first slot
        {
            float chairX = transform.position.x + (float) Math.Cos(radiansPerChair) * 2.5f;
            float chairZ = transform.position.z + (float) Math.Sin(radiansPerChair) * 2.5f;
            GameObject nextChair = Instantiate(chair, new Vector3(chairX, 0.25f, chairZ), Quaternion.identity);
            nextChair.transform.parent = transform;
            chairs.Add(nextChair);
        }
    }

    public List<GameObject> GetChairs()
    {
        return chairs;
    }
}
