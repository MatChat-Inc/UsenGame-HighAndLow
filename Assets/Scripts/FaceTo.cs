using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTo : MonoBehaviour
{
    public GameObject @object;
    public bool faceToBack = false;

    void Update()
    {
        // Calculate the direction from the object to the camera
        Vector3 directionToCamera = @object.transform.position - transform.position;

        // Rotate the object to face the camera or the opposite direction
        if (faceToBack)
            transform.rotation = Quaternion.LookRotation(-directionToCamera);
        else transform.rotation = Quaternion.LookRotation(directionToCamera);
    }


}
