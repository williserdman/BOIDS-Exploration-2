using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [SerializeField] int birdCount = 50;
    [SerializeField] GameObject birdPrefab;

    GameObject[] birdTracker;

    // Start is called before the first frame update
    void Start()
    {
        birdTracker = new GameObject[birdCount];

        foreach (GameObject b in birdTracker) Instantiate(birdPrefab, new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), Quaternion.identity); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
