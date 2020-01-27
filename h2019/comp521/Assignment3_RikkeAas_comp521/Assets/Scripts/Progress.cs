using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{

    public GameObject caravan;
    public GameObject player;

    private Caravan caravanScript;
    private Player playerScript;

    void Start()
    {
        caravanScript = caravan.GetComponent<Caravan>();
        playerScript = player.GetComponent<Player>();
    }

    void Update()
    {
        int[] currInventory = playerScript.GetInventory();
        for (int i = 1; i < 8; i++)
        {
            // The first getchild gets the row, the second gets the column of the state table.
            Text inv = transform.GetChild(i).GetChild(1).gameObject.GetComponent<Text>();
            Text car = transform.GetChild(i).GetChild(2).gameObject.GetComponent<Text>();
            inv.text = "" + currInventory[i-1];
            car.text = "" + caravanScript.getSpiceCount(i-1);

        }
    }
}
