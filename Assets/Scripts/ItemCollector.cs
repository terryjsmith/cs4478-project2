using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollector : MonoBehaviour
{
   private int Gold = 0;

   [SerializeField] private Text GoldText;

   [SerializeField] private AudioSource coinSoundEffect;

    List<string> collectedCoins;

    public void Start()
    {
        collectedCoins = new List<string>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
   {
        if (collision.gameObject.CompareTag("Gold"))
        {
            // Skip processing of already collected coins due to multiple rigid bodies
            if (collectedCoins.Contains(collision.gameObject.name)) return;

            collectedCoins.Add(collision.gameObject.name);
            coinSoundEffect.Play();
            Destroy(collision.gameObject);
            Gold++;
            GoldText.text = "Gold: " + Gold;

        }
   }
}
