using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    int health;

    [SerializeField]
    float flashingTimer;

    [SerializeField]
    private float zombieSpeed = 1.0f;

    [SerializeField]
    private float walkRadius = 5.0f;

    private Rigidbody2D m_rigidBody;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private float startX;

    float m_countdownTimer;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        startX = transform.position.x;

        // walking animation
        if (m_animator != null)
        {
            m_animator.SetBool("ZombieWalking", true);
        }

        player = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        // If we're flashing, flash for a while then stop
        if (m_countdownTimer > 0)
        {
            m_countdownTimer -= Time.deltaTime;
            if (m_countdownTimer <= 0)
            {
                m_animator.SetBool("ZombieFlashing", false);
                m_countdownTimer = 0.0f;
            }
        }

        // Move the zombie
        if (m_rigidBody)
        {
            if (!m_animator.GetBool("ZombieAttacking") && (m_countdownTimer == 0.0f)) // Only move if the zombie is not attacking
            {
                m_rigidBody.velocity = new Vector2(zombieSpeed, m_rigidBody.velocity.y);
            }
            else
            {
                m_rigidBody.velocity = new Vector2(0.0f, m_rigidBody.velocity.y);
            }
        }

        // startX + walkRadius or startX - walkRadius boundaries
        if (transform.position.x >= startX + walkRadius)
        {
            // Change direction to the left
            zombieSpeed = -Mathf.Abs(zombieSpeed);
        }
        else if (transform.position.x <= startX - walkRadius)
        {
            // Change direction to the right
            zombieSpeed = Mathf.Abs(zombieSpeed);
        }

        // Reset flip state each frame
        if(zombieSpeed < 0.0f)
        {
            m_spriteRenderer.flipX = true;
        }
        else
        {
            m_spriteRenderer.flipX = false;
        }

        // Measure distance from player
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if(distance < 4.0f)
        {
            m_animator.SetBool("ZombieAttacking", true);

            // Make sure we're facing the player to attack them
            if (player.transform.position.x < transform.position.x)
            {
                m_spriteRenderer.flipX = true;
            }
            else
            {
                m_spriteRenderer.flipX = false;
            }
        }
        else
        {
            m_animator.SetBool("ZombieAttacking", false);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health--;
            
            m_animator.SetBool("ZombieFlashing", true);
            m_countdownTimer = flashingTimer;
            
            if(health <= 0)
            {
                m_animator.SetBool("ZombieDying", true);
                m_animator.SetBool("ZombieWalking", false);
                GameObject.Destroy(gameObject, 1.0f);

                // Remove the rigidbody and collider so the player can step over a dead body without losing health
                Destroy(GetComponent<Rigidbody2D>());
                Destroy(GetComponent<BoxCollider2D>());
            }

            GameObject.Destroy(collision.gameObject);
        }
    }
}