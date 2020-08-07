using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] float startWaitTime;
    [SerializeField] float health = 100;
    [SerializeField] float immunity = 100;
    [SerializeField] float healthDecreaseRate = 0.5f;
    [SerializeField] float immunityDecreaseRate = 0.5f;

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

    [SerializeField] GameObject infectionForeField;
    [SerializeField] GameObject powerUpUseForceField;

    public bool isMovementAllowed = true;

    public int personIndex { get; set; }

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
        MovementHandler();
        StatusBarsHandler();
        ImmunityHandler();
        HealthHandler();
    }

    private void ImmunityHandler()
    {
        if(isInfected && immunity > 0)
        {
            immunity -= immunityDecreaseRate*Time.deltaTime;
            immunityBar.GetComponent<Slider>().value = immunity;
        }
    }

    private void HealthHandler()
    {
        if(immunity <= 0 && isInfected)
        {
            immunityBar.SetActive(false);
            healthBar.SetActive(true);
            health -= healthDecreaseRate*Time.deltaTime;
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
                other.GetComponent<Person>().enableInfectionForceField();
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
        powerUpUseForceField.SetActive(true);
    }

    // Code below manages the maskEffect
    public void MaskEffect()
    {
        StartCoroutine(MaskEffectTime());
    }
    IEnumerator MaskEffectTime()
    {
        GetComponent<SphereCollider>().radius = 1.5f;
        if (GetComponentInChildren<PowerUpShield>())
            GetComponentInChildren<PowerUpShield>().DecreasePowerShieldSize();
        if (GetComponentInChildren<InfectionShield>())
            GetComponentInChildren<InfectionShield>().DecreaseInfectionShieldSize();
        yield return new WaitForSeconds(10f);
        GetComponent<SphereCollider>().radius = 2f;
        if (GetComponentInChildren<InfectionShield>())
            GetComponentInChildren<InfectionShield>().IncreaseInfectionShieldSize();
        if (GetComponentInChildren<PowerUpShield>())
            GetComponentInChildren<PowerUpShield>().IncreasePowerShieldSize();
    }

    // code below manages the Quarantine Effect of Player it disables the person Infection trigger Collider for 60 sec
    public void QuarantineEffect(float timeValue, Vector3 position, GameObject prefab)
    {
         float  quarantineTime = timeValue;
        StartCoroutine(Quarantine());
        IEnumerator Quarantine()
        {
            SetHealth(100f);
            SetImmunity(100f);
            isInfected = false;
            isMovementAllowed = false;
            anim.SetBool("Walking", false);
            disableInfectionForceField();
            var newPower = Instantiate(prefab, position, Quaternion.identity);
            Destroy(newPower, quarantineTime - 1f);
            GetComponent<SphereCollider>().enabled = false;
            yield return new WaitForSeconds(quarantineTime);
            GetComponent<SphereCollider>().enabled = true;
            anim.SetBool("Walking", true);
            isMovementAllowed = true;
        }
    }

    public void SanatizeThePerson(GameObject SanatizerPrefab,Vector3 position)
    {
        var newPower = Instantiate(SanatizerPrefab, position, Quaternion.identity);
        Destroy(newPower,1f);
    }

    public void SanatizingPerson(float timeToWaiting)
    {
        StartCoroutine(waitForSanatizer());
        IEnumerator waitForSanatizer()
        {
            disableInfectionForceField();
            yield return new WaitForSeconds(timeToWaiting);
            enableInfectionForceField();
        }
    }
    
}
