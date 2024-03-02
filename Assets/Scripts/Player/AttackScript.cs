using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField] private float attackDelay;
    [SerializeField] private Transform firePoint;
    [SerializeField] private FireballScript[] fireBalls;
    private Animator animator;
    private PlayerMovement _playerMovement;
    private float currentTime = Mathf.Infinity;
    private AudioSource audioSource;
    [SerializeField] private AudioClip cast;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Player without animator");
        }
        _playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (Input.GetMouseButton(0) && currentTime > attackDelay && _playerMovement.canAttack())
        {
            Attack();
        }
    }
    private void Attack()
    {
        animator.SetTrigger("attack");
        audioSource.PlayOneShot(cast);
        currentTime = 0f;
        /*GameObject o = Instantiate(prefabProjectile, spawnProjectile.transform);
        o.GetComponent<FireballScript>().SetDirection(transform.localScale.x);*/

        int index = FindFireball();
        fireBalls[index].transform.position = firePoint.position;
        fireBalls[index].SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }

        return 0;
    }
}
