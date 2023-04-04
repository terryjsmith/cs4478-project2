using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    public float playerSpeed;

    [SerializeField]
    public float playerThrust;

    [SerializeField]
    public GameObject bulletPrefab;

    // Local copies for optimization
    Rigidbody2D m_rigidBody;
    Animator m_animator;

    // Current speed forward
    float m_currentSpeed;

    // Hearts in UI
    List<GameObject> hearts;
    int lifeRemaining;

    // Player direction
    bool flipped = false;

    Image damagePanel;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();

        lifeRemaining = 3;
        hearts = new List<GameObject>();
        hearts.Add(GameObject.Find("Heart1"));
        hearts.Add(GameObject.Find("Heart2"));
        hearts.Add(GameObject.Find("Heart3"));

        damagePanel = GameObject.Find("DamagePanel").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizMovement = Input.GetAxis("Horizontal");
        m_currentSpeed = horizMovement * playerSpeed;

        // Fade away damage panel if necessary
        if(damagePanel.color.a > 0.0f)
        {
            damagePanel.color = new Color(1.0f, 0.0f, 0.0f, damagePanel.color.a - (Time.deltaTime * 2.0f));
        }

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
            flipped = true;
        }

        if (horizMovement > 0.0f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            flipped = false;
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
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3((flipped ? -1.0f : 1.0f) * 50.0f, 0.0f, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Oh no!");
            hearts[lifeRemaining - 1].GetComponent<Image>().enabled = false;
            lifeRemaining--;

            damagePanel.color = new Color(1.0f, 0, 0, 0.6f);

            if (lifeRemaining <= 0)
            {
                Debug.Log("Game over man!");
            }
            return;
        }

        if (collision.gameObject.tag == "Heart")
        {
            Debug.Log("Life remaining: " + lifeRemaining);
            if (lifeRemaining < 3)
            {
                Debug.Log("Increment life remaining.");
                hearts[lifeRemaining].GetComponent<Image>().enabled = true;
                lifeRemaining++;
            }
            GameObject.Destroy(collision.gameObject);
        }
    }
}
