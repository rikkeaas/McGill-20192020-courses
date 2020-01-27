using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TraderHandler : MonoBehaviour
{
    // These are all the trader game objects. They do not change positions, but names and thus actions are assigned at runtime
    public GameObject traderA;
    public GameObject traderB;
    public GameObject traderC;
    public GameObject traderD;
    public GameObject traderE;
    public GameObject traderF;
    public GameObject traderG;
    public GameObject traderH;

    private List<GameObject> traderList = new List<GameObject>(); // List will contain traders in assigned order
    
    // To randomly allocate location to trader (actually just giving traders a random name)
    void Awake() // Using awake since other scripts depend on the assignment of traders, so this must be done first
    {
        List<GameObject> traders = new List<GameObject>{traderA, traderB, traderC, traderD, traderE, traderF, traderG, traderH};
    
        for (int i = 0; i < 8; i++)
        {
            int rand = UnityEngine.Random.Range(0, traders.Count - 1);
            GameObject currTrader = traders[rand];
            GameObject text = currTrader.transform.GetChild(0).GetChild(0).gameObject;
            text.GetComponent<Text>().text = "" + (i+1);
            currTrader.GetComponent<Trader>().SetName(i+1);
            traderList.Add(currTrader);
            traders.Remove(currTrader);
        }
    }

    // Returns the game object that has been assigned the given name this runthrough
    public GameObject GetTraderFromName(int traderName) 
    {
        return traderList[traderName - 1];
    }
}
