using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopperHandler : MonoBehaviour
{
    public GameObject shopper;
    private float spawnRate = 1;

    void Start()
    {
        SpawnShoppers();
    }

    void Update()
    {
        SpawnShoppers();
    }

    // Method to spawn spawnRate shoppers at entrance to mall at random z values
    private void SpawnShoppers() 
    {
        while (transform.childCount < spawnRate) // Less shoppers than spawn rate
        {
            float shopperZ = UnityEngine.Random.Range(-10f, 10f);
            GameObject newShopper = Instantiate(shopper, new Vector3(-24f, 2f, shopperZ), Quaternion.identity);
            newShopper.transform.parent = transform;
        }
    }

    public List<GameObject> GetShoppers()
    {
        List<GameObject> shoppers = new List<GameObject>();
        foreach (Transform shopper in transform)
        {
            shoppers.Add(shopper.gameObject);
        }
        return shoppers;
    }

    // Method connected to slider in UI to change spawn rate of shoppers
    public void ChangeSpawnRate(Slider slider)
    {
        spawnRate = slider.value;
    }
}
