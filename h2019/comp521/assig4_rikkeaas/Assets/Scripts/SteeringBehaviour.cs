using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour : MonoBehaviour
{
    private ObstacleHandler obstacleHandler;

    private GameObject target = null; // target of seeking force
    private float goalX; // The exit x the shopper will steer towards when done with food
    private float goalZ; // The exit z the shopper will steer towards when done with food
    private Rigidbody rb;

    private float timeCounter = -1; // Counter to keep track of shopping and eating time
    private float flyeredTimeCounter = -1; // Counter to keep track of flyered time when shopper is flyered
    private bool hasFood = false; // Indicates if the shopper has bought food and should look for seat (or is eating)
    private bool isFlyered = false; // Indicates if the shopper is currently flyered

    private float maxVelocity; // Velocity of forces
    private float objectAvoidanceLength = 5; // How far ahead the shopper looks for obstacles

    private GameObject closestObstacle = null; // The currently closest collision obstacle

    // Start is called before the first frame update
    void Start()
    {
        maxVelocity = 7f + UnityEngine.Random.Range(-1f, 1f); // Some variation in velocity for each shopper
        obstacleHandler = GameObject.Find("ObstacleHandler").GetComponent<ObstacleHandler>();
        goalX = 25f; // Exit the right side of the mall
        goalZ = transform.position.z; // Exit at same z-coord as entered
        rb = GetComponent<Rigidbody>();
        if (UnityEngine.Random.Range(0f, 1f) < 0.5) // 50% chance to shop for food at random shop
        {
            target = obstacleHandler.GetRandomShop();
        }
    }

    void Update()
    {
        if (isFlyered) 
        {
            rb.velocity = Vector3.zero; // Should not move
            if (flyeredTimeCounter == -1) // First update since being flyered, starting counter
            {
                flyeredTimeCounter = 0;
            }
            else if (flyeredTimeCounter < 2) // Still haven't finished flyered time, incrementing counter
            {
                flyeredTimeCounter += Time.deltaTime;
            }
            else // Have finished flyered time
            {
                isFlyered = false;
                ChangeColor(); // Change color back to normal to indicate no longer flyered
                flyeredTimeCounter = -1;
            } 
            return; // When flyered shopper shouldn't calculate forces (is stationary)
        }
        if (target == null && hasFood) target = FindFreeSeat(); // Hasn't found free seat yet, trying again every update

        if (target != null)
        {
            if (timeCounter == -1 && !hasFood) // Still looking for shop to buy food
            {
                if (MyDistance(transform.position, target.transform.position) <= 3.1f) timeCounter = 0; // Shop found, start shopping countdown
            }
            else if (timeCounter < 1 && !hasFood) timeCounter += Time.deltaTime; // Shop has been found, but not finished shopping, increment counter
            else if (timeCounter >= 1 && !hasFood) // Shopping done
            {
                timeCounter = -1; // Reset counter
                hasFood = true; // Indicate that food has been bought
            }
            else if (timeCounter == -1 && hasFood) // Just got food, must find seat
            {
                timeCounter = 0; 
                target = FindFreeSeat(); // Setting a free seat as new target (could be null if no free seats)
            }
            // Sitting (standing) on/by chair and eating
            else if (hasFood && timeCounter < 2.5f && MyDistance(transform.position, target.transform.position) <= 1.3f)
            {
                timeCounter += Time.deltaTime;
                return; // Should stand still by chair and "eat" so no forces needed
            }
            else if (hasFood && timeCounter >= 2.5f) // Finished eating
            {
                ReleaseChair(); // Indicate seat is now free
                hasFood = false;
                timeCounter = -1;
                target = null; // No longer a target, so shopper will head towards exit
            }
        }

        Vector3 combined = CombineForces(SeekForce(), AvoidanceForce(), SeparationForce()); // Calculate steering force
        rb.AddForce(combined, ForceMode.VelocityChange); // Add force to rigid body of shopper
        transform.rotation = Quaternion.LookRotation(combined); // Rotate shopper to face direction of movement

        if (transform.position.x > 24) // Shopper has exited, despawn
        {
            Destroy(this.gameObject);
        }

        if (transform.position.x < -24) // Player shouldn't move outside mall on left side
        {
            transform.position = new Vector3(-24, transform.position.y, transform.position.z);
        }
    }

    // Method to calculate distance in x-z-plane
    private float MyDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt((a.x - b.x)*(a.x - b.x) + (a.z - b.z)*(a.z - b.z));
    }

    // Method to combine all forces to one steering force
    private Vector3 CombineForces(Vector3 seekForce, Vector3 avoidForce, Vector3 separationForce)
    {
        if (closestObstacle == null) // No obstacle, so no need to consider avoidance force
        {
            return (seekForce + separationForce).normalized * maxVelocity;
        }
        else 
        {
            // Calculating distance to obstacle and (if applicable) the target to see how to weigh the forces
            float distObstacle = Vector3.Distance(transform.position, closestObstacle.transform.position);
            if (target != null) 
            {
                float distTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distTarget < distObstacle) // If target is closer, disregard other forces
                {
                    return (seekForce);
                }
            }
            
            float weight = 2f/distObstacle; // If distance to obstacle is less than 2, this weight will be more than one
            Vector3 newForce;
            if (weight > 1)  // Obstacle very close, disregard seeking force
            {
                newForce = (avoidForce + separationForce).normalized * maxVelocity;
            }
            else
            {
                // If distance is big, weight will be small and opposite for when obstacle is close
                newForce = avoidForce * weight; // Close obstacle -> big weight -> avoidance force most important
                newForce += seekForce * (1 - weight); // Far away obstacle -> big (1-weight) -> seeking force most important
                newForce = (newForce + separationForce).normalized * maxVelocity; // Separation force doesn't depend on closeness of obstacle
            }
            return newForce;
        }
    }

    // Method to calculate seeking force
    private Vector3 SeekForce() 
    {
        float desiredX;
        float desiredZ;
        if (target != null && !hasFood) // Shop is target
        {
            desiredX = target.transform.position.x - transform.position.x;
            desiredZ = target.transform.position.z;
            // Seeking the "entrance" of the shop, so dislocation z by +/- 2.5 depending on wether shop is in top or bottom row
            if (desiredZ < 0) 
            {
                desiredZ += 2.5f;
            }
            else desiredZ -= 2.5f;

            desiredZ -= transform.position.z;
        }
        else if (target != null && hasFood) // Target is a chair, so seeking directly to chair
        {
            desiredX = target.transform.position.x - transform.position.x;
            desiredZ = target.transform.position.z - transform.position.z;
        }
        else // No target, seek towards entrance
        {
            desiredX = goalX - transform.position.x;
            desiredZ = goalZ - transform.position.z;
        }

        Vector3 normalDesired = new Vector3(desiredX, 0, desiredZ);
        normalDesired = normalDesired.normalized * maxVelocity;

        Vector3 seekForce = normalDesired - rb.velocity;
        seekForce.y = 0f;

        return seekForce;
    }

    // Method to calculate obstacle avoidance force
    private Vector3 AvoidanceForce()
    {
        // Extrapolating current position with the velocity to see where shopper will be in future
        Vector3 nextPos = transform.position + rb.velocity.normalized * objectAvoidanceLength * 0.5f;
        Vector3 nextNextPos = transform.position + rb.velocity.normalized * objectAvoidanceLength;

        // Finding closest obstacle
        GameObject closestObstacle = obstacleHandler.FindClosestObstacle(transform.position, nextPos, nextNextPos, target);
        this.closestObstacle = closestObstacle;
        
        if (closestObstacle == null) // No obstacle to collide with so force is just the zero vector
        {
            return Vector3.zero;
        }
        
        Vector3 force;
        // Finding if collison will happen with first or second interpolation to find the perpendicular avoidance force
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

    // Method to calculate separation force
    private Vector3 SeparationForce()
    {
        Vector3 separation = obstacleHandler.CalculateSeparation(gameObject);
        return separation.normalized * maxVelocity;
    }

    private GameObject FindFreeSeat()
    {
        return obstacleHandler.FindFreeSeat();
    }

    private void ReleaseChair()
    {
        target.GetComponent<ChairProps>().SetOccupied(false);
    }

    // Method to register shopper colliding with flyer
    void OnTriggerEnter(Collider other)
    {
        isFlyered = true;
        ChangeColor(); // Change color to indicate being flyered
        Destroy(other.gameObject);
    }

    private void ChangeColor()
    {
        Renderer rend = GetComponent<Renderer>();
        if (isFlyered)
        {
            rend.material.SetColor("_Color", Color.blue);
        }
        else 
        {
            rend.material.SetColor("_Color", Color.cyan);
        }
    }

    public bool IsFlyered()
    {
        return isFlyered;
    }
}
