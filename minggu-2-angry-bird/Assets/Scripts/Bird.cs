using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    [HideInInspector] public GameObject parent;
    
    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    protected Rigidbody2D RigidBody;
    private CircleCollider2D _collider;

    private float _minVelocity = 0.05f;
    private bool _flagDestroy;

    public BirdState State { get; private set; }

    void Start()
    {
        RigidBody = gameObject.GetComponent<Rigidbody2D>();
        _collider = gameObject.GetComponent<CircleCollider2D>();
        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        _collider.enabled = false;
        State = BirdState.Idle;
    }

    void FixedUpdate()
    {
        if(State == BirdState.Idle && 
           RigidBody.velocity.sqrMagnitude >= _minVelocity)
        {
            State = BirdState.Thrown;
        }

        if ((State == BirdState.Thrown || State == BirdState.HitSomething) &&
            RigidBody.velocity.sqrMagnitude < _minVelocity &&
            !_flagDestroy)
        {
            //Hancurkan gameobject setelah 2 detik
            //jika kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }

    }
    
    void OnDestroy()
    {
        if(State == BirdState.Thrown || State == BirdState.HitSomething)
            OnBirdDestroyed();
    }
    
    public void OnCollisionEnter2D(Collision2D col)
    {
        State = BirdState.HitSomething;
        OnCollision(col);
    }
    
    protected virtual void OnCollision(Collision2D col)
    {
        // Do nothing
    }

    public virtual void OnTap()
    {
        //Do nothing
    }
    
    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        transform.SetParent(parent.transform);
        transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        _collider.enabled = true;
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        RigidBody.velocity = velocity * speed * distance;
        OnBirdShot(this);
    }
    
    

}
