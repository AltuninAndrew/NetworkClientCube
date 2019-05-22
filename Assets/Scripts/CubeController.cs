using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    
    
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0, 1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, 0, -1);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
        }
    }

    
    
}
