using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    public float[] spawnProbabilities;

    [SerializeField] private MonoBehaviour factory;

    private IFactory Factory => factory as IFactory;

    private void Start()
    {
        //Mengeksekusi fungs Spawn setiap beberapa detik sesui dengan nilai spawnTime
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    private void Spawn()
    {
        //Jika player telah mati maka tidak membuat enemy baru
        if (playerHealth.currentHealth <= 0f) return;

        //Mendapatkan nilai random
        var spawnPointIndex = Random.Range(0, spawnPoints.Length);
        var randomEnemyProbability = Random.Range(0, 100);
        
        var spawnEnemy = 0;
        var currentProbability = 0f;
        for (var i = 0; i < spawnProbabilities.Length; i++)
        {
            currentProbability += spawnProbabilities[i];
            if (randomEnemyProbability <= currentProbability)
            {
                spawnEnemy = i;
                break;
            }
        }

        var spawnPoint = spawnPoints[spawnPointIndex];
        //Memduplikasi enemy
        Factory.FactoryMethod(spawnEnemy, spawnPoint.position, spawnPoint.rotation);
    }
}