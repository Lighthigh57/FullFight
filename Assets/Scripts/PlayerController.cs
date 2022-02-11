using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    internal int hitpoint = 5;
    private Animator animator;
    private Rigidbody rb;
    private byte id;
    [SerializeField] private bool isGround;
    [SerializeField] private int jumpPower;

    internal void Init(byte id)
    {
        this.id = id;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isGround)
        {
            if (id == 0)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    Attack();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    Guard();
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    Avoid();
                    rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
                    isGround = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Attack();
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    Guard();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Avoid();
                    rb.AddForce(Vector3.up, ForceMode.VelocityChange);
                    isGround = false;
                }
            }
        }
        
    }

    private void Guard()
    {
        animator.SetTrigger("Guard");
    }

    private void Attack()
    {
        animator.SetTrigger("Panch");
    }

    private void Avoid()
    {
        animator.SetTrigger("Avoid");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            animator.SetTrigger("Land");
            isGround = true;
        }
    }
}
