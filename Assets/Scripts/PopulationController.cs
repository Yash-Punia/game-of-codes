using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class PopulationController : MonoBehaviour
{
    [SerializeField] GameObject personPrefab;
    [SerializeField] TextMeshProUGUI populationText;

    public List<GameObject> people { get; set; }
    private Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        people = new List<GameObject>();
    }

    private void Update()
    {
        populationText.text = people.Count.ToString();
    }

    public void AddPerson()
    {
        spawnPoint = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        bool coronaPositive = (Random.value > 0.5f);
        GameObject tempPerson = Instantiate(personPrefab, spawnPoint, Quaternion.identity);
        tempPerson.GetComponent<Person>().isInfected = coronaPositive;
        people.Add(tempPerson);
    }

    public void SendPersonToQuarantine()
    {
        if(people.Count > 0)
        {
            float minImmunity = 10f;
            GameObject tempPerson = people[0];
            foreach(GameObject person in people)
            {
                if(person.GetComponent<Person>().GetImmunity() < minImmunity)
                {
                    minImmunity = person.GetComponent<Person>().GetImmunity();
                    tempPerson = person;
                }
            }
            people.Remove(tempPerson);
            Destroy(tempPerson);
        }
    }

    public void LockDownEffect()
    {
        foreach(GameObject person in people)
        {
            StartCoroutine(LockDown());
            IEnumerator LockDown()
            {
                person.GetComponent<Person>().isMovementAllowed = false;
                yield return new WaitForSeconds(10f);
                person.GetComponent<Person>().isMovementAllowed = true;
            }
        }
    }
}
