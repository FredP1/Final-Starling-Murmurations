using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    void Update()
    {
        Vector3 watchPoint = StarlingController.goalPosition;
        transform.LookAt(StarlingController.predator.transform.position);
    }
}
