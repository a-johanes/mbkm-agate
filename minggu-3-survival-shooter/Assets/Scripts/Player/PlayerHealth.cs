using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [HideInInspector] public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public Color speedUpColour = new Color(0f, 1f, 0f, 0.1f);
    public Color healUpColour = new Color(1f, 1f, 0f, 0.1f);


    private Animator anim;
    private bool damaged;
    private bool healUp;
    private bool isDead;
    private AudioSource playerAudio;
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;

    private void Awake()
    {
        //Mendapatkan refernce komponen
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = maxHealth;
    }


    private void Update()
    {
        //Jika terkena damaage
        if (damaged)
            //Merubah warna gambar menjadi value dari flashColour
            damageImage.color = flashColour;    
        else if (playerMovement.isSpeedUp)
            damageImage.color = speedUpColour;
        else if (healUp)
            damageImage.color = healUpColour;
        else
            //Fade out damage image
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

        //Set damage to false
        damaged = false;
        healUp = false;
    }

    //fungsi untuk mendapatkan damage
    public void TakeDamage(int amount)
    {
        damaged = true;

        //mengurangi health
        currentHealth -= amount;

        //Merubah tampilan dari health slider
        healthSlider.value = currentHealth;

        //Memainkan suara ketika terkena damage
        playerAudio.Play();

        //Memanggil method Death() jika darahnya kurang dari sama dengan 0 dan belu mati
        if (currentHealth <= 0 && !isDead) Death();
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Math.Min(currentHealth, maxHealth);
        healthSlider.value = currentHealth;
        healUp = true;
    }


    private void Death()
    {
        isDead = true;

        //playerShooting.DisableEffects ();

        //mentrigger animasi Die
        anim.SetTrigger("Die");

        //Memainkan suara ketika mati
        playerAudio.clip = deathClip;
        playerAudio.Play();

        //mematikan script player movement
        playerMovement.enabled = false;

        playerShooting.enabled = false;
    }


    public void RestartLevel()
    {
        //meload ulang scene dengan index 0 pada build setting
        SceneManager.LoadScene(0);
    }
}