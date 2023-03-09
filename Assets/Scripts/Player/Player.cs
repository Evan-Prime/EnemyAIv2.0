using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{

    [Header("Visuals")]
    public GameObject model;
    public float rotatingSpeed = 2f;

    [Header("Movement")]
    public float speed;
    public float jumpingVelocity;

    private Rigidbody playerRigidbody;
    private bool canJump;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {

        // Raycast to identify if the player can jump.
        if (Physics.Raycast(transform.position, Vector3.down, 1.02f))
        {
            canJump = true;
        }

        ProcessInput();
    }


    void ProcessInput()
    {
        // Move in the XZ plane.
        // 

        if (Input.GetKey("w"))
        {
            
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);

        }
        else if (Input.GetKey("s"))
        {
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime), Space.Self);

        }
        else if (Input.GetKey("d"))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.Self);

        }
        else if (Input.GetKey("a"))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0), Space.Self);

        }

        // Check for jumps.
        if (canJump == true && Input.GetKey("space"))
        {
            canJump = false;
            playerRigidbody.AddRelativeForce(new Vector3(0, jumpingVelocity, 0), ForceMode.Force);
        }


    }
}
