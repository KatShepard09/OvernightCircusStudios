using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

    private Vector3 targetPosition;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
       
        float stoppingDistance = .1f;//Tells unit to stop moving when it is .1 away from location.
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;//movement speed of character
            transform.position += moveDirection * moveSpeed * Time.deltaTime;//keeps track of player movment and the amount of frames at which it moves.

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection,Time.deltaTime * rotateSpeed);//rotates charcter based on direction that they are moveing
            
            unitAnimator.SetBool("IsWalking", true);//when charcter moves it activates the move animation.
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);// when character is not moving it activates the idle animation.
        }

       
    }


    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
