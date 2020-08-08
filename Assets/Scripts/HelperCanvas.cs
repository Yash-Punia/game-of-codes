using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperCanvas : MonoBehaviour
{
    [SerializeField] GameObject UIHelperCanvas;
    private bool isHelperUIOn = true;

    public void SetHelperCanvasOffOn()
    {
        if (isHelperUIOn)
        {
            UIHelperCanvas.SetActive(false);
            isHelperUIOn = false;
        }
        else
        {
            UIHelperCanvas.SetActive(true);
            isHelperUIOn = true;
        }
    }

    
}
