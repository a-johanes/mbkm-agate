using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AddHealth : PowerUp
{
    public int amount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && other.isTrigger == false)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth.currentHealth < playerHealth.maxHealth)
            {
                playerHealth.AddHealth(amount);
                End();
            }
        }
    }
}
