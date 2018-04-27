using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour {

    public float speed;
    public float rotationalSpeed;
    public GameObject closestStarling;
    public static GameObject[] allStarlings;
    // Use this for initialization
    void Start () {
        speed = 1f;
        rotationalSpeed = 4f;
        allStarlings = StarlingController.allStarlings;
	}
	
	// Update is called once per frame
	void Update () {
        closestStarling = FindClosestStarling();
        Vector3 direction = closestStarling.transform.position - this.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationalSpeed * Time.deltaTime);
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    private GameObject FindClosestStarling()
    {
        GameObject tempClosestStarling = allStarlings[0];
        float closestStarlingDistance = 0;
        foreach (GameObject starling in allStarlings)
        {
            float distanceFromStarling = (starling.transform.position - this.transform.position).sqrMagnitude;
            if (distanceFromStarling < closestStarlingDistance)
            {
                tempClosestStarling = starling;
                closestStarlingDistance = distanceFromStarling;
            }
        }
        return tempClosestStarling;
    }
}
