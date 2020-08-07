using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionShield : MonoBehaviour
{

    public void DecreaseInfectionShieldSize()
    {
        this.gameObject.GetComponent<Transform>().localScale -= new Vector3(1, 1, 1);
    }
    public void IncreaseInfectionShieldSize()
    {
        this.gameObject.GetComponent<Transform>().localScale += new Vector3(1, 1, 1);
    }
}
