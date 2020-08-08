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

    public int currentPopulation { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        currentPopulation = 0;
        people = new List<GameObject>();
    }



    public void AddPerson()
    {
        spawnPoint = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        GameObject tempPerson = Instantiate(personPrefab, spawnPoint, Quaternion.identity);
        currentPopulation++;
        people.Add(tempPerson);
        FindObjectOfType<CoinController>().DecreaseCoins(personCost);

    }
        public void SendPersonToQuarantine()
        {
            if (currentPopulation > 0)
            {
                float minImmunity = 10f;
                GameObject tempPerson = people[0];
                foreach (GameObject person in people)
                {
                    if (person.GetComponent<Person>().isInfected)
                    {
                        minImmunity = person.GetComponent<Person>().GetImmunity();
                        tempPerson = person;
                        tempPerson.GetComponent<Person>().SetHealth(90f);
                        tempPerson.GetComponent<Person>().SetImmunity(90f);
                    }
                }
                people.Remove(tempPerson);
                Destroy(tempPerson);
                tempPerson.GetComponent<Person>().personIndex = currentPopulation;
                people.Add(tempPerson);
            }

        }
}
