﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    [SerializeField] GameObject personPrefab;

    private List<GameObject> people;
    private Vector3 spawnPoint;

    public int currentPopulation { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        currentPopulation = 0;
        people = new List<GameObject>();
        AddPerson();
        AddPerson();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPerson()
    {
        spawnPoint = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        GameObject tempPerson = Instantiate(personPrefab, spawnPoint, Quaternion.identity);
        tempPerson.GetComponent<Person>().personIndex = currentPopulation;
        currentPopulation++;
        people.Add(tempPerson);
    }
}
