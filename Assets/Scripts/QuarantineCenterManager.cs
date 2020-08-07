using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuarantineCenterManager : MonoBehaviour
{
    [SerializeField] GameObject statsCanvas;
    [SerializeField] GameObject gameManager;
    [SerializeField] TextMeshProUGUI upgradeCostText;
    [SerializeField] TextMeshProUGUI totalBedsText;
    [SerializeField] TextMeshProUGUI currentOccupiedBedsText;
    [SerializeField] TextMeshProUGUI treatmentSliderText;
    [SerializeField] GameObject treatmentSlider;
    [SerializeField] AudioClip upgradeSfx;

    private int totalBeds;
    private int currentOccupiedBeds;
    private int upgradeCost;
    private float timer;
    private float treatmentTime;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        statsCanvas.GetComponent<CanvasGroup>().alpha = 0;
        statsCanvas.GetComponent<CanvasGroup>().interactable = false;
        treatmentSlider.SetActive(false);
        totalBeds = 1;
        currentOccupiedBeds = 0;
        upgradeCost = 100;
        treatmentTime = 30f;
        timer = treatmentTime;
    }

    // Update is called once per frame
    void Update()
    {
        //updating text holders
        UpdateTextHolders();

        //call QuarantineCenter tap handler
        QuarantineCenterTapHandler();

        //call treatment handler
        TreatmentHandler();
    }

    private void QuarantineCenterTapHandler()
    {
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray touchRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if(Physics.Raycast(touchRay, out var hit))
            {
                if(hit.collider.CompareTag("QuarantineCenter"))
                {
                    statsCanvas.GetComponent<CanvasGroup>().alpha = 1;
                    statsCanvas.GetComponent<CanvasGroup>().interactable = true;
                }
            }
        }
    }

    //Add a person function
    public void QuarantinePerson()
    {
        if(currentOccupiedBeds < totalBeds)
        {
            currentOccupiedBeds++;
            gameManager.GetComponent<PopulationController>().SendPersonToQuarantine();
        }
    }

    //Upgrade method
    public void UpgradeQuarantineCenter()
    {
        if(gameManager.GetComponent<CoinController>().totalCoins > upgradeCost)
        {
            source.clip = upgradeSfx;
            source.Play();
            totalBeds++;
            gameManager.GetComponent<CoinController>().DecreaseCoins(upgradeCost);
            upgradeCost += upgradeCost;
        }
    }

    //Treatment Handler
    private void TreatmentHandler()
    { 
        if(currentOccupiedBeds > 0)
        {
            treatmentSlider.SetActive(true);
            treatmentSlider.GetComponent<Slider>().value = timer;
            if(timer < 0)
            {
                timer = treatmentTime;
                currentOccupiedBeds--;
                gameManager.GetComponent<PopulationController>().AddPerson();
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            treatmentSlider.SetActive(false);
        }
    }

    //Update text boxes method
    private void UpdateTextHolders()
    {
        upgradeCostText.text = upgradeCost.ToString();
        totalBedsText.text = totalBeds.ToString();
        currentOccupiedBedsText.text = currentOccupiedBeds.ToString();
        treatmentSliderText.text = timer.ToString() + " s Left";
    }

    //Exit button function
    public void ExitCanvas()
    {
        statsCanvas.GetComponent<CanvasGroup>().alpha = 0;
        statsCanvas.GetComponent<CanvasGroup>().interactable = false;
    }
}
