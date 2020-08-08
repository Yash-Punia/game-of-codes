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
    [SerializeField] int lockdownPowerUpCost = 30;


    [SerializeField] GameObject maskPowerUp;
    [SerializeField] int MaskCost = 20;


    [SerializeField] GameObject selfQuarantine;
    [SerializeField] int selfQuarantineCost = 50;


    [SerializeField] GameObject sanatizer;
    [SerializeField] int sanatizeCost = 30;

    [SerializeField] AudioClip buttonSelect;
    
    CoinController coinController;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        coinController = FindObjectOfType<CoinController>();
    }

    public void ImmunityDrink()
    {
        PlayButtonSound();
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
        PlayButtonSound();
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
        PlayButtonSound();
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
        PlayButtonSound();
        FindObjectOfType<CameraController>().isPanning = false;
        if (coinController.totalCoins >= personCost)
        {
            GetComponent<PopulationController>().AddPerson();
            coinController.DecreaseCoins(personCost);
        }
        FindObjectOfType<CameraController>().isPanning = true;
    }

     public void LockdownPowerUp()
    {
        PlayButtonSound();
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
        PlayButtonSound();
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
        PlayButtonSound();
        FindObjectOfType<CameraController>().isPanning = false;
        if (coinController.totalCoins >= sanatizeCost)
        {
            Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newPower = Instantiate(sanatizer, positionOfClick - new Vector3(0, 2, 0), Quaternion.identity);
            coinController.DecreaseCoins(sanatizeCost);
        }
    }

    public void PlayButtonSound()
    {
        source.clip = buttonSelect;
        source.Play();
    }
}
