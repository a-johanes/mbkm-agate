using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird
{
    [SerializeField]
    public float boostForce = 100;

    private bool _hasBoost;

    public void Boost()
    {
        if (State == BirdState.Thrown && !_hasBoost)
        {
            RigidBody.AddForce(RigidBody.velocity * boostForce);
            _hasBoost = true;
        }
    }
    
    public override void OnTap()
    {
        Boost();
    }
}
