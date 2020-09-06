using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpButton : MonoBehaviour
{
    //Using Person Prefab from PopulationController
    [SerializeField] int personCost = 50;


    [SerializeField] GameObject healthBooster;
    [SerializeField] int healthBoosterCost = 20;
    [SerializeField] TextMeshProUGUI healthBoosterText;


    [SerializeField] GameObject immunityDrink;
    [SerializeField] int immunityDrinkCost = 20;
    [SerializeField] TextMeshProUGUI immunityDrinkText;


    // Lockdown doesn't need a gameObject Prefab
    // Need to make a particle effect
    [SerializeField] int lockdownPowerUpCost = 30;
    [SerializeField] TextMeshProUGUI lockDownText;


    [SerializeField] GameObject maskPowerUp;
    [SerializeField] int MaskCost = 20;
    [SerializeField] TextMeshProUGUI maskText;


    [SerializeField] GameObject selfQuarantine;
    [SerializeField] int selfQuarantineCost = 50;
    [SerializeField] TextMeshProUGUI selfQuarantineText;


    [SerializeField] GameObject sanatizer;
    [SerializeField] int sanatizeCost = 30;
    [SerializeField] TextMeshProUGUI sanatizerText;

    [SerializeField] TextMeshProUGUI personText;

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
        if (!GetComponent<HelperCanvas>().isAnyCanvasOpened)
        {
            PlayButtonSound();
            FindObjectOfType<CameraController>().isPanning = false;
            if (coinController.totalCoins >= immunityDrinkCost)
            {
                Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var newPower = Instantiate(immunityDrink, positionOfClick - new Vector3(0, 2, 0), Quaternion.identity);
                coinController.DecreaseCoins(immunityDrinkCost);
                immunityDrinkCost += 5;
                if (immunityDrinkText)
                {
                    immunityDrinkText.text = immunityDrinkCost.ToString();
                }
            }
            else
            {
                FindObjectOfType<CameraController>().isPanning = true;
            }
        }
    }
    public void HealthBooster()
    {
        if (!GetComponent<HelperCanvas>().isAnyCanvasOpened)
        {
            PlayButtonSound();
            FindObjectOfType<CameraController>().isPanning = false;
            if (coinController.totalCoins >= healthBoosterCost)
            {
                Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var newPower = Instantiate(healthBooster, positionOfClick - new Vector3(0, 2, 0), Quaternion.identity);
                coinController.DecreaseCoins(healthBoosterCost);
                healthBoosterCost += 5;
                if (healthBoosterText)
                {
                    healthBoosterText.text = healthBoosterCost.ToString();
                }
            }
            else
            {
                FindObjectOfType<CameraController>().isPanning = true;
            }
        }
    }

    public void MaskPowerUp()
    {
        if (!GetComponent<HelperCanvas>().isAnyCanvasOpened)
        {
            PlayButtonSound();
            FindObjectOfType<CameraController>().isPanning = false;
            if (coinController.totalCoins >= MaskCost)
            {
                Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var newPower = Instantiate(maskPowerUp, positionOfClick - new Vector3(0, 2, 0), Quaternion.identity);
                MaskCost += 5;
                coinController.DecreaseCoins(MaskCost);
                if (maskText)
                {
                    maskText.text = MaskCost.ToString();
                }
            }
            else
            {
                FindObjectOfType<CameraController>().isPanning = true;
            }
        }

    }

    public void UpdatePersonCostText(int value)
    {
        if(personText)
        {
            personText.text = value.ToString();
        }
    }

    public void PersonPowerUp()
    {
        if (!GetComponent<HelperCanvas>().isAnyCanvasOpened)
        {
            PlayButtonSound();
            FindObjectOfType<CameraController>().isPanning = false;
            if (coinController.totalCoins >= personCost)
            {
                GetComponent<PopulationController>().AddPerson();
                coinController.DecreaseCoins(personCost);
                if (personText)
                {
                    personText.text = personCost.ToString();
                }
            }
            else
            {
                FindObjectOfType<CameraController>().isPanning = true;
            }
            FindObjectOfType<CameraController>().isPanning = true;
        }
    }

    public int timesQuarantineCentreUsed = 0;
    public void LockdownPowerUp()
    {
        if (!GetComponent<HelperCanvas>().isAnyCanvasOpened)
        {
            PlayButtonSound();
            FindObjectOfType<CameraController>().isPanning = false;
            if (coinController.totalCoins >= lockdownPowerUpCost)
            {
                Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Person[] allPerson = FindObjectsOfType<Person>();
                foreach (Person person in allPerson)
                {
                    if (person)
                    {
                        if (person.GetComponent<Person>().isQuarantined == false)
                        {
                            timesQuarantineCentreUsed++;
                            StartCoroutine(LockDown());
                            IEnumerator LockDown()
                            {
                                person.GetComponent<Person>().isMovementAllowed = false;
                                person.GetComponent<Person>().anim.SetBool("Walking", false);
                                yield return new WaitForSeconds(15f);
                                person.GetComponent<Person>().isMovementAllowed = true;
                                person.GetComponent<Person>().anim.SetBool("Walking", true);
                            }
                        }
                    }
                }
                coinController.DecreaseCoins(lockdownPowerUpCost + timesQuarantineCentreUsed * 10);
                if (lockDownText)
                {
                    lockDownText.text = (lockdownPowerUpCost + timesQuarantineCentreUsed * 10).ToString();
                }

            }
            else
            {
                FindObjectOfType<CameraController>().isPanning = true;
            }

            FindObjectOfType<CameraController>().isPanning = true;
        }
     }

    public void SelfQuarantine()
    {
        if (!GetComponent<HelperCanvas>().isAnyCanvasOpened)
        {
            PlayButtonSound();
            FindObjectOfType<CameraController>().isPanning = false;
            if (coinController.totalCoins >= selfQuarantineCost)
            {
                Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var newPower = Instantiate(selfQuarantine, positionOfClick - new Vector3(0, 2, 0), Quaternion.identity);
                coinController.DecreaseCoins(selfQuarantineCost);
                selfQuarantineCost += 10;
                if (selfQuarantineText)
                {
                    selfQuarantineText.text = selfQuarantineCost.ToString();
                }
            }
            else
            {
                FindObjectOfType<CameraController>().isPanning = true;
            }
        }
    }

    public void SanatizerArea()
    {
        if (!GetComponent<HelperCanvas>().isAnyCanvasOpened)
        {
            PlayButtonSound();
            FindObjectOfType<CameraController>().isPanning = false;
            if (coinController.totalCoins >= sanatizeCost)
            {
                Vector3 positionOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var newPower = Instantiate(sanatizer, positionOfClick - new Vector3(0, 2, 0), Quaternion.identity);
                coinController.DecreaseCoins(sanatizeCost);
                sanatizeCost += 10;
                if (sanatizerText)
                {
                    sanatizerText.text = sanatizeCost.ToString();
                }
            }
            else
            {
                FindObjectOfType<CameraController>().isPanning = true;
            }
        }
    }

    public void PlayButtonSound()
    {
        source.clip = buttonSelect;
        source.Play();
    }
}
