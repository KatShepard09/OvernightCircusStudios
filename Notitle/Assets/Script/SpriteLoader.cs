using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    public Sprite[] sprites; // Array of sprites for different directions
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Get the direction from the sprite to the camera
        Vector3 toCameraDirection = mainCamera.transform.position - transform.position;
        toCameraDirection.y = 0f; // Ignore vertical component

        // Get the forward direction of the sprite (its "front")
        Vector3 spriteForward = transform.forward;
        spriteForward.y = 0f; // Ignore vertical component

        // Calculate the angle in degrees between the camera's forward direction and the direction to the camera
        float angle = Mathf.Atan2(toCameraDirection.x, toCameraDirection.z) * Mathf.Rad2Deg;

        // Ensure the angle is positive and in the range [0, 360)
        if (angle < 0f)
        {
            angle += 360f;
        }

        // Calculate the index of the sprite to use
        int index = Mathf.FloorToInt(angle / 45f) % 8;
        if (index < 0)
        {
            index += 8; // Ensure positive index
        }

        // Debug.Log to check the angle and index
        Debug.Log("Angle: " + angle + ", Index: " + index);

        // Check if the index is within the bounds of the sprites array
        if (index >= 0 && index < sprites.Length)
        {
            // Set the sprite based on the index
            spriteRenderer.sprite = sprites[index];
        }
        else
        {
            Debug.LogError("Index out of bounds: " + index);
        }

        // Debug.DrawRay to visualize the vectors for debugging
        Debug.DrawRay(transform.position, spriteForward * 2f, Color.green);
        Debug.DrawRay(transform.position, toCameraDirection.normalized * 2f, Color.red);
    }
}

