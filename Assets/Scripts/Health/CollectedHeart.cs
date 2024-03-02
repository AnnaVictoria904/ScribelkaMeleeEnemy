using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedHeart : MonoBehaviour
{
    [SerializeField] private float healthValue;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<HealthPlayer>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
