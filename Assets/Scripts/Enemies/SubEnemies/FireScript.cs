using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : Enemy
{
    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator animator;
    private bool triggered;
    private bool active;
    private SpriteRenderer spriteRend;
    private AudioSource audioSource;
    [SerializeField] private AudioClip burst;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (ticking)
        {
            time += Time.deltaTime;
            GetComponent<SpriteRenderer>().material.color = Color.Lerp(Color.white, Color.red, time / 2f);
        }*/
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggered)
            {
                StartCoroutine(TurnOn());
            }
            if (active)
            {
                base.OnTriggerEnter2D(collision);
            }
        }
    }
    private IEnumerator TurnOn()
    {
        /*for (float i = 0f; i <= 10f; i += 0.001f)
        {
            GetComponent<SpriteRenderer>().material.color = Color.Lerp(Color.white, Color.red, i);
            yield return null;
        }
        animator.SetBool("turnOn", true);
        GetComponent<SpriteRenderer>().material.color = Color.white;*/

        triggered = true;
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white;
        active = true;
        animator.SetBool("turnOn", true);
        audioSource.PlayOneShot(burst);

        yield return new WaitForSeconds(activeTime);
        animator.SetBool("turnOn", false);
        active = false;
        triggered = false;
    }
}
