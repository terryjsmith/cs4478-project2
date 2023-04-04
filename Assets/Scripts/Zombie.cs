using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    int health;

    [SerializeField]
    float flashingTimer;

    float m_countdownTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If we're flashing, flash for a while then stop
        if(m_countdownTimer > 0)
        {
            m_countdownTimer -= Time.deltaTime;
            if(m_countdownTimer <= 0)
            {
                GetComponent<Animator>().SetBool("ZombieFlashing", false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
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
            }
            
            GameObject.Destroy(collision.gameObject);
        }
    }
}
