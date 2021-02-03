using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBird : Bird
{
    public float radius;
    public float force;

    public List<LayerMask> layersToHit;

    public GameObject ExplosionEffect;
    
    private void Explode(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Obstacle") && !col.gameObject.CompareTag("Enemy")) return;
        foreach (var layerMask in layersToHit)
        {
            var objects = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
            foreach (var obj in objects)
            {
                Vector2 dir = obj.transform.position - transform.position;
                obj.GetComponent<Rigidbody2D>().AddForce(dir * force);
                float damage = obj.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
                obj.BroadcastMessage("Hit", damage,SendMessageOptions.RequireReceiver);
            }
        }

        GameObject explosionEffectIns = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(explosionEffectIns, 1);
        // StartCoroutine(DestroyAfter(1));
    }

    protected override void OnCollision(Collision2D col)
    {
        Explode(col);
    }
    
    // private IEnumerator DestroyAfter(float second)
    // {
    //     yield return new WaitForSeconds(second);
    //     Destroy(ExplosionEffect);
    // }
}
