using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        // Flips the character based on arrow key input

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FlipRight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FlipLeft();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FlipUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FlipDown();
        }

    }

    // Rotates the sprite in 90 degree increments
    public void FlipUp()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    public void FlipLeft()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 90);
    }
    public void FlipDown()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 180);
    }
    public void FlipRight()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 270);
    }
}
