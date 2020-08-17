using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

public class PopulationController : MonoBehaviour
{
    [SerializeField] GameObject personPrefab;
    [SerializeField] TextMeshProUGUI currentPopulationText;
    public List<GameObject> people;
    private Vector3 spawnPoint;
    private int personCost = 50;
    
    [SerializeField] ParticleSystem personJumpVFX;
    [SerializeField] GameObject winCanvas;
    public int currentPopulation { get; set; }
    private PauseManagerScript pauseManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        currentPopulationText.text = currentPopulation.ToString();
        AddPerson();
        currentPopulation = 0;
        people = new List<GameObject>();
        pauseManagerScript = GetComponent<PauseManagerScript>();
    }
    private int tempPeople = 0;
    public void AddPerson()
    {
        if (FindObjectOfType<CoinController>().totalCoins > personCost)
        {
            GetComponent<PowerUpButton>().PlayButtonSound();
            spawnPoint = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
            ParticleSystem particleVFX = Instantiate(personJumpVFX, spawnPoint + new Vector3(0,1,0), Quaternion.identity);
            FindObjectOfType<PowerUpButton>().PlayButtonSound();
            Destroy(particleVFX, 1.5f);
            GameObject tempPerson = Instantiate(personPrefab, spawnPoint, Quaternion.identity);
            FindObjectOfType<CoinController>().DecreaseCoins(personCost);
            personCost += 5;
            GetComponent<PowerUpButton>().UpdatePersonCostText(personCost);
            tempPerson.GetComponent<Person>().isInfected = (Random.value > 0.5f);
            currentPopulation++;
            UpdatePeopleNumber();
            tempPeople++;// a variable to get the number of person added
            people.Add(tempPerson);
            // Code for finding the healthy people
        }

        CheckLevelUp();
        if (currentPopulation >= 0 && gameWon == false)
        {
            GameWin();
        }
    }

    public void UpdatePeopleNumber()
    {
        currentPopulationText.text = currentPopulation.ToString();
        return;
    }

    public void CheckLevelUp()
    {
        if (tempPeople % 5 == 0 && tempPeople > 4)
        {
            FindObjectOfType<LevelUpScript>().PlayerLevelUp();
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        winCanvas.SetActive(false);
    }
    public void AddHealthyPerson()
    {
        if (FindObjectOfType<CoinController>().totalCoins > personCost)
        {
            spawnPoint = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
            ParticleSystem particleVFX = Instantiate(personJumpVFX, spawnPoint + new Vector3(0, 1, 0), Quaternion.identity);
            FindObjectOfType<PowerUpButton>().PlayButtonSound();
            Destroy(particleVFX, 3f);
            GameObject tempPerson = Instantiate(personPrefab, spawnPoint, Quaternion.identity);
            tempPerson.GetComponent<Person>().isInfected = false;
            currentPopulation++;
            UpdatePeopleNumber();
            people.Add(tempPerson);
        }

    }

    private bool gameWon = false;
    private int count = 0;
    public void GameWin()
    {
        Person[] persons = FindObjectsOfType<Person>();
        foreach(Person person in persons)
        {
            
            if(person && person.isInfected== false && person.isSanatizerInUse== false && person.isQuarantined== false)
            {
                count++; // counting the players
            }         
            else
            {
                gameWon = false;
                break; 
            }
            pauseManagerScript.UpdateWinSlider(count);
        }
        if (count >= 100 && count <= currentPopulation)
        {
            gameWon = true;
            GetComponent<PauseManagerScript>().PlayGameWonSound();
            winCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            count = 0;
        }
    }
    
    public void SendPersonToQuarantine()
        {
        if (currentPopulation > 0)
            {
                float minImmunity = 20f;
                //GameObject tempPerson = people[0];
                GameObject tempPerson;
                //foreach (GameObject person in people)
            for (int i = 0; i < people.Count; i++)
            {
                GameObject person = people[i];
                if (person)
                {
                    if (person.GetComponent<Person>())
                    {
                        if (person.GetComponent<Person>().isInfected || person.GetComponent<Person>().GetImmunity() < minImmunity)
                        {
                            if (person.GetComponent<Person>().isQuarantined == false)
                            {
                                if (FindObjectOfType<QuarantineCenterManager>().currentOccupiedBeds < FindObjectOfType<QuarantineCenterManager>().totalBeds)
                                {
                                    FindObjectOfType<QuarantineCenterManager>().currentOccupiedBeds++;
                                    minImmunity = person.GetComponent<Person>().GetImmunity();
                                    tempPerson = person;
                                    people.Remove(tempPerson);
                                    currentPopulation--;
                                    Destroy(tempPerson);
                                    tempPerson.GetComponent<Person>().personIndex = currentPopulation;
                                    people.Add(tempPerson); // lets see what this affects now
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
