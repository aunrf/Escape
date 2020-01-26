using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 3.0f;

    private CharacterController controller;
    private Vector3 moveVector;

    // Movement
    private float speed = 5.0f;
    private float verticalVelocity = 0.0f;
    private float gravity = 12.0f;
    private int desiredLane = 1; // 0 = Left, 1 = Middle, 2 = Right

    private float animationDuration = 2.0f;
    private float startTime;

    private bool isDead = false; 

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController> ();
        startTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDead)
            return;

        if (Time.time - startTime < animationDuration)
        {
            controller.Move(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        // Gather the inputs on which lane we should be
        if (MobileInputs.Instance.SwipeLeft)
            MoveLane(false);
        if (MobileInputs.Instance.SwipeRight)
            MoveLane(true);

        // Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANCE;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE;

        // Let's calculate the move delta
        Vector3 moveVector =Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);
        }

        moveVector.y = -0.1f;
        moveVector.z = speed;
        // Move the robot
        controller.Move(moveVector * Time.deltaTime);
    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    public void SetSpeed(float modifier)
    {
        speed = 5.0f + modifier;
    }

    // It is begin called every time our capsule hits something
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.point.z > transform.position.z + 0.1f && hit.gameObject.tag == "Enemy")
            Death();
    }
    private void Death()
    {
        isDead = true;
        GetComponent<Score>().OnDeath ();
    }
}

