using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] float houseProductionInterval;
    [SerializeField] GameObject coinPrefab;

    private float timer;
    private int houseProductionAmount;

    public int totalCoins { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        timer = houseProductionInterval;
        totalCoins = 300;
        houseProductionAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Displyaing coinText UI
        Debug.Log(totalCoins);

        //House money production
        HouseProductionHandler();

        //Office money production/collection
        OfficeProductionHandler();

        //House money collection
        HouseProductionCollectionHandler();
    }

    public void IncreaseCoins(int amount)
    {
        totalCoins += amount;
    }

    public void DecreaseCoins(int amount)
    {
        totalCoins -= amount;
    }

    private void HouseProductionHandler()
    {
        int currentPopulation = GetComponent<PopulationController>().currentPopulation;
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
                    if(hit.collider.CompareTag("House"))
                    {
                        IncreaseCoins(houseProductionAmount);
                        houseProductionAmount = 0;
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
                    IncreaseCoins(GetComponent<PopulationController>().currentPopulation);
                }
            }
        }
    }
}
