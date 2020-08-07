using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShield : MonoBehaviour
{
    public void DecreasePowerShieldSize()
    {
        //this.gameObject.SetActive(true);
        this.gameObject.GetComponent<Transform>().localScale -= new Vector3(1, 1, 1);
    }
    public void IncreasePowerShieldSize()
    {
        //this.gameObject.SetActive(true);
        this.gameObject.GetComponent<Transform>().localScale += new Vector3(1, 1, 1);
    }
}
