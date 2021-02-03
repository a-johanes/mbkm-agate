using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameManager gameManager;
    
    // durasi power up
    public float duration = 10.0f;
    // multiplier untuk ukuran
    public float size = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameManager.isPowerUpAvailable = true;
        gameObject.SetActive(false);

        BallControl tempBall = other.GetComponent<BallControl>();
        if (tempBall)
        {
            tempBall.LastPlayer.PowerUp(duration, size);
        }
    }
}
