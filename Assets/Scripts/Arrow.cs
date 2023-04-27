using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 10f;
    float xSpeed;

    Rigidbody2D rigidBody;
    PlayerMovement player;
   
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        rigidBody = GetComponent<Rigidbody2D>();
        xSpeed = player.transform.localScale.x * arrowSpeed;
    }

    void Update()
    {
        rigidBody.velocity = new Vector2(xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        Destroy(gameObject, 0.3f);
        
    }
}
