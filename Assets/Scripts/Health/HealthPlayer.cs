using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    public float currentHealth { get; private set; }
    private Animator animator;

    private bool isInvincible = false;
    private float invincibilityDuration = 2.0f;
    private float invincibilityTimer = 0.0f;

    private float queueTime = .2f;
    private float time = 0f;
    private AudioSource audioSource;
    [SerializeField] private AudioClip hurt;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip add;
    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        // Si est� en invincibilidad, actualiza el temporizador
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            time += Time.deltaTime;

            if (time > queueTime)
            {
                GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 0f);
                if (time > queueTime * 2f)
                {
                    GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
                    time = 0f;
                }
            }

            // Si el temporizador llega a cero, desactiva la invincibilidad
            if (invincibilityTimer <= 0)
            {
                GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
                isInvincible = false;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        // Si no est� en invincibilidad, toma da�o
        if (!isInvincible && PlayerPrefs.GetInt("died") == 0)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

            // Activa la invincibilidad y configura el temporizador
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;

            if (currentHealth == 0)
            {
                audioSource.PlayOneShot(death);
                animator.SetTrigger("die");
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<AttackScript>().enabled = false;
                PlayerPrefs.SetInt("died", 1);
            }
            else
            {
                audioSource.PlayOneShot(hurt);
                animator.SetTrigger("hurt");
            }
        }
    }
    public void AddHealth(float value)
    {
        audioSource.PlayOneShot(add);
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
    }
}
