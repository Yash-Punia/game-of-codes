using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperCanvas : MonoBehaviour
{
    public bool isAnyCanvasOpened = false;
    [SerializeField] GameObject UIHelperCanvas;
    private bool isHelperUIOn = false;

    public void SetHelperCanvasOffOn()
    {
        if (!isAnyCanvasOpened)
        {
            GetComponent<PowerUpButton>().PlayButtonSound();
            if (!isHelperUIOn)
            {
                UIHelperCanvas.SetActive(true);
                isAnyCanvasOpened = true;
                isHelperUIOn = true;
            }
        }
        else
        {
            if (isHelperUIOn)
            {
                UIHelperCanvas.SetActive(false);
                isHelperUIOn = false;
                isAnyCanvasOpened = false;
            }
        }
    }    
}
