using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float dmg;
    [SerializeField] protected float health;
    [SerializeField] protected bool indestructible;
    public float damage { get; protected set; }

    void Awake()
    {
        damage = dmg;
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthPlayer healthPlayer = collision.GetComponent<HealthPlayer>();

            if (healthPlayer != null)
            {
                healthPlayer.TakeDamage(damage);
            }

        }
        if (collision.CompareTag("Fireball") && !indestructible)
        {
            health -= 1;
        }
    }
    /*protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthPlayer healthPlayer = collision.GetComponent<HealthPlayer>();

            if (healthPlayer != null)
            {
                healthPlayer.TakeDamage(damage);
            }

        }
    }*/
}
