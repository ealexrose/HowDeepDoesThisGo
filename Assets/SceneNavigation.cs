using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneNavigation : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoseGame()
    {
        SceneManager.LoadScene("LoseScene");
    }

    public void PlayGame() 
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
