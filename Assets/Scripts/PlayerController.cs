using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class PlayerController : MonoBehaviour
{
    internal int hitpoint = 5;
    private Animator animator;
    private Rigidbody rb;
    private byte side;
    private WebSocketServer Server;

    [SerializeField] private bool isGround;
    [SerializeField] private int jumpPower;
    [SerializeField] private GameObject snowBall;

    internal void Init(byte side)
    {
        this.side = side;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Server = new WebSocketServer(2010);
        Server.AddWebSocketService<GetAction>("/");
        Server.Start();
    }

    private void Update()
    {
        if (isGround)
        {
            if (side == 0)
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
                    rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
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
        Instantiate(snowBall,transform.position,transform.rotation).GetComponent<ThrowBall>().Init(side);

    }

    private void Avoid()
    {
        animator.SetTrigger("Avoid");
    }

    public class GetAction : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            switch (int.Parse(e.ToString()))
            {

                case 0:
                    break;
                case 1:
                    
                    break;
                default:
                    break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
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
