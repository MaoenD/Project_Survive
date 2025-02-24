using UnityEngine;
using TMPro;

public class Canvas : MonoBehaviour
{
    public TMP_Text BulletCountText;
    public TMP_Text InteractionText;
    public TMP_Text HealthText;
    public TMP_Text GameText;
    public TMP_Text TimerText;
    public GameObject Player;
    public GameObject Enemy;

    void Update()
    {
        UpdateBulletCount();
        UpdateInteractionText();
        UpdateHealthText();
        UpdateGameText();
        UpdateTimerText();
        CheckForRestart();
    }

    void UpdateBulletCount()
    {
        BulletCountText.text = "Bullets: " + Player.GetComponent<PlayerController>().BulletCount;
    }

    void UpdateInteractionText()
    {
        if (Player.GetComponent<PlayerController>().CanInteract)
        {
            InteractionText.text = "Interact [E]";
        }
        else
        {
            InteractionText.text = "";
        }
    }

    void UpdateHealthText()
    {
        HealthText.text = "Health: " + Player.GetComponent<PlayerController>().Health;
    }

    void UpdateGameText()
    {
        if (Player.GetComponent<PlayerController>().Health <= 0)
        {
            GameText.text = "Game Over! \n Press R to restart";
            InteractionText.text = "";
        }
        else if (Enemy.GetComponent<Enemy>().Health <= 0)
        {
            GameText.text = "You Win! \n Press R to restart";
            InteractionText.text = "";
        }
    }

    void UpdateTimerText()
    {
        if (Player.GetComponent<PlayerController>().Health > 0 && Enemy.GetComponent<Enemy>().Health > 0)
        {
            TimerText.text = "Time: " + Time.timeSinceLevelLoad.ToString("F2");
        }
    }

    void CheckForRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
