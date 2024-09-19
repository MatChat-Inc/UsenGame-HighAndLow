using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    // Reference to the main camera
    public Camera mainCamera;
    // Whether to face the camera or the opposite direction
    public bool faceToBack = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the main camera
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the direction from the object to the camera
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        // Rotate the object to face the camera or the opposite direction
        if (faceToBack)
            transform.rotation = Quaternion.LookRotation(-directionToCamera);
        else transform.rotation = Quaternion.LookRotation(directionToCamera);
    }


}
