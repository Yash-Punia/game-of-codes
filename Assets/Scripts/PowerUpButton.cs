using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpButton : MonoBehaviour
{
    //Using Person Prefab from PopulationController
    [SerializeField] int personCost = 50;


    [SerializeField] GameObject healthBooster;
    [SerializeField] int healthBoosterCost = 20;


    [SerializeField] GameObject immunityDrink;
    [SerializeField] int immunityDrinkCost = 20;


    // Lockdown doesn't need a gameObject Prefab
    // Need to make a particle effect
    [SerializeField] int lockdownPowerUpCost = 20;


    [SerializeField] GameObject maskPowerUp;
    [SerializeField] int MaskCost = 20;


    [SerializeField] GameObject selfQuarantine;
    [SerializeField] int selfQuarantineCost = 20;


    [SerializeField] GameObject sanatizer;
    [SerializeField] int sanatizeCost = 20;

    CoinController coinController;

    private void Start()
    {
        coinController = FindObjectOfType<CoinController>();
    }

    public void ImmunityDrink()
    {
        FindObjectOfType<CameraController>().isPanning = false;
        if (coinController.totalCoins >= immunityDrinkCost)
        {
            Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newPower = Instantiate(immunityDrink, positionOfClick - new Vector3(0,2,0), Quaternion.identity);
            coinController.DecreaseCoins(immunityDrinkCost);
        }
    }
    public void HealthBooster()
    {
        FindObjectOfType<CameraController>().isPanning = false;
        if (coinController.totalCoins >= healthBoosterCost)
        {
            Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newPower = Instantiate(healthBooster, positionOfClick - new Vector3(0, 2, 0), Quaternion.identity);
            coinController.DecreaseCoins(healthBoosterCost);
        }
    }

    public void MaskPowerUp()
    {
        FindObjectOfType<CameraController>().isPanning = false;
        if (coinController.totalCoins >= MaskCost)
        {
            Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newPower = Instantiate(maskPowerUp, positionOfClick - new Vector3(0, 2, 0), Quaternion.identity);
            coinController.DecreaseCoins(MaskCost);
        }
    }

    public void PersonPowerUp()
    {
        FindObjectOfType<CameraController>().isPanning = false;
        if (coinController.totalCoins >= personCost)
        {
            Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GetComponent<PopulationController>().AddPerson();
            coinController.DecreaseCoins(personCost);
        }
        FindObjectOfType<CameraController>().isPanning = true;
    }

     public void LockdownPowerUp()
    {
        FindObjectOfType<CameraController>().isPanning = false;
        if (coinController.totalCoins >= lockdownPowerUpCost)
        {
            Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Person[] allPerson = FindObjectsOfType<Person>();
            foreach(Person person in allPerson)
            {
                StartCoroutine(LockDown());
                IEnumerator LockDown()
                {
                    person.GetComponent<Person>().isMovementAllowed = false;
                    person.GetComponent<Person>().anim.SetBool("Walking", false);
                    yield return new WaitForSeconds(10f);
                    person.GetComponent<Person>().isMovementAllowed = true;
                    person.GetComponent<Person>().anim.SetBool("Walking", true);
                }
            }
            coinController.DecreaseCoins(lockdownPowerUpCost);
        }
        
        FindObjectOfType<CameraController>().isPanning = true;
     }

    public void SelfQuarantine()
    {

        FindObjectOfType<CameraController>().isPanning = false;
        if (coinController.totalCoins >= selfQuarantineCost)
        {
            Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newPower = Instantiate(selfQuarantine, positionOfClick - new Vector3(0, 2, 0), Quaternion.identity);
            coinController.DecreaseCoins(selfQuarantineCost);
        }
    }

    public void SanatizerArea()
    {
        FindObjectOfType<CameraController>().isPanning = false;
        if (coinController.totalCoins >= sanatizeCost)
        {
            Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newPower = Instantiate(sanatizer, positionOfClick - new Vector3(0, 2, 0), Quaternion.identity);
            coinController.DecreaseCoins(sanatizeCost);
        }
    }

}
