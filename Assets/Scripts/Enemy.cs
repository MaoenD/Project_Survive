using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent Agent;

    public BoxCollider HitCollider;
    public GameObject Player;
    public Animator Animator;
    public AudioSource AudioSource;
    public AudioClip AttackSound;
    public AudioClip DeathSound;
    public AudioClip WalkSound;
    public int Health;
    public bool Pursuing;
    public int AttackDistance = 5;
    public int AttackInterval = 3;
    public int Speed;


    private float timeSinceLastAttack;
    private bool isDead = false;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        timeSinceLastAttack = 0f;
    }

    private void Update()
    {
        if (isDead) return;

        MoveTowardsPlayer();
        HandlePursuit();
        HandleAttack();
        CheckDeath();
    }

    private void MoveTowardsPlayer()
    {
        Agent.SetDestination(Player.transform.position);
    }

    private void HandlePursuit()
    {
        if (Pursuing)
        {
            Speed = 4;
            Animator.SetBool("pursue", true);
            PlaySound(WalkSound);
        }
        else
        {
            Speed = 2;
            Animator.SetBool("pursue", false);
            StopSound(WalkSound);
        }
    }

    private void HandleAttack()
    {
        if (Pursuing && Player.GetComponent<PlayerController>().Health > 0)
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= AttackInterval)
            {
                Player.GetComponent<PlayerController>().Health -= 1;
                timeSinceLastAttack = 0f;
                PlaySound(AttackSound, true);
                HitCollider.enabled = false;
            }
            else if (timeSinceLastAttack >= AttackInterval - 0.5f)
            {
                HitCollider.enabled = true;
            }
        }
    }

    private void CheckDeath()
    {
        if (Health <= 0 && !isDead)
        {
            isDead = true;
            Animator.SetBool("dead", true);
            PlaySound(DeathSound, true);
        }
    }

    private void PlaySound(AudioClip clip, bool oneShot = false)
    {
        if (oneShot)
        {
            AudioSource.PlayOneShot(clip);
        }
        else if (!AudioSource.isPlaying || AudioSource.clip != clip)
        {
            AudioSource.clip = clip;
            AudioSource.Play();
        }
    }

    private void StopSound(AudioClip clip)
    {
        if (AudioSource.clip == clip)
        {
            AudioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Pursuing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Pursuing = false;
        }
    }
}
