using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [SerializeField] int birdCount = 50;
    [SerializeField] GameObject birdPrefab;

    public Bird[] birdTracker;

    // Start is called before the first frame update
    void Start()
    {
        birdTracker = new Bird[birdCount];

        for (int i = 0; i < birdTracker.Length; i++)
        {
            birdTracker[i] = Instantiate(birdPrefab, new Vector3(Random.Range(-200, 200), Random.Range(0, 300), Random.Range(-200, 200)), Quaternion.identity).GetComponent<Bird>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
