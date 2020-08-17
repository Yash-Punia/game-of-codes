using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] float health = 100;
    [SerializeField] float immunity = 100;
    [SerializeField] float healthDecreaseRate = 0.5f;
    [SerializeField] float immunityDecreaseRate = 0.5f;
    [SerializeField] AudioClip deathSfx;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject immunityBar;
    public bool isInfected;

    public Animator anim;
    private Vector3 moveSpot;
    private float waitTime;
    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;
    private bool rotationCalculated;
    private AudioSource source;
    private float startWaitTime;

    [SerializeField] GameObject infectionForeField;
    [SerializeField] GameObject powerUpUseForceField;

    public bool isMovementAllowed = true;
    public bool isQuarantined = false;

    public bool isSanatizerInUse = false;
    public int personIndex { get; set; }

    [SerializeField] GameObject gameManager;
    private PopulationController populationController;
    private PauseManagerScript pauseManagerScript;

    void Start()
    {
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        waitTime = startWaitTime;
        rotationCalculated = false;
        healthBar.GetComponent<Slider>().value = health;
        immunityBar.GetComponent<Slider>().value = immunity;
        immunityBar.SetActive(true);

        populationController = gameManager.GetComponent<PopulationController>();
        pauseManagerScript = FindObjectOfType<PauseManagerScript>();

        //setting up initial values
        minX = -10f;
        maxX = 10f;
        minZ = -10f;
        maxZ = 10f;
        startWaitTime = Random.Range(1, 5);
        moveSpot = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));

        if (isInfected)
        {
            enableInfectionForceField();
        }
        else
        {
            disableInfectionForceField();
        }
    }

    void Update()
    {
        if(gameObject)
        {
            MovementHandler();
            StatusBarsHandler();
            ImmunityHandler();
            HealthHandler();
        }
    }

    private void ImmunityHandler()
    {
        if(isInfected && immunity > 0)
        {
            immunity -= immunityDecreaseRate*Time.deltaTime;
            immunityBar.GetComponent<Slider>().value = immunity;
        }
    }

    private bool isPersonDead = false;
    private void HealthHandler()
    {
        if(immunity <= 0 && isInfected)
        {
            immunityBar.SetActive(false);
            healthBar.SetActive(true);
            health -= healthDecreaseRate*Time.deltaTime;
            healthBar.GetComponent<Slider>().value = health;
            if (health <= 0 && isPersonDead== false)
            {
                isPersonDead = true;
                source.clip = deathSfx;
                source.Play();
                FindObjectOfType<PopulationController>().currentPopulation -= 1;
                FindObjectOfType<PopulationController>().UpdatePeopleNumber();
                FindObjectOfType<PauseManagerScript>().peopleDied();
                Destroy(gameObject);
            }
        }
    }

    // below code triggers off and on of the health and immunity bar of perosn on zooming
    private void StatusBarsHandler()
    {
        if (Camera.main)
        {
            if (Camera.main.transform.position.y < 10f)
            {
                immunityBar.SetActive(true);
            }
            else
            {
                immunityBar.SetActive(false);
                healthBar.SetActive(false);
            }
        }
    }

    // below code manages the motion of person
    private void MovementHandler()
    {
        if (isMovementAllowed)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveSpot, moveSpeed * Time.deltaTime);
            if (moveSpot - transform.position != Vector3.zero && !rotationCalculated)
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

    // Infection Spread Script 
    private void OnTriggerEnter(Collider other)
    {
        if (isInfected)
        {
            if (other.GetComponent<Person>())
            {
                if (other.GetComponent<Person>().isQuarantined == false)
                {
                    other.GetComponent<Person>().enableInfectionForceField();
                }
            }
        }
    }

    public void SetHealth(float value)
    {
        health += value;
        if(health>100f)
        {
            health = 100f;
        }
    }
    public void SetImmunity(float value)
    {
        immunity += value;
        if (immunity > 100f)
        {
            immunity = 100f;
        }
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetImmunity()
    {
        return immunity;
    }

    public void disableInfectionForceField()
    {
        isInfected = false;
        infectionForeField.SetActive(false);
    }
    public void enableInfectionForceField()
    {
        isInfected = true;
        infectionForeField.SetActive(true);
    }

    public void disablePowerUpForceField()
    {
        powerUpUseForceField.SetActive(false);
    }
    public void enablePowerUpForceField()
    {
        StartCoroutine(disablingPowerUpShield());
        IEnumerator disablingPowerUpShield()
        {
            powerUpUseForceField.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            powerUpUseForceField.SetActive(false);
        }
    }

    // Code below manages the maskEffect

    public bool isMaskFullyUpgraded = false;
    private int timesMaskUsed = 0;
    
    
    public void MaskEffect()
    {
        timesMaskUsed++;
        GetComponent<SphereCollider>().radius = 1.5f;
        if (GetComponentInChildren<PowerUpShield>())
            GetComponentInChildren<PowerUpShield>().DecreasePowerShieldSize();
        if (GetComponentInChildren<InfectionShield>())
            GetComponentInChildren<InfectionShield>().DecreaseInfectionShieldSize();
        if(timesMaskUsed == 2)
        {
            isMaskFullyUpgraded = true;
        }
    }

    // code below manages the Quarantine Effect of Player it disables the person Infection trigger Collider for 45 sec

    public void QuarantineEffect(float timeValue, Vector3 position, GameObject prefab)
    {
        
        float  quarantineTime = timeValue;
        StartCoroutine(Quarantine());
        IEnumerator Quarantine()
        {
            GetComponent<Rigidbody>().mass = 10; // inreasing the mass of person so as to make him stay at his place
            isQuarantined = true; // adding a bool to fix the error caused by lockdown 
            SetHealth(90f);
            SetImmunity(90f);
            isMovementAllowed = false;
            anim.SetBool("Walking", false);
            disableInfectionForceField();
            var newPower = Instantiate(prefab, position, Quaternion.identity);
            Destroy(newPower, quarantineTime - 1f);
            GetComponent<SphereCollider>().enabled = false;
            yield return new WaitForSeconds(quarantineTime);
            GetComponent<Rigidbody>().mass = 1; // making the mass same back to again
            isQuarantined = false;  // adding a bool to fix the error caused by lockdown 
            GetComponent<SphereCollider>().enabled = true;
            anim.SetBool("Walking", true);
            isMovementAllowed = true;
        }
    }

    public void SanatizeThePerson(GameObject SanatizerPrefab,Vector3 position)
    {
        var newPower = Instantiate(SanatizerPrefab, position, Quaternion.identity);
        Destroy(newPower,0.1f);
    }

    public void SanatizingPerson(float timeToWaiting)
    {
            StartCoroutine(waitForSanatizer());
            IEnumerator waitForSanatizer()
            {
                isSanatizerInUse = true;
                disableInfectionForceField();
                yield return new WaitForSeconds(timeToWaiting);
                enableInfectionForceField();
                isSanatizerInUse = false;
            }
    }
    
}
