using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 3f;


    //todo remove from inspector later
    [Range(0,1)] [SerializeField] float movementFactor; // 0 for not moved, 1 for full range.

    Vector3 startingPos;

	// Use this for initialization
	void Start ()
    {
        startingPos = transform.position;
	}

    // Update is called once per frame
    void Update()
    {
        //set movement factor automatically
        if (period > 0)     //protect against zero or negative period
        {
            float cycles = Time.time / period; //grows continually
            const float tau = Mathf.PI * 2; //6.28
            float rawSinWave = Mathf.Sin(cycles * tau);
            movementFactor = (rawSinWave + 1f) / 2f;
        }
        
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
