using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    private CharacterController characterController;

    private bool isRunning = false;
    private float moveSpeed = 4f;

    [Header("Movement System")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;

    PlayerInteraction playerInteraction;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
        characterController = GetComponent<CharacterController>();

        playerInteraction = GetComponentInChildren<PlayerInteraction>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Interact();

        if(Input.GetKey(KeyCode.RightBracket))
        {
            TimeManager.Instance.Tick();
        }
    }

    public void Interact()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            playerInteraction.Interact();
        }
    }

    public void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 veclocity = moveSpeed * Time.deltaTime * dir;

        if (Input.GetButton("Sprint"))
        {
            moveSpeed = runSpeed;
            animator.SetBool("Running", true);
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("Running", false);
        }

        //Check if there is movement
        if (dir.magnitude >= 0.1f)
        {
            //look towards that direction
            transform.rotation = Quaternion.LookRotation(dir);

            //Move
            characterController.Move(veclocity);
        }
       
      /*  if (veclocity.magnitude >= 0.04f)
        {
            isRunning = true;
            animator.SetBool("Running", isRunning);
        } else if (veclocity.magnitude == 0.0f)
        {
            isRunning = false;
            animator.SetBool("Running", isRunning);
        }*/

        animator.SetFloat("Speed", veclocity.magnitude);
    }
}
