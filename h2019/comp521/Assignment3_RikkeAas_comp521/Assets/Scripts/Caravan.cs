using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caravan : MonoBehaviour
{

    private int[] spiceCounts = new int[7]{0,0,0,0,0,0,0};

    public int[] GetAllSpiceCounts()
    {
        return spiceCounts;
    }

    public int getSpiceCount(int spiceIdx)
    {
        return spiceCounts[spiceIdx];
    }

    public void UpdateSpices(int[] spicesToAdd)
    {
        for (int i = 0; i < 7; i++)
        {
            spiceCounts[i] += spicesToAdd[i];
        }
    }

    public bool HasSpices(int[] spicesToHave)
    {
        for (int i = 0; i < 7; i++)
        {
            if (spicesToHave[i] > spiceCounts[i]) return false;
        }
        return true;
    }

    // Method to let thief steal a random item
    // Returns false if caravan is empty (thief can't steal), true otherwise
    public bool StealRandomItem()
    {
        List<int> idxWithSpice = new List<int>();
        for (int i = 0; i < 7; i++)
        {
            if (spiceCounts[i] > 0) idxWithSpice.Add(i);
        }
        if (idxWithSpice.Count == 0) return false;

        int rand = UnityEngine.Random.Range(0,idxWithSpice.Count);
        spiceCounts[idxWithSpice[rand]] -= 1;
        return true;
    }
}
