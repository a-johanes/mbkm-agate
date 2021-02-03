using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class FireBall : MonoBehaviour
{
    private GameManager _gameManager;
    private GameObject _ball;
    private PlayerControl _player1, _player2;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _ball = GameObject.Find("Ball");
        _player1 = GameObject.Find("Player1").GetComponent<PlayerControl>();
        _player2 = GameObject.Find("Player2").GetComponent<PlayerControl>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerControl tempPlayer = other.GetComponent<PlayerControl>();
        if (tempPlayer)
        {
            float score = 0;
            if (tempPlayer.name.Equals("Player1"))
            {
                _player2.IncrementScore();
                score = _player2.Score;
            }
            else if (tempPlayer.name.Equals("Player2"))
            {
                _player1.IncrementScore();
                score = _player1.Score;
            }
            
            // Jika skor pemain belum mencapai skor maksimal...
            if (score < _gameManager.maxScore)
            {
                // ...restart game setelah bola mengenai dinding.
                _ball.SendMessage("RestartGame", 1.0f, SendMessageOptions.RequireReceiver);
                _gameManager.RestartSet();
            }
        }

        if (!other.GetComponent<FireBall>())
        {
            Destroy(gameObject);
        }
    }
}
