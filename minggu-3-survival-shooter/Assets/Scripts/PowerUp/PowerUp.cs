using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp : MonoBehaviour
{
    public float speed = 20;
    public Transform center;

    public UnityAction onPowerUpDestroy = delegate { };

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.RotateAround(center.position, Vector3.up, speed * Time.deltaTime);
    }

    protected void End()
    {
        _audioSource.Play();
        onPowerUpDestroy();
        transform.position += Vector3.down * 100;
        Destroy(gameObject, _audioSource.clip.length);
    }
}