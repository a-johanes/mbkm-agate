using System;
using UnityEngine;

public class EnemyFactory : MonoBehaviour, IFactory{

    [SerializeField]
    public GameObject[] enemyPrefab;

    public GameObject FactoryMethod(int enemyTag, Vector3 position, Quaternion rotation)
    {
        GameObject enemy = Instantiate(enemyPrefab[enemyTag], position, rotation);
        return enemy;
    }
}