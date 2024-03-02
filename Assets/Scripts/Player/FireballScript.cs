using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float speed;
    private Animator animator;
    private bool hit;
    private BoxCollider2D collider;
    private float direction;
    private float lifeTime;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            return;
        }
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
        lifeTime += Time.deltaTime;
        if (lifeTime > 5)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetDirection(float direction)
    {
        //body.velocity = new Vector2(direction * speed, 0f);

        lifeTime = 0.0f;
        this.direction = direction;
        gameObject.SetActive(true);
        collider.enabled = true;
        hit = false;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction)
        {
            transform.localScale = new Vector3(-localScaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("explote");
    }*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        hit = true;
        collider.enabled = false;
        animator.SetTrigger("explote");
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
