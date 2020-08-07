using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManagerScript : MonoBehaviour
{
    [SerializeField] GameObject pauseGameMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        pauseGameMenu.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseGameMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseGameMenu.SetActive(false);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
