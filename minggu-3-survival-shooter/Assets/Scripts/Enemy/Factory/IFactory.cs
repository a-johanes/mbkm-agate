using UnityEngine;

public interface IFactory
{
    GameObject FactoryMethod(int enemyTag, Vector3 position, Quaternion rotation);
}
