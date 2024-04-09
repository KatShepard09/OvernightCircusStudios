using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Get the direction from the sprite to the camera
        Vector3 lookDirection = mainCamera.transform.position - transform.position;
        lookDirection.y = 0f; // Lock rotation to the y-axis (2D)

        // Rotate the sprite to face the camera
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }

}
