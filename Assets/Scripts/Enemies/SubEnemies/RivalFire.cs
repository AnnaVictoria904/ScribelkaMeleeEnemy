using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalFire : Enemy
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    [SerializeField] private Transform rivalTransform;
    private float lifeTime;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.right = rivalTransform.right;
    }
    public void ActivateProjectile()
    {
        lifeTime = 0;
        gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifeTime += Time.deltaTime;
        if (lifeTime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        animator.SetTrigger("explote");
        gameObject.SetActive(false);
    }
}
