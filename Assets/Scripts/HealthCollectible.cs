using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [Header("恢复生命值的量")]
    public int health = 1;
    [Header("音效")]
    public AudioClip collectedClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (playerController.Health < playerController.maxHealth)
            {
                playerController.ChangeHealth(health);
                playerController.PlaySound(collectedClip);
                Destroy(gameObject);
            }
        }
    }
}
