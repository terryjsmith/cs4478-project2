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
    }

    // Update is called once per frame
    void Update()
    {
        m_currentSpeed = Input.GetAxis("Horizontal") * playerSpeed;

        // Check for jump and add force against gravity
        if (Input.GetButtonDown("Jump"))
        {
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            m_rigidBody.AddForce(transform.up * playerThrust);
        }

        // Check for firing bullets, play animation
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
        // Set velocity, but leave Y intact since we're using physics
        m_rigidBody.velocity = new Vector3(m_currentSpeed * Time.fixedDeltaTime, m_rigidBody.velocity.y, 0);
    }
}
