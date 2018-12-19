using System;                           //namespaces are similar to libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//todo fix lighting bug

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audiosource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;
    int level = 0;
    const int max_level = 3;

    //debug variables
    bool collisionON = true;

    [SerializeField] float rcsThrust = 75f;
    [SerializeField] float linThrust = 75f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip Death;
    [SerializeField] AudioClip levelup;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Debug.isDebugBuild)
        {
            Debugger();
        }

        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        else
        {
            mainEngineParticles.Stop();
        }
    }

    private void Debugger()
    {
        //advance level
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        //toggle collision detection
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionON = !collisionON;  //toggle collisions
            print(collisionON);
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
            ApplyThrust();

        }
        else
        {
            audiosource.Stop();
            mainEngineParticles.Stop();
            
        }
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = linThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audiosource.isPlaying)
        {
            audiosource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                if (collisionON == true)
                {
                    StartDeathSequence();
                }
                        
                break;
        }
    }

    private void StartDeathSequence()
    {
        // destroy it
        state = State.Dying;
        audiosource.Stop();
        audiosource.PlayOneShot(Death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audiosource.Stop();
        audiosource.PlayOneShot(levelup);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        level = 0;
        SceneManager.LoadScene(level);
        state = State.Alive;
    }

    private void LoadNextLevel()
    {
        level = level + 1;
        print("level");
        print(level);
        if (level > max_level)
        {
            level = max_level;
        }
        SceneManager.LoadScene(level); //todo allow for more than 2 levels
        state = State.Alive;
        audiosource.PlayOneShot(levelup);
    }
}
