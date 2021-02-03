using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float health = 50f;

    public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

    private bool _isHit = false;

    void OnDestroy()
    {
        if (_isHit)
        {
            OnEnemyDestroyed(gameObject);
        }
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;

        if (col.gameObject.tag == "Bird")
        {
            _isHit = true;
            Destroy(gameObject);
        }
        else if(col.gameObject.tag == "Obstacle")
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
            _isHit = true;
            Destroy(gameObject);
        }
    }
}