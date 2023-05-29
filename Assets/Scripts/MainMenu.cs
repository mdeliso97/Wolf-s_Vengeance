using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    Wolf_Movement wolfController;
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Closed");
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
