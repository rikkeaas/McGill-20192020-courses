using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvertiserHandler : MonoBehaviour
{
    public GameObject advertiser;

    private float observationDist = 1f;
    private float pitchDist = 1f;
    private float adRate = 1f;
    private float adProb = 0.1f;

    private float spawnRate = 1;
 
    void Start()
    {
        spawnAdvertisers();
    }

    void Update()
    {
        spawnAdvertisers();
    }

    private void spawnAdvertisers() 
    {
        while (transform.childCount < spawnRate)
        {
            float advX = UnityEngine.Random.Range(-23f, 23f);
            float advZ = UnityEngine.Random.Range(-18f, 18f);

            while (advX >= -15 && advX <= 20 && advZ >= -8 && advZ <= 8) // Shouldn't spawn in the food court
            {
                advX = UnityEngine.Random.Range(-23f, 23f);
                advZ = UnityEngine.Random.Range(-18f, 18f);
            }

            GameObject newAdv = Instantiate(advertiser, new Vector3(advX, 2f, advZ), Quaternion.identity);
            newAdv.transform.parent = transform;
        }
    }

    public List<GameObject> GetAdvertisers()
    {
        List<GameObject> advertisers = new List<GameObject>();
        foreach (Transform adv in transform)
        {
            advertisers.Add(adv.gameObject);
        }
        return advertisers;
    }

    public float GetObservationDist()
    {
        return observationDist;
    }

    public float GetPitchDist()
    {
        return pitchDist;
    }

    public float GetAdRate()
    {
        return adRate;
    }

    public float GetAdProb()
    {
        return adProb;
    }

    public void ChangeSpawnRate(Slider slider)
    {
        spawnRate = slider.value;
    }

    public void ChangeObservationDist(Slider slider)
    {
        observationDist = slider.value;
    }

    public void ChangePitchDist(Slider slider)
    {
        pitchDist = slider.value;
    }

    public void ChangeAdRate(Slider slider)
    {
        adRate = slider.value;
    }

    public void ChangeAdProb(Slider slider)
    {
        adProb = slider.value;
    }
}
