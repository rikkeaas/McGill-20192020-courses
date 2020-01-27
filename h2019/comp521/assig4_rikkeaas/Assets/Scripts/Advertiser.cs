using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Advertiser : MonoBehaviour
{
    public GameObject flyer;

    private ObstacleHandler obstacleHandler;
    private AdvertiserHandler advertiserHandler;
    private GameObject target = null;
    private GameObject closestObstacle = null;
    private float goalX;
    private float goalZ;
    private Rigidbody rb;

    private int successfulPichCount = 0;
    private float adTimeCounter = -1;
    private float pitchTimeCounter = -1;
    private float pursuitTimeCounter = -1;
    private float newGoalCounter = 0;

    private float maxVelocity = 0f;
    private float objectAvoidanceLength = 5;

    void Start()
    {
        maxVelocity = 9f + UnityEngine.Random.Range(-1f, 1f); // Velocity slightly varying between advertisers
        obstacleHandler = GameObject.Find("ObstacleHandler").GetComponent<ObstacleHandler>();
        advertiserHandler = GameObject.Find("AdvertiserHandler").GetComponent<AdvertiserHandler>();
        ChooseRandomGoal(); 
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (target == null) // No shopper targeted yet
        {
            LookForTarget(); // See if any flyered shoppers close by
            if (newGoalCounter >= 1f) // Choose random goal locations to implement wandering
            {
                ChooseRandomGoal();
                newGoalCounter = 0;
            }
            else newGoalCounter += Time.deltaTime;

            if (adTimeCounter == -1) adTimeCounter = 0; // Start counter for dropping ad
            else if (adTimeCounter < advertiserHandler.GetAdRate()) adTimeCounter += Time.deltaTime; // Increment counter for ad dropping
            else // Counter for dropping ad done, drop ad at location and reset counter
            {
                DropFlyer();
                adTimeCounter = -1;
            }
        }
        else // Advertiser currently has a shopper targeted
        {   
            adTimeCounter = -1; // Reset ad counter, advertiser can't drop ads while pursuing shopper

            if (pitchTimeCounter >= 4f) // Advertiser has successfully pitched to targeted shopper
            {
                RegisterSuccessfulPitch();
                pursuitTimeCounter = -1;
                pitchTimeCounter = -1;
                target = null; // "Release" shopper
            }
            else if (pursuitTimeCounter >= 5f) // Abandonning pursuit, timeout
            {
                target = null;
                pursuitTimeCounter = -1;
                pitchTimeCounter = -1;
            }
            else if (pursuitTimeCounter == -1) // Start timer for pursuit
            {
                pursuitTimeCounter = 0;
                if (MyDistance(transform.position, target.transform.position) <= advertiserHandler.GetPitchDist()) // Only start if within pitching range
                {
                    pitchTimeCounter = 0;
                }
            }
            else
            {
                pursuitTimeCounter += Time.deltaTime; // Always increment pursuit timer
                if (MyDistance(transform.position, target.transform.position) <= advertiserHandler.GetPitchDist())
                {
                    pitchTimeCounter += Time.deltaTime; // Only increment pitch timer if in range of shopper
                }
            } 
            // Set velocity to zero if the target is flyered (immobile) and advertiser is very close (so that it doesn't push shopper)
            if (target != null && target.GetComponent<SteeringBehaviour>().IsFlyered() && MyDistance(transform.position, target.transform.position) <= 1.1f) 
            {
                rb.velocity = Vector3.zero;
                return; // No forces should be added
            }
        }

        // Calculating steering force, adding it to rigid body and rotating advertiser to face movement direction
        Vector3 combined = CombineForces(PursuitForce(), AvoidanceForce(), EvasionForce());
        rb.AddForce(combined, ForceMode.VelocityChange);
        transform.rotation = Quaternion.LookRotation(combined);

        // Advertiser shouldn't move out of mall area
        if (transform.position.x > 24)
        {
            transform.position = new Vector3(24, transform.position.y, transform.position.z);
        }

        if (transform.position.x < -24)
        {
            transform.position = new Vector3(-24, transform.position.y, transform.position.z);
        }
    }

    // Method to calculate distance in x-z-plane
    private float MyDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt((a.x - b.x)*(a.x - b.x) + (a.z - b.z)*(a.z - b.z));
    }

    // Method to combine different forces to one steering force
    private Vector3 CombineForces(Vector3 pursuitForce, Vector3 avoidForce, Vector3 evasionForce)
    {
        if (target != null) pursuitForce *= 5; // Seek force a lot more weighted if advertiser is pursuing a shopper
        if (closestObstacle == null) // No obstacle to collide with so disregarding avoidance force
        {
            return (pursuitForce + evasionForce).normalized * maxVelocity;
        }
        else // Similar to shoppers combining of forces (see commentary in SteeringBehaviour.cs)
        {
            float distObstacle = Vector3.Distance(transform.position, closestObstacle.transform.position);
            if (target != null) 
            {
                float distTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distTarget < distObstacle) 
                {
                    return (pursuitForce);
                }
            }
            
            float weight = 2f/distObstacle;
            Vector3 newForce;
            if (weight > 1) 
            {
                newForce = (avoidForce + evasionForce).normalized * maxVelocity;
            }
            else
            {
                newForce = avoidForce * weight;
                newForce += pursuitForce * (1 - weight);
                newForce = (newForce + evasionForce).normalized * maxVelocity;
            }
            return newForce;
        }
    }

    // Method to calculate the pursuit force
    private Vector3 PursuitForce() 
    {
        float desiredX;
        float desiredZ;
        if (target != null) // Pursuing shopper
        {
            desiredX = target.transform.position.x - transform.position.x;
            desiredZ = target.transform.position.z - transform.position.z;
        }
        else // "Pursuing"/seeking a random location in the mall
        {
            desiredX = goalX - transform.position.x;
            desiredZ = goalZ - transform.position.z;
        }

        Vector3 normalDesired = new Vector3(desiredX, 0, desiredZ);
        normalDesired.Normalize();
        normalDesired *= maxVelocity;

        Vector3 pursuitForce = normalDesired - rb.velocity;
        pursuitForce.y = 0f;

        return pursuitForce;
    }

    // Same as the avoidance force for shoppers (see comments in SteeringBehaviour.cs)
    private Vector3 AvoidanceForce()
    {
        Vector3 nextPos = transform.position + rb.velocity.normalized * objectAvoidanceLength * 0.5f;
        Vector3 nextNextPos = transform.position + rb.velocity.normalized * objectAvoidanceLength;

        GameObject closestObstacle = obstacleHandler.FindClosestObstacle(transform.position, nextPos, nextNextPos, target);
        this.closestObstacle = closestObstacle;
        if (closestObstacle == null)
        {
            return Vector3.zero;
        }

        Vector3 force;
        if (Vector3.Distance(closestObstacle.transform.position, nextPos) <= Vector3.Distance(closestObstacle.transform.position, nextNextPos))
        {
            force = nextPos - closestObstacle.transform.position;
        }
        else 
        {
            force = nextNextPos - closestObstacle.transform.position;
        }
        force = (force).normalized * maxVelocity;
        force.y = 0f;
        return force;
    }

    private Vector3 EvasionForce()
    {
        // Finding closest other advertiser (if any within a relatively big radius)
        GameObject closestAdv = FindClosestAdv();
        if (closestAdv == null) return Vector3.zero; // No advertiser close enough so evasion force is zero

        float x = closestAdv.transform.position.x - transform.position.x;
        float z = closestAdv.transform.position.z - transform.position.z;

        Vector3 desired = new Vector3(-x, 0, -z); // Force in opposite direction of the closest other advertiser
        desired.Normalize();
        desired *= maxVelocity;

        Vector3 evasionForce = desired - rb.velocity;
        evasionForce.y = 0f;

        return evasionForce;
    }

    private GameObject FindFreeSeat()
    {
        return obstacleHandler.FindFreeSeat();
    }

    private void ReleaseChair()
    {
        target.GetComponent<ChairProps>().SetOccupied(false);
    }

    private void ChangeColor()
    {
        Renderer rend = GetComponent<Renderer>();
        if (successfulPichCount == 1)
        {
            rend.material.SetColor("_Color", Color.yellow);
        }
        if (successfulPichCount == 2)
        {
            rend.material.SetColor("_Color", Color.green);
        }
    }

    // Method to find a new goal location (outside the food court)
    private void ChooseRandomGoal()
    {
        float advX = UnityEngine.Random.Range(-24f, 24f);
        float advZ = UnityEngine.Random.Range(-18f, 18f);

        while (advX >= -15 && advX <= 20 && advZ >= -8 && advZ <= 8)
        {
            advX = UnityEngine.Random.Range(-24f, 24f);
            advZ = UnityEngine.Random.Range(-18f, 18f);
        }
        goalX = advX;
        goalZ = advZ;
    }

    // Method to drop flyer at current postition with the given probability
    private void DropFlyer()
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) < advertiserHandler.GetAdProb())
        {
            Instantiate(flyer, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
        }
    }

    // Finding a flyered shopper in observation radius (if any)
    private void LookForTarget()
    {
        target = obstacleHandler.FindFlyeredShopper(this.transform.position, advertiserHandler.GetObservationDist());
    }

    // Method to registed a pitch being successful
    private void RegisterSuccessfulPitch()
    {
        successfulPichCount++;
        if (successfulPichCount == 3)
        {
            Destroy(gameObject); // Despawn advertiser if it has finished 3 pitches
        }
        else 
        {
            ChangeColor(); // Update color to match number of successful pitches
        }
    }

    // Method to find closest other advertiser withing a radius of 7 (relatively big)
    private GameObject FindClosestAdv()
    {
        GameObject closestAdv = null;
        foreach (GameObject adv in advertiserHandler.GetAdvertisers())
        {
            if (adv.Equals(gameObject)) continue; // Don't consider the current advertiser
            if (MyDistance(transform.position, adv.transform.position) < 7f)
            {
                if (closestAdv == null) closestAdv = adv;
                else if (MyDistance(transform.position, closestAdv.transform.position) > MyDistance(transform.position, adv.transform.position))
                {
                    closestAdv = adv;
                }
            }
        }
        return closestAdv;
    }
}
