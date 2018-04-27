using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TopologicalBehaviour : MonoBehaviour
{

    public float speed;
    public float alignmentValue;
    public float cohesionFactor;
    public float predatorAvoidanceValue;
    float rotationSpeed;
    public static Vector3 cohesion, separation, alignment, predatorAvoidance;
    GameObject[] starlingArray;
    GameObject predator;
    List<GameObject> neighbourStarlings;
    List<GameObject> starlingsTooClose;
    public float groupSpeed;
    public float separationDistance;
    public float vicsekNoise;

    Vector3 goalPosition = StarlingController.goalPosition;

    // Use this for initialization
    void Start()
    {
        speed = UnityEngine.Random.Range(1f, 2f);
        cohesionFactor = 5;
        alignmentValue = 0.3f;
        rotationSpeed = 2.0f;
        separationDistance = 0.01f;
        vicsekNoise = 1f;
        predatorAvoidanceValue = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        goalPosition = StarlingController.goalPosition;
        cohesion = Vector3.zero;
        separation = Vector3.zero;
        alignment = Vector3.zero;
        starlingArray = StarlingController.allStarlings;
        //predator = TopologicalController.predator;
        vicsekNoise = UnityEngine.Random.Range(0.8f, 1.2f);
        neighbourStarlings = FindTopologicalNeighbours();
        if (neighbourStarlings.Count != 0)
        {
            starlingsTooClose = FindSeparationStarlings(neighbourStarlings);

            cohesion = Cohesion(neighbourStarlings);
            separation = Separate(starlingsTooClose);
            alignment = Align(neighbourStarlings);
        }
        //predatorAvoidance = CheckForPredators();
        Vector3 direction = ((cohesion + separation + alignment + predatorAvoidance + goalPosition) - transform.position) * vicsekNoise;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        transform.Translate(0, 0, Time.deltaTime * speed);
        //Debug.Log("The speed is: " + speed);
    }

    private Vector3 CheckForPredators()
    {
        Vector3 predatorVector = Vector3.zero;
        float distanceFromPredator = (predator.transform.position - this.transform.position).sqrMagnitude;
        if (distanceFromPredator < predatorAvoidanceValue)
        {
            predatorVector = (-predator.transform.position * 10);
        }
        return predatorVector;
    }

    private List<GameObject> FindSeparationStarlings(List<GameObject> neighbourStarlings)
    {
        List<GameObject> separationStarlings = new List<GameObject>();
        foreach (GameObject starling in neighbourStarlings)
        {
            float distanceFromStarling = (starling.transform.position - this.transform.position).sqrMagnitude;
            if (distanceFromStarling <= separationDistance)
            {
                separationStarlings.Add(starling);
            }
        }
        //Debug.Log("Num of Separation starlings: " + separationStarlings.Count);
        return separationStarlings;
    }

    private Vector3 Cohesion(List<GameObject> neighbourStarlings)
    {
        Vector3 flockCentre = Vector3.zero;
        foreach (GameObject starling in neighbourStarlings)
        {
            if (starling != this.gameObject)
            {
                flockCentre = flockCentre + starling.transform.position;
            }
        }
        flockCentre = flockCentre / (neighbourStarlings.Count);
        //Calculates factor that the starling should head to the centre of mass
        return flockCentre / cohesionFactor;
    }

    private Vector3 Align(List<GameObject> neighbourStarlings)
    {
        //throw new NotImplementedException();
        Vector3 alignVector = Vector3.zero;
        float totalNeighbourSpeed = 0;
        int neighbourCount = 0;
        foreach (GameObject starling in neighbourStarlings)
        {
            if (starling != this.gameObject)
            {
                alignVector += starling.transform.forward;
                TopologicalBehaviour anotherStarling = starling.GetComponent<TopologicalBehaviour>();
                totalNeighbourSpeed = totalNeighbourSpeed + anotherStarling.speed;
                neighbourCount++;
            }
        }
        if (neighbourCount > 0)
        {
            speed = totalNeighbourSpeed / neighbourCount;
            alignVector /= neighbourCount;
        }
        return alignVector;
    }

    private Vector3 Separate(List<GameObject> starlingsTooClose)
    {
        Vector3 groupCentre = Vector3.zero;

        if (starlingsTooClose.Count == 0)
        {
            return groupCentre;
        }
        foreach (GameObject starling in starlingsTooClose)
            if (starling != this.gameObject)
            {
                Vector3 separationValue = this.transform.position - starling.transform.position;
                float starlingDistance = separationValue.sqrMagnitude;
                float distanceMultiplier = 1 / starlingDistance;
                groupCentre += separationValue * distanceMultiplier;
            }
        return groupCentre;
    }
    private List<GameObject> FindTopologicalNeighbours()
    {
        List<GameObject> tempNeighbourList = new List<GameObject>();
        //Dictionary<GameObject, float> starlingObjectAndDistance = new Dictionary<GameObject, float>();
        //SortedList<GameObject, float> starlingObjectAndDistance = new SortedList<GameObject, float>();
        List<KeyValuePair<GameObject, float>> starlingObjectAndDistance = new List<KeyValuePair<GameObject, float>>();
        foreach (GameObject starling in starlingArray)
        {
            //float distanceFromStarling = Vector3.Distance(starling.transform.position, this.transform.position);
            float distanceFromStarling = (starling.transform.position - this.transform.position).sqrMagnitude;
            //Debug.Log("Distance from starling: " + distanceFromStarling);
            //starlingObjectAndDistance.Add(starling, distanceFromStarling);
            starlingObjectAndDistance.Add(new KeyValuePair<GameObject, float>(starling, distanceFromStarling));
        }
        starlingObjectAndDistance.Sort((x, y) => x.Value.CompareTo(y.Value));
        for (int i = 1; i < 8; i++)
        {
            tempNeighbourList.Add(starlingObjectAndDistance[i].Key);
            //Debug.Log("Neighbour List: " + starlingObjectAndDistance[i].Value);
            //Debug.Log("Closest Neighbour distance: " + starlingObjectAndDistance[i].Value + ": "+i);
        }
        //Debug.Log("Number of neighbours: " + tempNeighbourList.Count);
        return tempNeighbourList;
    }
}