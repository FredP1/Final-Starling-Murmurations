using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarlingController : MonoBehaviour {
    public GameObject starlingPrefab;
    public GameObject predatorPrefab;
    public static float borderSize = 1;

    public static int numStarling = 200;
    public static GameObject[] allStarlings = new GameObject[numStarling];
    public static GameObject predator;

    public static Vector3 goalPosition;

	// Use this for initialization
	void Start ()
    {
		for (int i = 0; i < numStarling; i++)
        {
            Vector3 position = new Vector3(UnityEngine.Random.Range(-borderSize, borderSize),
                                           UnityEngine.Random.Range(-borderSize, borderSize),
                                           UnityEngine.Random.Range(-borderSize, borderSize));
            allStarlings[i] = Instantiate(starlingPrefab, position, Quaternion.identity);
        }
        SpawnPredator();
    }

    private void SpawnPredator()
    {
        Vector3 predatorPosition = new Vector3(UnityEngine.Random.Range(-borderSize, borderSize),
                       UnityEngine.Random.Range(-borderSize, borderSize),
                       UnityEngine.Random.Range(-borderSize, borderSize));
        predator = Instantiate(predatorPrefab, predatorPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
        if (UnityEngine.Random.Range(0, 500) < 50)
        {
            goalPosition = new Vector3(UnityEngine.Random.Range(-borderSize, borderSize),
                                       UnityEngine.Random.Range(-borderSize, borderSize),
                                       UnityEngine.Random.Range(-borderSize, borderSize));
        }
    }
}
