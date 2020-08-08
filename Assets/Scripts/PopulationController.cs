using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopulationController : MonoBehaviour
{
    [SerializeField] GameObject personPrefab;

    public List<GameObject> people;
    private Vector3 spawnPoint;
    private int personCost = 50;

    [SerializeField] ParticleSystem personJumpVFX;

    public int currentPopulation { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        AddPerson();
        currentPopulation = 0;
        people = new List<GameObject>();
    }

    public void AddPerson()
    {
        if (FindObjectOfType<CoinController>().totalCoins > personCost)
        {
            spawnPoint = new Vector3(Random.Range(-10f, 10f), 10f, Random.Range(-10f, 10f));
            ParticleSystem particleVFX = Instantiate(personJumpVFX, spawnPoint, Quaternion.identity);
            FindObjectOfType<PowerUpButton>().PlayButtonSound();
            Destroy(particleVFX, 3f);
            GameObject tempPerson = Instantiate(personPrefab, spawnPoint, Quaternion.identity);
            currentPopulation++;
            people.Add(tempPerson);
            FindObjectOfType<CoinController>().DecreaseCoins(personCost);
        }

    }
        public void SendPersonToQuarantine()
        {
            if (currentPopulation > 0)
            {
                float minImmunity = 20f;
                GameObject tempPerson = people[0];
                foreach (GameObject person in people)
                {
                    if (person.GetComponent<Person>().isInfected || person.GetComponent<Person>().GetImmunity() < minImmunity)
                    {
                        FindObjectOfType<QuarantineCenterManager>().currentOccupiedBeds++;
                        minImmunity = person.GetComponent<Person>().GetImmunity();
                        tempPerson = person;
                        tempPerson.GetComponent<Person>().SetHealth(90f);
                        tempPerson.GetComponent<Person>().SetImmunity(90f);
                        tempPerson.GetComponent<Person>().disableInfectionForceField();
                        people.Remove(tempPerson);
                        currentPopulation--;
                        Destroy(tempPerson);
                        tempPerson.GetComponent<Person>().personIndex = currentPopulation;
                        people.Add(tempPerson);
                    }
                }
            
            }

        }
}
