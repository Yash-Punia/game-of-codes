using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinController : MonoBehaviour
{
    //Setting up some variables through the Unity Editor
    [SerializeField] float houseProductionInterval;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI currentPopulationText;
    [SerializeField] GameObject office;
    [SerializeField] GameObject officeCanvas;
    [SerializeField] GameObject houseCanvas;
    [SerializeField] TextMeshProUGUI houseProductionText;
    [SerializeField] TextMeshProUGUI houseUpgradationText;
    [SerializeField] AudioClip upgradeSfx;
    [SerializeField] AudioClip coinSfx;

    //private variables
    private float timer;
    private int houseProductionAmount;
    private int houseProductionBoost;
    private int houseUpgradationCost = 10;
    private int officeUnlockCost = 500;
    private bool officeUnlocked = false;
    private AudioSource source;

    public int totalCoins { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        timer = houseProductionInterval;

        //Setting up initial values for private variables
        totalCoins = 300;
        houseProductionAmount = 0;
        houseProductionBoost = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Displyaing coinText
        coinText.text = totalCoins.ToString();
        currentPopulationText.text = FindObjectOfType<PopulationController>().currentPopulation.ToString();
        //House money production
        HouseProductionHandler();
        houseProductionText.text = (houseProductionBoost * GetComponent<PopulationController>().people.Count / houseProductionInterval).ToString() + " coins/sec";
        houseUpgradationText.text = houseUpgradationCost.ToString();

        //Office money production/collection
        OfficeProductionHandler();

        //House money collection
        HouseProductionCollectionHandler();
    }

    //Method to increase total coins
    public void IncreaseCoins(int amount)
    {
        source.clip = coinSfx;
        source.Play();
        totalCoins += amount;
    }

    //Method to decrease total coins 
    public void DecreaseCoins(int amount)
    {
        totalCoins -= amount;
    }

    //Regularly produces coins based on the number of people in playArea
    private void HouseProductionHandler()
    {
        int currentPopulation = GetComponent<PopulationController>().people.Count;
        if(currentPopulation > 0)
        {
            if(timer < 0)
            {
                houseProductionAmount += currentPopulation;
                timer = houseProductionInterval;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void HouseProductionCollectionHandler()
    {
        if(houseProductionAmount > 0)
        {
            coinPrefab.SetActive(true);
            if(Input.touchCount>0)
            {
                Touch touch = Input.GetTouch(0);
                Ray rayFromTouch = Camera.main.ScreenPointToRay(touch.position);
                if(Physics.Raycast(rayFromTouch, out var hit))
                {
                    if(hit.collider.CompareTag("HouseCoin"))
                    {
                        IncreaseCoins(houseProductionAmount * houseProductionBoost);
                        houseProductionAmount = 0;
                    }

                    if(hit.collider.CompareTag("House"))
                    {
                        Debug.Log("House Tapped");
                        houseCanvas.SetActive(true);
                    }
                }
            }
        }
        else
        {
            coinPrefab.SetActive(false);
        }
    }

    private void OfficeProductionHandler()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Ray rayFromTouch = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(rayFromTouch, out var hit))
            {
                if (hit.collider.CompareTag("Office"))
                {
                    if(officeUnlocked)
                    {
                        IncreaseCoins(GetComponent<PopulationController>().people.Count);
                        office.GetComponent<Animator>().SetTrigger("Tapped");
                    }
                    else
                    {
                        officeCanvas.SetActive(true);
                    }
                }
            }
        }
    }

    public void CloseOfficeCanvas()
    {
        officeCanvas.SetActive(false);
    }

    public void UnlockOffice()
    {
        if (totalCoins > officeUnlockCost)
        {
            source.clip = upgradeSfx;
            source.Play();
            DecreaseCoins(officeUnlockCost);
            officeCanvas.SetActive(false);
            officeUnlocked = true;
        }
    }

    public void CloseHouseCanvas()
    {
        houseCanvas.SetActive(false);
    }

    public void UpgradeHouseProduction()
    {
        if(totalCoins > houseUpgradationCost)
        {
            source.clip = upgradeSfx;
            source.Play();
            houseProductionBoost++;
            DecreaseCoins(houseUpgradationCost);
            houseUpgradationCost += houseUpgradationCost;
        }

    }
}
