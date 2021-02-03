using UnityEngine;
using UnityEngine.Events;

public class Wood : MonoBehaviour
{
    public float health = 30f;

    public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;
        
        if(col.gameObject.CompareTag("Bird"))
        {
            //Hitung damage yang diperoleh
            float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            Hit(damage);
        }
    }

    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}