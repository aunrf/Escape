using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform lookAt; // our robot // object that we are looking at
    private Vector3 startOffset = new Vector3(0, 5.0f, -10.0f);
    private Vector3 desiredPosition;

    private float transition = 0.0f;
    private float animationDuration = 2.0f;
    private Vector3 animationOffset = new Vector3 (0, 5, 5);

    // Start is called before the first frame update
    private void Start ()
    {
       lookAt = GameObject.FindGameObjectWithTag ("Player").transform;
       startOffset = transform.position - lookAt.position;
    }

    // Update is called once per frame
    private void Update ()
    {
        Vector3 desiredPosition = lookAt.position + startOffset;
        //X
        desiredPosition.x = 0;
        //Y
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, 3, 5);

        if(transition > 1.0f)
        {
            transform.position = desiredPosition; 
        }
        else
        {
            // Animation at the start of the game
            transform.position = Vector3.Lerp(desiredPosition + animationOffset,desiredPosition, transition);
            transition += Time.deltaTime * 1 / animationDuration;
            transform.LookAt(lookAt.position + Vector3.up);
        }
        
    }
}
