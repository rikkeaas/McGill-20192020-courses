using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;
    private int inventoryCount;
    private int[] inventory;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        inventoryCount = 0;
        inventory = new int[7]{0,0,0,0,0,0,0};
    }

    // Method to make player move towards another game object (i.e. traders or the caravan)
    public void MoveTowards(GameObject destination)
    {
        navMeshAgent.SetDestination(destination.transform.position);
    }

    public void ChangeState(bool paused, float speed) 
    {
        if (paused) navMeshAgent.speed = 0;
        else navMeshAgent.speed = speed;
    }

    public void ChangeSpeed(float newSpeed)
    {
        navMeshAgent.speed = newSpeed;
    }

    public int[] GetInventory()
    {
        return inventory;
    }

    // Method to update state of the inventory of the player
    public void ChangeInventory(int[] newInventory)
    {
        int sum = 0;
        for (int i = 0; i < newInventory.Length; i++)
        {
            sum += newInventory[i];
        }
        if (sum > 4) Debug.Log("Somehow too much stuff in inventory.."); // This should not be possible

        inventory = newInventory;
        inventoryCount = sum;
    }

    // Method to let thief steal random item
    // Returns false if there is no item to steal, true otherwise
    public bool StealRandomItem()
    {
        List<int> idxWithSpice = new List<int>();
        for (int i = 0; i < 7; i++)
        {
            if (inventory[i] > 0) idxWithSpice.Add(i);
        }
        if (idxWithSpice.Count == 0) return false;

        int rand = UnityEngine.Random.Range(0,idxWithSpice.Count);
        inventory[idxWithSpice[rand]] -= 1;
        return true;
    }
}
