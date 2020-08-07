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


    [SerializeField] GameObject lockDownPowerUp;
    [SerializeField] int lockdownPowerUpCost = 20;

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
            GetComponent<PopulationController>().LockDownEffect();
            coinController.DecreaseCoins(lockdownPowerUpCost);
        }
        
        FindObjectOfType<CameraController>().isPanning = true;
    }
}
