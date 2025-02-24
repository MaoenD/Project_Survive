using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Speed = 2;
    public int Health = 10;
    public Rigidbody Rigbody;
    public GameObject Enemy;
    public bool CanInteract;
    public Chest Chest;
    public GameObject Lantern;
    public GameObject Pistol;

    public GameObject MuzzleFlash;
    public Animator LanternAnimator;
    public Animator GunAnimator;
    public AudioClip GunShot;
    public AudioClip AmbientSound;
    public AudioSource AudioSource;
    public AudioSource MusicSource;
    public int BulletCount;
    public bool Shoot;
    public float FireDelay = 2.0f;
    public float DamageDelay = 1.0f;

    private float fireTimer = 0.0f;
    private float shootTimer = 0.0f;
    private float damageTimer = 0.0f;

    void Start()
    {
        Rigbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Health <= 0) return;

        PlayAmbientMusic();
        HandleMovement();
        HandleInteraction();
        HandleShooting();
        UpdateTimers();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void PlayAmbientMusic()
    {
        if (!MusicSource.isPlaying)
        {
            MusicSource.clip = AmbientSound;
            MusicSource.Play();
        }
    }

    private void HandleMovement()
    {
        Vector3 CurrentSpeed = transform.forward * Input.GetAxis("Vertical") * Speed + transform.right * Input.GetAxis("Horizontal") * Speed;
        CurrentSpeed.y = GetComponent<Rigidbody>().linearVelocity.y;

        GetComponent<Rigidbody>().linearVelocity = CurrentSpeed;
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanInteract)
        {
            Chest.Open()
            ;
            if (Chest.Lantern) 
            {
                Lantern.SetActive(true);
            }

            if (Chest.Gun) 
            {
                Pistol.SetActive(true);
            }

            if (Chest.Bullet)
            {
                BulletCount+= 2;
                Chest.Bullet = false;
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && fireTimer <= 0 && Pistol.activeSelf && BulletCount > 0)
        {
            Shoot = true;
            AudioSource.PlayOneShot(GunShot);
            FirePistol();
            BulletCount--;
        }
    }

    private void FirePistol()
    {
        fireTimer = FireDelay;
        GunAnimator.SetTrigger("Fire");
        shootTimer = 0.5f;
        StartCoroutine(ShowMuzzleFlash());
    }

    private IEnumerator ShowMuzzleFlash()
    {
        MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        MuzzleFlash.SetActive(false);
    }

    private void UpdateTimers()
    {
        if (fireTimer > 0) 
        {
            fireTimer -= Time.deltaTime;
        }

        if (shootTimer > 0) {
            shootTimer -= Time.deltaTime;
        }

        else 
        {
            Shoot = false;
        }

        if (damageTimer > 0) 
        { 
            damageTimer -= Time.deltaTime; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Chest newChest = other.gameObject.GetComponent<Chest>();
        if (newChest && newChest != Chest) 
        { 
            Chest = newChest;
        }

        if (Chest) 
        { 
            CanInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CanInteract = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Enemy && Shoot && damageTimer <= 0)
        {
            Enemy.GetComponent<Enemy>().Health -= 1;
            damageTimer = DamageDelay;
        }
    }
}
