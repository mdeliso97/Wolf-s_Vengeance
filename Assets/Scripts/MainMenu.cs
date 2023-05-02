using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    wolfController wolfController;
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
        wolfController.hitCooldownTime = wolfController.hitCooldownTime + 10f;
    }

    public void addRecoveryTime()
    {
        SceneManager.LoadScene("GameScene");
    }
}
