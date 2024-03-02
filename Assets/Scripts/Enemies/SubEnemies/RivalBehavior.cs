using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalBehavior : Enemy
{
    [SerializeField] private bool closeBehavior = false;
    [SerializeField] private GameObject closeVision;
    [SerializeField] private float distance;
    [SerializeField] private float speed;
    [SerializeField] private float stopTime;
    [SerializeField] private float visionDistance;
    [SerializeField] private LayerMask playerLayer;
    private bool movingRight;
    private Animator animator;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private RivalFire[] fire;
    private float cooldownTimer;
    private bool stop = false;
    private Vector3 startPosition;
    private AudioSource audioSource;
    [SerializeField] private AudioClip slash;
    [SerializeField] private AudioClip hurt;
    [SerializeField] private AudioClip cast;
    [SerializeField] private AudioClip death;
    private void Start()
    {
        animator = GetComponent<Animator>();
        movingRight = transform.localScale == Vector3.one ? true : false;
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        if (closeBehavior)
        {
            closeVision.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (closeBehavior)
        {
            Movement();
        }
        else
        {
            FarAttack();
        }
    }
    public void CloseAttack()
    {
        animator.SetBool("walking", false);
        animator.SetTrigger("sword");
        audioSource.PlayOneShot(slash);
    }
    private void FarAttack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 0), visionDistance, playerLayer.value);

        if (hit.collider != null)
        {
            FireAttack();
        }
    }
    private void Movement()
    {
        if (!stop)
        {
            animator.SetBool("walking", true);
            float movement = movingRight ? speed * Time.deltaTime : -speed * Time.deltaTime;
            float currentDistance = movingRight ? (transform.position.x - startPosition.x) : (startPosition.x - transform.position.x);
            transform.Translate(new Vector3(movement, 0, 0));

            if (currentDistance >= distance)
            {
                stop = true;
                StartCoroutine(StopForDuration(stopTime));
            }
        }
        else
        {
            transform.Translate(Vector3.zero);
        }
    }
    private IEnumerator StopForAttacking(float duration)
    {
        if (stop)
        {
            animator.SetBool("walking", false);
            yield return new WaitForSeconds(duration);
            animator.SetBool("walking", true);
            stop = false;
        }
    }
    private IEnumerator StopForDuration(float duration)
    {
        if (stop)
        {
            animator.SetBool("walking", false);
            yield return new WaitForSeconds(duration);
            animator.SetBool("walking", true);
            movingRight = !movingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            stop = false;
        }
    }
    public void FireAttack()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
        {
            animator.SetTrigger("attack");
            cooldownTimer = 0;

            int index = FindFireball();
            fire[index].transform.position = spawnPoint.position;
            fire[index].ActivateProjectile();
            audioSource.PlayOneShot(cast);
        }
    }
    private int FindFireball()
    {
        for (int i = 0; i < fire.Length; i++)
        {
            if (!fire[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (closeBehavior && other.CompareTag("Player"))
        {
            stop = true;
            CloseAttack();
            base.OnTriggerEnter2D(other);
            StartCoroutine(StopForAttacking(stopTime));
        }
        if (other.CompareTag("Fireball") && !indestructible)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(1f, 0f, 0f, 1f);
            animator.SetTrigger("hurt");
            audioSource.PlayOneShot(hurt);
            health -= 1;
            if (health <= 0)
            {
                StartCoroutine(Dying());
            }
        }
    }
    private IEnumerator Dying()
    {
        closeVision.SetActive(false);
        stop = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<BoxCollider2D>().enabled = false;
        animator.SetTrigger("die");
        audioSource.PlayOneShot(death);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
