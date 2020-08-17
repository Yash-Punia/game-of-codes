using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering;
//using UnityEngine.UIElements;

public class CoinController : MonoBehaviour
{
    //Setting up some variables through the Unity Editor
    [SerializeField] float houseProductionInterval;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] TextMeshProUGUI coinText;
    //[SerializeField] TextMeshProUGUI currentPopulationText;
    [SerializeField] GameObject office;
    [SerializeField] GameObject officeCanvas;
    [SerializeField] GameObject houseCanvas;
    [SerializeField] GameObject shoppingCanvas;
    [SerializeField] TextMeshProUGUI houseProductionText;
    [SerializeField] TextMeshProUGUI houseUpgradationText;
    [SerializeField] AudioClip upgradeSfx;
    [SerializeField] AudioClip coinSfx;
    [SerializeField] GameObject[] houseModels;
    [SerializeField] TextMeshProUGUI houseModelUpgradationText;
    [SerializeField] TextMeshProUGUI houseUpgradeLevelText;
    [SerializeField] TextMeshProUGUI officeUpgradeLevelText;
    [SerializeField] TextMeshProUGUI officeUnlockText;
    [SerializeField] GameObject[] bussinessBuildingModels;
    [SerializeField] TextMeshProUGUI businessBuildingModelUpgradationText;
    [SerializeField] Button houseFullyUpgardeButton;
    [SerializeField] Button officeUnlockButton;
    [SerializeField] Button officeFullyUpgardeButton;
    [SerializeField] TextMeshProUGUI officeFullyText;
    [SerializeField] TextMeshProUGUI houseFullyText;


    //private variables
    private float timer;
    private int houseProductionAmount;
    private int houseProductionBoost;
    private int houseUpgradationCost = 10;
    private int houseModelUpgradationCost = 50000;
    private int businessBuidlingModelUpgradationCost = 50000;
    private int houseModelNumeber = 1;
    private int businessBuildingModelNumer = 1;
    private int officeUnlockCost = 500;
    private bool officeUnlocked = false;
    int numberOfTaps = 0;
    private bool isHouseFullyUpgraded = false;
    private bool isOfficeFullyUpgraded = false;
    private AudioSource source;
    [SerializeField] GameObject animalsGameobject;
    [SerializeField] int animalsCost = 10000;
    private bool animalsEnabled = false;
    public bool is3DViewerEnabled = false;
    [SerializeField] int ViewerCost = 50000;
    [SerializeField] AudioClip shoppingListClicked;
    [SerializeField] AudioClip cancelClicked;
    


    //public variables with their count to manage dynamic varaition of cost of item

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
        animalsGameobject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Displyaing coinText
        coinText.text = totalCoins.ToString();
        houseModelUpgradationText.text = houseModelUpgradationCost.ToString();
        //currentPopulationText.text = FindObjectOfType<PopulationController>().currentPopulation.ToString();
        //House money production
        HouseProductionHandler();
        houseProductionText.text = (houseProductionBoost * GetComponent<PopulationController>().people.Count / houseProductionInterval).ToString() + " coins/sec";
        houseUpgradationText.text = houseUpgradationCost.ToString();

        businessBuildingModelUpgradationText.text = businessBuidlingModelUpgradationCost.ToString();
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
                if (Camera.main)
                {
                    Ray rayFromTouch = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(rayFromTouch, out var hit))
                    {
                        if (hit.collider.CompareTag("HouseCoin"))
                        {
                            IncreaseCoins(houseProductionAmount * houseProductionBoost * houseModelNumeber);
                            houseProductionAmount = Convert.ToInt32(houseProductionAmount * Time.deltaTime);
                        }
                        /*
                        if(hit.collider.CompareTag("House"))
                        {
                            houseCanvas.SetActive(true);
                        }
                        */
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
            numberOfTaps++;
            Touch touch = Input.GetTouch(0);
            if (Camera.main)
            {
                Ray rayFromTouch = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(rayFromTouch, out var hit))
                {
                    if (hit.collider.CompareTag("Office"))
                    {
                        if (officeUnlocked)
                        {
                            IncreaseCoins(GetComponent<PopulationController>().people.Count * businessBuildingModelNumer);
                            office.GetComponent<Animator>().SetTrigger("Tapped");
                        }
                    }
                }
            }
        }
    }

    public void CloseOfficeCanvas()
    {
        source.clip = cancelClicked;
        source.Play();
        officeCanvas.SetActive(false);
        shoppingCanvas.SetActive(true);
    }
    public void OpenShoppingCanvas()
    {
        source.clip = shoppingListClicked;
        source.Play();
        FindObjectOfType<CameraController>().StopPanning();
        shoppingCanvas.SetActive(true);
        Time.timeScale = 0f; // pausing the game
    }
    public void OpenOfficeCanvas()
    {
        source.clip = shoppingListClicked;
        source.Play();
        officeUpgradeLevelText.text = "Beginner";
        officeCanvas.SetActive(true);
        shoppingCanvas.SetActive(false);
    }
    public void OpenHouseCanvas()
    {
        source.clip = shoppingListClicked;
        source.Play();
        houseUpgradeLevelText.text = "Beginner";
        houseCanvas.SetActive(true);
        shoppingCanvas.SetActive(false);
    }
   
    public void CloseShoppingCanvas()
    {
        source.clip = cancelClicked;
        source.Play();
        FindObjectOfType<CameraController>().AllowPanning();
        Time.timeScale = 1f; // making game active again
        shoppingCanvas.SetActive(false);
    }

    private bool isOfficeUnlocked = false;

    public void UnlockOffice()
    {

        if (totalCoins > officeUnlockCost && isOfficeUnlocked == false )
        {
            officeUnlockButton.GetComponent<Image>().color = new Color(0,0,0);
            FindObjectOfType<LevelUpScript>().PlayerLevelUp();
            source.clip = upgradeSfx;
            source.Play();
            DecreaseCoins(officeUnlockCost);
            //officeCanvas.SetActive(false);
            officeUnlocked = true;
            isOfficeUnlocked = true;
            officeUnlockText.text = "Office Unlocked";
        }
    }

    public void CloseHouseCanvas()
    {
        source.clip = cancelClicked;
        source.Play();
        houseCanvas.SetActive(false);
        shoppingCanvas.SetActive(true);
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

    public void UpgradeHouseModel()
    {
        
        if (totalCoins > houseModelUpgradationCost && isHouseFullyUpgraded==false)
        {
            FindObjectOfType<LevelUpScript>().PlayerLevelUp();
            source.clip = upgradeSfx;
            source.Play();
            houseModels[houseModelNumeber-1].SetActive(false);
            houseModelNumeber++;
            houseModels[houseModelNumeber-1].SetActive(true);
            DecreaseCoins(houseModelUpgradationCost);
            houseModelUpgradationCost += houseModelUpgradationCost;
        }
        if(houseModelNumeber ==2)
        {
            houseUpgradeLevelText.text = "Intermediate";
        }
        if (houseModelNumeber >= 3)
        {
            houseUpgradeLevelText.text = "Professional";
            houseFullyText.text = "Fully Upgraded";
            isHouseFullyUpgraded = true;
            houseFullyUpgardeButton.GetComponent<Image>().color = new Color(0, 0, 0);

        }
    }

    public void UpgradeBuildingModel()
    {
        
            if (totalCoins > businessBuidlingModelUpgradationCost && isOfficeFullyUpgraded==false)
            {
                FindObjectOfType<LevelUpScript>().PlayerLevelUp();
                source.clip = upgradeSfx;
                source.Play();
                bussinessBuildingModels[businessBuildingModelNumer - 1].SetActive(false);
                businessBuildingModelNumer++;
                bussinessBuildingModels[businessBuildingModelNumer - 1].SetActive(true);
                DecreaseCoins(businessBuidlingModelUpgradationCost);
                businessBuidlingModelUpgradationCost += businessBuidlingModelUpgradationCost;
            }
        if(businessBuildingModelNumer ==2)
        {
            officeUpgradeLevelText.text = "Intermediate";
        }
        if (businessBuildingModelNumer >= 3)
        {
            officeFullyUpgardeButton.GetComponent<Image>().color = new Color(0, 0, 0);
            isOfficeFullyUpgraded = true;
            officeUpgradeLevelText.text = "Professional";
            officeFullyText.text = "Fully Upgraded";

        }
    }

    [SerializeField] GameObject ViewerColorChange;
    [SerializeField] GameObject AnimalColorChange;

    public void enableAnimals()
    {
        if (animalsEnabled == false)
        {
            if (totalCoins > animalsCost)
            {
                FindObjectOfType<LevelUpScript>().PlayerLevelUp();
                FindObjectOfType<LevelUpScript>().PlayerLevelUp();
                source.clip = shoppingListClicked;
                source.Play();
                DecreaseCoins(animalsCost);
                animalsGameobject.SetActive(true);
                animalsEnabled = true;
                AnimalColorChange.GetComponent<Image>().color = new Color(0f,200f,50f);
            }
        }
    }
    public void enable3dViewer()
    {
        if (is3DViewerEnabled == false)
        {
            if (totalCoins > ViewerCost)
            {
                FindObjectOfType<LevelUpScript>().PlayerLevelUp();
                FindObjectOfType<LevelUpScript>().PlayerLevelUp();
                FindObjectOfType<LevelUpScript>().PlayerLevelUp();
                source.clip = shoppingListClicked;
                source.Play();
                DecreaseCoins(ViewerCost);
                is3DViewerEnabled = true;
                ViewerColorChange.GetComponent<Image>().color = new Color(0f, 200f, 50f);
            }
        }
    }

    public void CancelButtonClicked()
    {
        source.clip = cancelClicked;
        source.Play();
    }
}
