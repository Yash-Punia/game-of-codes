using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManagerScript : MonoBehaviour
{
    [SerializeField] GameObject pauseGameMenu;
    [SerializeField] Canvas UIMainCanvas;
    int numberOfPersonDied=0;
    [SerializeField] GameObject LoseCanvas;
    [SerializeField] AudioClip loseClip;
    [SerializeField] AudioClip winClip;
    private AudioSource source;
    [SerializeField] Slider winSlider, loseSlider;
    //[SerializeField] Canvas WinCanvas;
    // Start is called before the first frame update

    // this lose slider is giving reference error.
    void Start()
    {
        source = GetComponent<AudioSource>();
        //loseSlider.GetComponent<Slider>().value = numberOfPersonDied; 
        Time.timeScale = 1f;
        pauseGameMenu.SetActive(false);
        LoseCanvas.SetActive(false);
    }
    public void peopleDied()
    {
        numberOfPersonDied += 1;
        UpdateLoseSlider(numberOfPersonDied);
        if (numberOfPersonDied >= 10)
        {
            StartCoroutine(GameLose());
            IEnumerator GameLose()
            {
                source.clip = loseClip;
                source.Play();
                yield return new WaitForSeconds(1.5f);
                LoseCanvas.SetActive(true);
                UIMainCanvas.enabled = false;
                Time.timeScale = 0f;
            }
        }
        
    }
    
    public void UpdateLoseSlider(int Value)
    {
        if(loseSlider)
        loseSlider.GetComponent<Slider>().value = Value;
    }

    public void UpdateWinSlider(int Value)
    {
        winSlider.GetComponent<Slider>().value = Value;
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Title");
    }
   /*
    public void GameFinished()
    {
        StartCoroutine(GameWin());
        IEnumerator GameWin()
        {
            yield return new WaitForSeconds(1.5f);
            WinCanvas.enabled = true;
        }
    }
   */
   


    public void PauseGame()
    {
        if (!FindObjectOfType<HelperCanvas>().isAnyCanvasOpened)
        {
            GetComponent<PowerUpButton>().PlayButtonSound();
            FindObjectOfType<HelperCanvas>().isAnyCanvasOpened = true;
            Time.timeScale = 0f;
            pauseGameMenu.SetActive(true);
        }
    }

    public void PlayGameWonSound()
    {

        source.clip = winClip;
        source.Play();
    }




    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseGameMenu.SetActive(false);
        FindObjectOfType<HelperCanvas>().isAnyCanvasOpened = false;
    }

    public void BackToTitle()
    {
        FindObjectOfType<HelperCanvas>().isAnyCanvasOpened = false;
        SceneManager.LoadScene("Title");
    }

    public void DisableUICanvas()
    {
        UIMainCanvas.enabled = false;
    }
    public void EnableUICanvas()
    {
        UIMainCanvas.enabled = true;
    }
}
