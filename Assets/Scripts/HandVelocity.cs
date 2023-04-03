using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVelocity : MonoBehaviour
{


    private Vector3 lastFramePosition;

    // Update is called once per frame
    void Update()
    {
        lastFramePosition = transform.position;
    }

    public float GetSpeed()
    {
        return Vector3.Distance(lastFramePosition, transform.position) / Time.deltaTime;

    }
}
