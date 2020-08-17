using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GuideCanvasScript : MonoBehaviour
{
    [SerializeField] GameObject Text1;
    [SerializeField] GameObject Text2;

    private void Start()
    {
        Text1.SetActive(true);
    }
    public void MoveForward()
    {
        Text1.SetActive(false);
        Text2.SetActive(true);
    }
    public void MoveBackward()
    {

        Text1.SetActive(true);
        Text2.SetActive(false);
    }
}
