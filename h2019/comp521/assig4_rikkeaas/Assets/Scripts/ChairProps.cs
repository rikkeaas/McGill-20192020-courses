using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairProps : MonoBehaviour
{
    private bool isOccupied = false;

    public void SetOccupied(bool occupation)
    {
        isOccupied = occupation;
    }

    public bool GetOccupied()
    {
        return isOccupied;
    }
    
}
