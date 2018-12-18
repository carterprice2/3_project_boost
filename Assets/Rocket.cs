using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audiosource;

    [SerializeField] float rcsThrust = 75f;
    [SerializeField] float linThrust = 75f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();

    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;  //take manual control of rotation
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.W))    //can thrust while rotating
        {
            float thrustThisFrame = linThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!audiosource.isPlaying)
            {
                audiosource.Play();
            }

        }
        else
        {
            audiosource.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                print("ok"); // todo remove
                break;
            case "Fuel":
                //do nothing
                print("ok"); // todo remove
                break;
            default:
                // destroy it
                print("dead");
                break;
        }
            
    }

}
