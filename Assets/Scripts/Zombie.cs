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

    [SerializeField]
    private float attackInterval = 1.0f;

    private Rigidbody2D m_rigidBody;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private float startX;

    float m_countdownTimer;

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

        StartCoroutine(PerformAttack());
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
                GetComponent<Animator>().SetBool("ZombieFlashing", false);
            }
        }

        // Move the zombie
        if (m_rigidBody && !m_animator.GetBool("ZombieAttacking")) // Only move if the zombie is not attacking
        {
            m_rigidBody.velocity = new Vector2(zombieSpeed, m_rigidBody.velocity.y);
        }
        else
        {
            m_rigidBody.velocity = new Vector2(0.0f, m_rigidBody.velocity.y);
        }

        // startX + walkRadius or startX - walkRadius boundaries
        if (transform.position.x >= startX + walkRadius)
        {
            // Change direction to the left
            zombieSpeed = -Mathf.Abs(zombieSpeed);
            m_spriteRenderer.flipX = true;
        }
        else if (transform.position.x <= startX - walkRadius)
        {
            // Change direction to the right
            zombieSpeed = Mathf.Abs(zombieSpeed);
            m_spriteRenderer.flipX = false;
        }
    }

    IEnumerator PerformAttack()
    {
        while (true)
        {
            // Stop moving and play the attack animation
            m_animator.SetBool("ZombieAttacking", true);
            yield return new WaitForSeconds(1); 

            // Resume walking and wait for 5 seconds before attacking again
            m_animator.SetBool("ZombieAttacking", false);
            yield return new WaitForSeconds(5 - 1); 
        }
    }


    
    void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "Bullet")
        {
            health--;
            if (health > 0)
            {
                GetComponent<Animator>().SetBool("ZombieFlashing", true);
                m_countdownTimer = flashingTimer;
            }
            else
            {
                GetComponent<Animator>().SetBool("ZombieDying", true);
                GameObject.Destroy(gameObject, 1.25f);

                // Remove the rigidbody and collider so the player can step over a dead body without losing health
                Destroy(GetComponent<Rigidbody2D>());
                Destroy(GetComponent<BoxCollider2D>());
            }

            GameObject.Destroy(collision.gameObject);
        }
    }
}


/*
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

    [SerializeField]
    private float attackInterval = 1.0f;

    private Rigidbody2D m_rigidBody;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private float startX;

    float m_countdownTimer;
    private bool playerInRange = false;

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

        StartCoroutine(PerformAttack());
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
                GetComponent<Animator>().SetBool("ZombieFlashing", false);
            }
        }

        // Move the zombie
        if (m_rigidBody && !m_animator.GetBool("ZombieAttacking")) // Only move if the zombie is not attacking
        {
            m_rigidBody.velocity = new Vector2(zombieSpeed, m_rigidBody.velocity.y);
        }
        else
        {
            m_rigidBody.velocity = new Vector2(0.0f, m_rigidBody.velocity.y);
        }

        // startX + walkRadius or startX - walkRadius boundaries
        if (transform.position.x >= startX + walkRadius)
        {
            // Change direction to the left
            zombieSpeed = -Mathf.Abs(zombieSpeed);
            m_spriteRenderer.flipX = true;
        }
        else if (transform.position.x <= startX - walkRadius)
        {
            // Change direction to the right
            zombieSpeed = Mathf.Abs(zombieSpeed);
            m_spriteRenderer.flipX = false;
        }
    }

    IEnumerator PerformAttack()
    {
        while (true)
        {
            if (playerInRange)
            {
                // Stop moving and play the attack animation
                m_animator.SetBool("ZombieAttacking", true);
                yield return new WaitForSeconds(1); // Replace 1 with the duration of your attack animation
            }
            else
            {
                m_animator.SetBool("ZombieAttacking", false);
            }

            // Resume walking and wait for 5 seconds before checking player's proximity again
            yield return new WaitForSeconds(5 - 1); // Replace 1 with the duration of your attack animation and 5 with the interval between checks
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }

        if (collision.gameObject.tag == "Bullet")
        {
            health--;
            if (health > 0)
            {
                GetComponent<Animator>().SetBool("ZombieFlashing", true);
                m_countdownTimer = flashingTimer;
            }
            else
            {
                GetComponent<Animator>().SetBool("ZombieDying", true);
                GameObject.Destroy(gameObject, 1.25f);

                // Remove the rigidbody and collider so the player can step over a dead body without losing health

                Destroy(GetComponent<Rigidbody2D>());
                Destroy(GetComponent<BoxCollider2D>());
            }
            GameObject.Destroy(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}

*/ 