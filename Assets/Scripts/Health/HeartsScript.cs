using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsScript : MonoBehaviour
{
    [SerializeField] private Image lives;
    private HealthPlayer healthPlayer;
    private void Awake()
    {
        healthPlayer = GetComponent<HealthPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        lives.fillAmount = (healthPlayer.currentHealth / 10f);
    }
}
