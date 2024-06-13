using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] float maxSpeed = 7f;
    [SerializeField] float viewRadius = 30;
    [SerializeField] float wanderAmmount = 5f;
    [SerializeField] float neighborsToNotice = 5f;
    [SerializeField] float alignmentFactor = 1f;
    [SerializeField] float seperationFactor = 5f;
    [SerializeField] float cohesionFactor = 5f;
    [SerializeField] float wanderFactor = 5f;
    [SerializeField] float directionFactor = 5f;
    [SerializeField] float verticalLimiter = 0.2f;
    [SerializeField] float boundary = 300f;
    [SerializeField] float forceBoundaryFactor = 5f;
    [SerializeField] float maxSpeedBurst = 12f;
    [SerializeField] float angleFactor = 1f;

    public Vector3 velocity = Vector3.zero;

    float originalDirectionFactor;

    // Start is called before the first frame update
    void Start()
    {
        originalDirectionFactor = directionFactor;
        Wander();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //velocity = Vector3.ClampMagnitude(velocity, velocity.magnitude / 1.5f);

        Bird[] neighbors = GetNeighbors();

        //print(neighbors);
        //print(neighbors.Length);

        if (!IsWithinBoundary())
        {
            directionFactor = forceBoundaryFactor;
        } else
        {
            directionFactor = originalDirectionFactor;
        }

        if (IsBelowOrigin())
        {
            velocity += Vector3.up * forceBoundaryFactor;
        }

        if (neighbors != null & neighbors.Length > 0)
        {
            Seperation(neighbors);
            Alignment(neighbors);
            Cohesion(neighbors);
        } else
        {
            Wander();
        }

        if (AngleToOrigin() < 45) PushCentralNoYShift();

        GoTowardOrigin();
        

        int neighborCount = neighbors.Length;

        if (neighborCount > 1)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        } else
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }

        // velocity.y = velocity.y * verticalLimiter * Time.deltaTime;

        //velocity *= 5f;
        //print(velocity);

        if (Random.Range(1, 501) == 1)
        {
            velocity = velocity.normalized * maxSpeedBurst;
        } else
        {
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }

        //print(clampedVelocity);
        transform.Translate(velocity * Time.deltaTime);
    }

    void Wander()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-wanderAmmount, wanderAmmount), verticalLimiter * Random.Range(-wanderAmmount, wanderAmmount), Random.Range(-wanderAmmount, wanderAmmount));

        velocity += randomDirection.normalized * wanderFactor;
    }

    Bird[] GetNeighbors()
    {
        // returns 1 when it's by itself
        Collider[] physObj = Physics.OverlapSphere(transform.position, viewRadius);
        List<Bird> gameObjs = new List<Bird>();

        // skipping itself which will be first object in list
        for(int i = 1; i < physObj.Length; i++)
        {
            Bird b = physObj[i].gameObject.GetComponent<Bird>();
            if (b != null)
            {
                gameObjs.Add(b);
            }
        }

        return gameObjs.ToArray();
    }

    void Alignment(Bird[] neighbors) {
        Vector3 vel = Vector3.zero;

        foreach(Bird n in neighbors)
        {
            vel += n.velocity.normalized;
        }

        velocity += vel.normalized * alignmentFactor;
    }

    void Seperation(Bird[] neighbors) {
        Vector3 vel = Vector3.zero;

        foreach (Bird n in neighbors)
        {
            Vector3 directionToOther = n.gameObject.transform.position - transform.position;
            vel -= directionToOther.normalized;
        }

        velocity += vel.normalized * seperationFactor;
    }

    void Cohesion(Bird[] neighbors) {
        Vector3 sum = Vector3.zero;


        foreach (Bird n in neighbors)
        {
            sum += n.transform.position;
        }

        Vector3 average = sum / neighbors.Length;

        Vector3 directionToCenter = average - transform.position;

        velocity += directionToCenter.normalized * cohesionFactor;
    }

    void GoTowardOrigin()
    {
        Vector3 directionToOrigin = Vector3.zero - transform.position;

        velocity += directionToOrigin.normalized * directionFactor;
    }

    bool IsWithinBoundary()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) > boundary)
        {
            return false;
        }

        return true;
    }

    bool IsBelowOrigin() {
        if (transform.position.y < 0) return true;
        return false;
    }

    bool IsWithinChimney()
    {
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), Vector3.zero) < 30) return true;

        return false;
    }

    float AngleToOrigin()
    {
        Vector3 vecToOrigin = Vector3.zero - transform.position;
        return Vector3.Angle(vecToOrigin, Vector3.ProjectOnPlane(vecToOrigin, Vector3.up));
    }

    void PushCentralNoYShift()
    {
        Vector3 directionToOrigin = Vector3.zero - transform.position;
        directionToOrigin.y *= 0;

        velocity += directionToOrigin * angleFactor;
    }
}
