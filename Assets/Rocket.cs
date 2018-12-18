using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audiosource;


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

        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating Left");
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("Rotating Right");
            transform.Rotate(-Vector3.forward);
        }

        rigidBody.freezeRotation = false; //resume physics control
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))    //can thrust while rotating
        {
            print("Thrusting");
            rigidBody.AddRelativeForce(Vector3.up);
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
}
