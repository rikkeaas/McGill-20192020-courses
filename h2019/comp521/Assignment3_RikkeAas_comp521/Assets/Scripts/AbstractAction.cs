using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAction : MonoBehaviour
{
    private int[] preInventoryCondition = new int[7];
    private int[] postInventory = new int[7];
    private int[] postCaravan = new int[7];
    private int maxSizeOfInventory = 4;
    private GameObject actionObject;

    // Method to check if player has the right spices in the inventory, and the inventory is not full
    public virtual bool PreConditionsSatisfied(int[] preInventory)
    {
        int inventorySum = 0;
        for (int i = 0; i < preInventory.Length; i++)
        {
            inventorySum += preInventory[i];
            if (preInventoryCondition[i] > preInventory[i]) return false;
        }
        return inventorySum <= maxSizeOfInventory;
    }

    // Method to check if player is in range of the corresponding trader or caravan
    public bool IsInRange(Transform playerTransform)
    {
        float distance = Vector3.Distance(playerTransform.position, actionObject.transform.position);
        return distance <= 2f; // Acceptable distance for player to be in range of trader
    }

    // Method to do transaction, deposit or reclaim.
    // It takes the inventory of the player and returns the new world state (updated inventory and changes made to caravan) 
    public (int[], int[]) DoAction(int[] preInventory)
    {
        int[] inventory = new int[7];
        int[] caravan = new int[7];
        for (int i = 0; i < 7; i++)
        {
            inventory[i] = preInventory[i] + postInventory[i];
            caravan[i] = postCaravan[i];
        }
        return (inventory, caravan);
    }

    public void SetActionObject(GameObject actionObject)
    {
        this.actionObject = actionObject;
    }

    public void SetPreInventoryConditions(int[] preConds) 
    {
        for (int i = 0; i < 7; i++)
        {
            preInventoryCondition[i] = preConds[i];
        }
    }

    public void SetPostInventory(int[] postInv)
    {
        for (int i = 0; i < 7; i++)
        {
            postInventory[i] = postInv[i];
        }
    }

    public void SetPostCaravan(int[] postCar)
    {
        for (int i = 0; i < 7; i++)
        {
            postCaravan[i] = postCar[i];
        }
    }

    public void SetMaxInventorySize(int size) 
    {
        maxSizeOfInventory = size;
    }

    public int[] GetPreInventoryCondition()
    {
        return preInventoryCondition;
    }

    public int[] GetPostInventory()
    {
        return postInventory;
    }

    public int[] GetPostCaravan()
    {
        return postCaravan;
    }

    public GameObject GetActionObject()
    {
        return actionObject;
    }

    // For deposit and reclaim actions this will be overridden again
    public override string ToString()
    {
        string actionName = "Trade with trader ";
        actionName += actionObject.GetComponent<Trader>().GetName();
        return actionName;
    } 
}
