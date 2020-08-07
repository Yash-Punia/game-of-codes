using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] GameObject cameraRig;
    [SerializeField] Image transitionPanel;
    [SerializeField] float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        cameraRig.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        if(Input.GetKey(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void PlayGame()
    {
        StartCoroutine(LoadPlayScene());
    }

    IEnumerator LoadPlayScene()
    {
        transitionPanel.GetComponent<Animator>().SetBool("NextScene",true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Yash");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
