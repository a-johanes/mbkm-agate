using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public PowerUp[] powerUps;
    public float[] spawnProbabilities;
    public float spawnTime = 3f;

    public float xLimit = 23f;
    public float zLimit = 23f;

    public int maxNumberOfSpawn = 10;
    [HideInInspector] public int currentNumberOfSpawn;

    private int _layerMask;

    // Start is called before the first frame update
    private void Start()
    {
        _layerMask = LayerMask.GetMask("Environment");
        InvokeRepeating(nameof(Spawn), spawnTime, spawnTime);
    }


    private void Spawn()
    {
        if (currentNumberOfSpawn >= maxNumberOfSpawn) return;
        
        var chosenPowerUpIndex = 0;
        var randomPowerUpProbability = Random.Range(0, 100);
        float currentProbability = 0;

        for (var i = 0; i < spawnProbabilities.Length; i++)
        {
            currentProbability += spawnProbabilities[i];
            if (randomPowerUpProbability <= currentProbability)
            {
                chosenPowerUpIndex = i;
                break;
            }
        }

        var powerUp = powerUps[chosenPowerUpIndex];

        var canSpawn = false;
        var position = Vector3.zero;
        var steps = 0;

        while (!canSpawn)
        {
            var x = Random.Range(-xLimit, xLimit);
            var z = Random.Range(-zLimit, zLimit);
            var y = powerUp.transform.localPosition.y;

            position = new Vector3(x, y, z);
            canSpawn = CanSpawn(position);
            steps++;

            if (steps >= 100) break;
        }


        PowerUp powerUpInstance = Instantiate(powerUp, position, Quaternion.identity);
        powerUpInstance.onPowerUpDestroy = OnPowerUpDestroy;
        currentNumberOfSpawn++;
    }

    private bool CanSpawn(Vector3 position)
    {
        var colliders = Physics.OverlapSphere(position, 0.5f, _layerMask);
        return colliders.Length == 0;
    }

    private void OnPowerUpDestroy()
    {
        currentNumberOfSpawn--;
    }
}