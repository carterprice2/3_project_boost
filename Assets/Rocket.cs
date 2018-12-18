using System;                           //namespaces are similar to libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//todo fix lighting bug

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audiosource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    [SerializeField] float rcsThrust = 75f;
    [SerializeField] float linThrust = 75f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        else
        {
            audiosource.Stop();
        }
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
        if (Input.GetKey(KeyCode.Space))    //can thrust while rotating
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
        if (state != State.Alive){ return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                print("ok"); // todo remove
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f);
                break;
            default:
                // destroy it
                state = State.Dying;
                Invoke("LoadFirstLevel",2f);
                break;
        }
            
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); //todo allow for more than 2 levels
        state = State.Alive;
    }
}
