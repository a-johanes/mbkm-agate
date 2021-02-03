using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Pemain 1
    public PlayerControl player1; // skrip
    private Rigidbody2D _player1Rigidbody;

    // Pemain 2
    public PlayerControl player2; // skrip
    private Rigidbody2D _player2Rigidbody;

    // Bola
    public BallControl ball; // skrip
    private Rigidbody2D _ballRigidbody;
    private CircleCollider2D _ballCollider;

    // Skor maksimal
    public int maxScore;

    // Apakah debug window ditampilkan?
    private bool _isDebugWindowShown = false;

    // Objek untuk menggambar prediksi lintasan bola
    public Trajectory trajectory;

    // Dua jenis power up
    public GameObject fireBall;
    public double fireBallProbability = 1e-3;
    public float fireBallInitialForce = 10;
    public bool holdFireBall = true;
    private ArrayList _fireBallList = new ArrayList();

    public GameObject powerUp;

    public bool isPowerUpAvailable = false;


    // probabilitas power up
    public double powerUpProbability = 1e-3;

    /// Inisialisasi rigidbody dan collider
    void Start()
    {
        _player1Rigidbody = player1.GetComponent<Rigidbody2D>();
        _player2Rigidbody = player2.GetComponent<Rigidbody2D>();
        _ballRigidbody = ball.GetComponent<Rigidbody2D>();
        _ballCollider = ball.GetComponent<CircleCollider2D>();
        trajectory.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value < fireBallProbability && !holdFireBall)
        {
            Vector3 orientation;

            if (Random.value < 0.5f)
            {
                // attack player 1
                orientation = player1.transform.position;
            }
            else
            {
                // attack player 2
                orientation = player2.transform.position;
            }

            GameObject fireBallObject =
                Instantiate(fireBall, Vector3.zero, Quaternion.FromToRotation(Vector3.up, -orientation));
            fireBallObject.GetComponent<Rigidbody2D>().AddForce(orientation.normalized * fireBallInitialForce);
            
            _fireBallList.Add(fireBallObject);
        }

        if (isPowerUpAvailable)
        {
            if (Random.value < powerUpProbability)
            {
                float x = Random.Range(-10, 10);
                float y = Random.Range(-8, 8);
                powerUp.transform.position = new Vector3(x, y, 1f);
                powerUp.SetActive(true);
                isPowerUpAvailable = false;
            }
        }
    }

    // Untuk menampilkan GUI
    void OnGUI()
    {
        // Tampilkan skor pemain 1 di kiri atas dan pemain 2 di kanan atas
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + player1.Score);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2.Score);

        // Tombol restart untuk memulai game dari awal
        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART"))
        {
            // Ketika tombol restart ditekan, reset skor kedua pemain...
            player1.ResetScore();
            player2.ResetScore();

            // ...dan restart game.
            RestartSet();
            ball.RestartGame(2.0f);
        }

        // Jika pemain 1 menang (mencapai skor maksimal), ...
        if (player1.Score == maxScore)
        {
            // ...tampilkan teks "PLAYER ONE WINS" di bagian kiri layar...
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 10, 2000, 1000), "PLAYER ONE WINS");

            // ...dan kembalikan bola ke tengah.
            RestartSet();
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
        // Sebaliknya, jika pemain 2 menang (mencapai skor maksimal), ...
        else if (player2.Score == maxScore)
        {
            // ...tampilkan teks "PLAYER TWO WINS" di bagian kanan layar... 
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER TWO WINS");

            // ...dan kembalikan bola ke tengah.
            RestartSet();
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }

        // Toggle nilai debug window ketika pemain mengeklik tombol ini.
        if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 73, 120, 53), "TOGGLE\nDEBUG INFO"))
        {
            _isDebugWindowShown = !_isDebugWindowShown;
            trajectory.enabled = _isDebugWindowShown;
        }

        // Jika isDebugWindowShown == true, tampilkan text area untuk debug window.
        if (_isDebugWindowShown)
        {
            // Simpan nilai warna lama GUI
            Color oldColor = GUI.backgroundColor;

            // Beri warna baru
            GUI.backgroundColor = Color.red;

            // Simpan variabel-variabel fisika yang akan ditampilkan. 
            float ballMass = _ballRigidbody.mass;
            Vector2 ballVelocity = _ballRigidbody.velocity;
            float ballSpeed = ballVelocity.magnitude;
            Vector2 ballMomentum = ballMass * ballVelocity;
            float ballFriction = _ballCollider.friction;

            float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
            float impulsePlayer1Y = player1.LastContactPoint.tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
            float impulsePlayer2Y = player2.LastContactPoint.tangentImpulse;

            // Tentukan debug text-nya
            string debugText =
                "Ball mass = " + ballMass + "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 = (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 = (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n";

            // Tampilkan debug window
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width / 2 - 200, Screen.height - 200, 400, 110), debugText, guiStyle);

            // Kembalikan warna lama GUI
            GUI.backgroundColor = oldColor;
        }
    }

    public void RestartSet()
    {
        isPowerUpAvailable = false;
        holdFireBall = true;
        powerUp.SetActive(false);
        foreach (GameObject fireBallObject in _fireBallList)
        {
            Destroy(fireBallObject);
        }
        player1.RestartGame();
        player2.RestartGame();
    }
}