using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    public float playerSpeed;

    [SerializeField]
    public float playerThrust;

    [SerializeField]
    public GameObject bulletPrefab;

    Rigidbody2D m_rigidBody;
    float m_currentSpeed;
    Animator m_animator;
    GameObject spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
        spawnPoint = GameObject.Find("BulletSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        float horizMovement = Input.GetAxis("Horizontal");
        m_currentSpeed = horizMovement * playerSpeed;

        // Is the player running?
        if(horizMovement != 0.0f && m_rigidBody.velocity.y == 0.0f) // Don't run while jumping
        {
            m_animator.SetBool("PlayerRunning", true);
        }
        else
        {
            m_animator.SetBool("PlayerRunning", false);
        }

        // Which direction are they running?
        if (horizMovement < 0.0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (horizMovement > 0.0f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

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
            FireBullet();
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

    private void FireBullet()
    {
        // Create a bullet at the start of our gun
        GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(50.0f, 0.0f, 0.0f);
    }
}
