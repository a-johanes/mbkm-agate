using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    
    // Rigidbody 2D bola
    private Rigidbody2D _rigidBody2D;
 
    // Besarnya gaya awal yang diberikan untuk mendorong bola
    public float xInitialForce;
    public float yInitialForce;
    
    // Titik asal lintasan bola saat ini
    private Vector2 _trajectoryOrigin;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
 
        // Mulai game
        RestartGame();
        
        _trajectoryOrigin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Ketika bola beranjak dari sebuah tumbukan, rekam titik tumbukan tersebut
    private void OnCollisionExit2D(Collision2D collision)
    {
        _trajectoryOrigin = transform.position;
    }
    
    private void ResetBall()
    {
        // Reset posisi menjadi (0,0)
        transform.position = Vector2.zero;
 
        // Reset kecepatan menjadi (0,0)
        _rigidBody2D.velocity = Vector2.zero;
    }
    
    private void PushBall()
    {
        // Tentukan nilai komponen y dari gaya dorong antara -yInitialForce dan yInitialForce
        float yRandomInitialForce = Random.Range(-yInitialForce, yInitialForce);

        // Tentukan nilai acak antara 0 (inklusif) dan 2 (eksklusif)
        float randomDirection = Random.Range(0, 2);

        // Jika nilainya di bawah 1, bola bergerak ke kiri. 
        // Jika tidak, bola bergerak ke kanan.
        if (randomDirection < 1.0f)
        {
            // Gunakan gaya untuk menggerakkan bola ini.
            _rigidBody2D.AddForce(new Vector2(-xInitialForce, yRandomInitialForce));
        }
        else
        {
            _rigidBody2D.AddForce(new Vector2(xInitialForce, yRandomInitialForce));
        }
    }
    
    void RestartGame()
    {
        // Kembalikan bola ke posisi semula
        ResetBall();
 
        // Setelah 2 detik, berikan gaya ke bola
        Invoke(nameof(PushBall), 2);
    }
    
    // Untuk mengakses informasi titik asal lintasan
    public Vector2 TrajectoryOrigin => _trajectoryOrigin;
}
