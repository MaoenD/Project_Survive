using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator Animator;

    public bool Lantern;
    public bool Gun;
    public bool Bullet;

    public AudioSource AudioSource;
    public AudioClip OpenSound;

    public void Open()
    {
        Animator.SetBool("Open", true);
        AudioSource.PlayOneShot(OpenSound);
    }
}
