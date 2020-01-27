using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGenerator : MonoBehaviour
{
    public GameObject shopPrefab;
    public float startX;
    public float topRowZ, bottomRowZ;

    private List<GameObject> shops = new List<GameObject>();

    void Start()
    {
        GenerateShopRow(startX, topRowZ);
        GenerateShopRow(startX, bottomRowZ);
    }

    private void GenerateShopRow(float x, float z)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject shop = Instantiate(shopPrefab, new Vector3(x + 5*i, 1, z), Quaternion.identity);
            shop.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f);
            shop.transform.parent = transform;
            shops.Add(shop);
        }
    }

    public GameObject ChooseRandomShop() 
    {
        int shop = UnityEngine.Random.Range(0, 20);
        return shops[shop];
    }

    public List<GameObject> GetShops()
    {
        return shops;
    }
}
