using System;
using UnityEngine;

public class SpeedUp : PowerUp
{
    public float duration = 5.0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && other.isTrigger == false)
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (!playerMovement.isSpeedUp)
            {
                playerMovement.SpeedUp(duration);
                End();
            }
        }
    }
}
