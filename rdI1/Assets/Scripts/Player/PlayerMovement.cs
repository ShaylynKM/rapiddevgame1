using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private void Update()
    {
        // Adjust the orientation of the character based on arrow key input

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FlipRight(); // Turn right
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FlipLeft(); // Turn left
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FlipUp(); // Turn up
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FlipDown(); // Turn down
        }
    }

    // The following methods rotate the character's orientation in 90-degree increments

    // Turn up
    public void FlipUp()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Turn left
    public void FlipLeft()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 90);
    }

    // Turn down
    public void FlipDown()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 180);
    }

    // Turn right
    public void FlipRight()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 270);
    }
}
