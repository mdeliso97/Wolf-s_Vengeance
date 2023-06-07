using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{

    Wolf_Movement wolfController;
    public void MenuButton()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void addHeart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void addSpeed()
    {
        // SceneManager.LoadScene("GameScene");
        wolfController.hitCooldownTime = wolfController.hitCooldownTime + 0.5f;
    }

    public void addRecoveryTime()
    {
        SceneManager.LoadScene("GameScene");
    }
}
