using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Thief : MonoBehaviour
{
    public GameObject player;
    public GameObject caravan;

    private NavMeshAgent navMeshAgent;
    private Vector3 destination;
    private float timeStamp = -1;
    private bool onThievingMission = false; // True when thief is currently moving towards target or stealing
    private bool playerIsTarget = false; // False means thief wil steal from carvan
    private int stolenCount = 0;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        Vector3 start = GenerateRandTransform();
        while (IsInCaravanSpace(start)) // Don't want thief to start in caravan space
        {
            start = GenerateRandTransform();
        }
        transform.position = start;
        destination = start;
    }

    // Method to generate a random destination to make thief wander randomly
    private Vector3 GenerateRandTransform()
    {
        float x = UnityEngine.Random.Range(-8f, 8f);
        float z = UnityEngine.Random.Range(-8f, 8f);
        return new Vector3(x, 0.5f, z);
    }

    private bool IsInCaravanSpace(Vector3 pos) 
    {
        return pos.x >= -2 && pos.x <= 2 && pos.z >= -2 && pos.z <= 2;
    }

    void Update()
    {
        if (onThievingMission)
        {
            if (AtDestination()) // Thief has reached target and can commence stealing
            {
                bool stole; // Need to know if stealing was successfull
                if (playerIsTarget) stole = player.GetComponent<Player>().StealRandomItem();
                else stole = caravan.GetComponent<Caravan>().StealRandomItem();
                if (stole) stolenCount++; // Count nb of items thief has stolen
                onThievingMission = false; // Thief has completed thieving mission and is now "idle"
            }
            else if (playerIsTarget) // Only need to update position if player is the target, since caravan doesn't move
            {
                destination = player.transform.position;
            }
        }
        else if (timeStamp == -1 && !onThievingMission) // Thieving missions should happen every 5s, so we need to count time
        {
            timeStamp = Time.time;
        }
        else if (Time.time - timeStamp >= 5 && stolenCount < 2) 
        {
            float rand = UnityEngine.Random.Range(0f,1f);
            if (rand < 0.33) // 33% chance of thief going on new thieving mission
            {
                GoOnTheivingMission();
                onThievingMission = true;
            }
            timeStamp = -1; // Resetting timestamp regardless of wether thief will be thieving or not
        }
        else if (AtDestination() || IsInCaravanSpace(destination)) // This is to generate new random destionations when thief is not on mission
        {
            destination = GenerateRandTransform();
        } 
        navMeshAgent.SetDestination(destination); 
    }

    private void GoOnTheivingMission()
    {
        float rand = UnityEngine.Random.Range(0f,1f);
        if (rand < 0.5f) // 50% chance of stealing from caravan, 50% of stealing from player
        {
            Debug.Log("Thief will steal from caravan");
            destination = caravan.transform.position;
            playerIsTarget = false;
        }
        else
        {
            Debug.Log("Thief will steal from player");
            destination = player.transform.position;
            playerIsTarget = true;
        }
    }

    private bool AtDestination() 
    {
        float distance = Vector3.Distance(destination, transform.position);
        return distance <= 1.5f;
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
}
