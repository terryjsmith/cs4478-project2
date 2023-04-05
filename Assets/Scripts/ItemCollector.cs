using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollector : MonoBehaviour
{
   private int Gold = 0;

   [SerializeField] private Text GoldText;

   [SerializeField] private AudioSource coinSoundEffect;

   private void OnTriggerEnter2D(Collider2D collision)
   {
        if (collision.gameObject.CompareTag("Gold"))
        {
            coinSoundEffect.Play();
            Destroy(collision.gameObject);
            Gold++;
            GoldText.text = "Gold: " + Gold;

        }
   }
}
