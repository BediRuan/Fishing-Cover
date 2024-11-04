using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Left Arrow Pressed");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Right Arrow Pressed");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up Arrow Pressed");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Down Arrow Pressed");
        }
    }
}
