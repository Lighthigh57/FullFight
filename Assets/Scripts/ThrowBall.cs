using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float speed;
    private Vector3 direction;

    internal void Init(int side)
    {
        if (side == 0)
        {
            direction = Vector3.forward * speed;
        }
        else
        {
            direction = Vector3.back * speed;
        }
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, -3.5f, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(direction);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
