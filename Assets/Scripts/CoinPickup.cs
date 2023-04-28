using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinSFX;
    [SerializeField] int pointsForCoinPickup = 100;

    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinSFX, Camera.main.transform.position, 0.5f);
            FindObjectOfType<GameSession>().IncreaseScore(pointsForCoinPickup);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
