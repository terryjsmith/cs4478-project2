using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float minimumY, maximumY, maximumX, minimumX;

    BoxCollider2D rightBound;
    BoxCollider2D player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Character").GetComponent<BoxCollider2D>();
        rightBound = GameObject.Find("RightBound").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();
        if (player.OverlapCollider(filter, results) > 0)
        {
            foreach(Collider2D collider in results)
            {
                if(collider.gameObject.name == "RightBound" && transform.position.x < maximumX)
                {
                    // Move camera right
                    transform.Translate(new Vector3(5.0f * Time.deltaTime, 0, 0));
                    continue;
                }

                if (collider.gameObject.name == "LeftBound" && transform.position.x > minimumX)
                {
                    // Move camera right
                    transform.Translate(new Vector3(-5.0f * Time.deltaTime, 0, 0));
                    continue;
                }

                if (collider.gameObject.name == "BottomBound" && transform.position.y > minimumY)
                {
                    // Move camera right
                    transform.Translate(new Vector3(0, -5.0f * Time.deltaTime, 0));
                    continue;
                }

                if (collider.gameObject.name == "TopBound" && transform.position.y < maximumY)
                {
                    // Move camera right
                    transform.Translate(new Vector3(0, 5.0f * Time.deltaTime, 0));
                    continue;
                }
            }
        }
    }
}
