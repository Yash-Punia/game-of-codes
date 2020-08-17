using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanatizerScript : MonoBehaviour
{
    [SerializeField] float timeToWaiting = 15f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Person>())
        {
            if (other.GetComponent<Person>().isInfected)
            {
                other.GetComponent<Person>().SanatizingPerson(timeToWaiting);
                Destroy(gameObject);
            }
        }
    }
}
