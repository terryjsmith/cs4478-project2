using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    public float playerSpeed;

    [SerializeField]
    public float playerThrust;

    Rigidbody2D m_rigidBody;
    float m_currentSpeed;
    Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
        //m_animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_currentSpeed = Input.GetAxis("Horizontal") * playerSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            m_rigidBody.AddForce(transform.up * playerThrust);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            m_animator.SetBool("PlayerShooting", true);
        }

        if(Input.GetButtonUp("Fire1"))
        {
            m_animator.SetBool("PlayerShooting", false);
        }
    }

    private void FixedUpdate()
    {
        m_rigidBody.velocity = new Vector3(m_currentSpeed * Time.fixedDeltaTime, m_rigidBody.velocity.y, 0);
    }
}
