using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float startWaitTime;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject immunityBar;
    [SerializeField] bool isInfected;

    private Animator anim;
    private Vector3 moveSpot;
    private float waitTime;
    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;
    private bool rotationCalculated;

    public float health { get; set; }
    public float immunity { get; set; }

    void Start()
    {
        moveSpot = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
        anim = GetComponent<Animator>();
        waitTime = startWaitTime;
        rotationCalculated = false;
        healthBar.GetComponent<Slider>().value = health;
        immunityBar.GetComponent<Slider>().value = immunity;
        immunityBar.SetActive(true);

        //setting up initial values
        minX = -10f;
        maxX = 10f;
        minZ = -10f;
        maxZ = 10f;
        health = 10f;
        immunity = 10f;
    }

    void Update()
    {
        MovementHandler();
        StatusBarsHandler();
        ImmunityHandler();
        HealthHandler();
    }

    private void ImmunityHandler()
    {
        if(isInfected && immunity > 0)
        {
            immunity -= 0.001f;
            immunityBar.GetComponent<Slider>().value = immunity;
        }
    }

    private void HealthHandler()
    {
        if(immunity <= 0)
        {
            immunityBar.SetActive(false);
            healthBar.SetActive(true);
            health -= 0.001f;
            healthBar.GetComponent<Slider>().value = health;
            if (health < 0)
                Destroy(gameObject);
        }
    }

    private void StatusBarsHandler()
    {
        if(Camera.main.transform.position.y < 10f)
        {
            immunityBar.SetActive(true);
        }
        else
        {
            immunityBar.SetActive(false);
            healthBar.SetActive(false);
        }
    }

    private void MovementHandler()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot, moveSpeed * Time.deltaTime);
        if(moveSpot - transform.position != Vector3.zero && !rotationCalculated)
        {
            rotationCalculated = true;
            transform.rotation = Quaternion.LookRotation(moveSpot - transform.position, Vector3.up);
        }
        if (Vector3.Distance(transform.position, moveSpot) < 0.2f)
        {
            if (waitTime <= 0)
            {
                rotationCalculated = false;
                moveSpot = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
                waitTime = startWaitTime;
                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
                waitTime -= Time.deltaTime;
            }
        }
    }
}
