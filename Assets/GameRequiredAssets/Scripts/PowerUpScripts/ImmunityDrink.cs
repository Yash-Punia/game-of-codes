﻿using System.Collections;
using UnityEngine;

public class ImmunityDrink : MonoBehaviour
{
    bool moveAllowed = false;
    public Camera cam;
    private float speedModifier = 0.05f;
    private LineRenderer laserLine;
    private bool isPowerUsed = false;

    //
    //Power up variables
    float immunityIncreaseValue = 10f;
    [SerializeField] float valueToBeUpdate = 5f;
    //

    private void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        FindObjectOfType<CameraController>().isPanning = false;
        laserLine.enabled = false;
    }
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 rayOrigin = transform.position;// the position of our game Object;
            laserLine.enabled = true;
            RaycastHit hit;

            laserLine.SetPosition(0, rayOrigin);

            if (Physics.Raycast(rayOrigin,cam.transform.forward,out hit))
            {
                laserLine.SetPosition(1, hit.point);    // if laser hits something
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + cam.transform.forward*50); // it laser doesnt hit anything
            }

            if (hit.collider.GetComponent<Person>())
            {
                if (hit.collider.GetComponent<Person>().isInfected && isPowerUsed == false)
                {
                    isPowerUsed = true;
                    GameObject infectedPerson = hit.collider.gameObject;
                    infectedPerson.GetComponent<Person>().disableInfectionForceField();
                    infectedPerson.GetComponent<Person>().enablePowerUpForceField();
                    StartCoroutine(UsePowerUp());

                    IEnumerator UsePowerUp()
                    {
                        yield return new WaitForSeconds(0.5f);
                        infectedPerson.GetComponent<Person>().disablePowerUpForceField();
                        infectedPerson.GetComponent<Person>().enableInfectionForceField();
                        //
                        // Write Code for the actual impact of power up
                        infectedPerson.GetComponent<Person>().SetImmunity(immunityIncreaseValue);
                        //
                        laserLine.enabled = false;
                        FindObjectOfType<CameraController>().isPanning = true;
                        Destroy(gameObject);
                    }
                }
                else
                {
                    StartCoroutine(usePower());
                    IEnumerator usePower()
                    {
                        //hit.collider.GetComponent<Person>().enablePowerUpForceField();
                        yield return new WaitForSeconds(5f);
                        hit.collider.GetComponent<Person>().disablePowerUpForceField();
                        //hit.collider.GetComponent<Person>().SetImmunity(immunityIncreaseValue);
                        laserLine.enabled = false;
                        FindObjectOfType<CameraController>().isPanning = true;
                        if(gameObject)
                        Destroy(gameObject);
                    }
                }

            }
            
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                {   
                        moveAllowed = true;
                        FindObjectOfType<CameraController>().isPanning = false;
                        if (moveAllowed)
                        {
                            transform.position = new Vector3((transform.position.x + touch.deltaPosition.x * speedModifier), transform.position.y,
                            (transform.position.z + touch.deltaPosition.y * speedModifier));
                        }      
                }
        }
    }

    public void UpdateImmunityDrink()
    {
        if(immunityIncreaseValue <= 95f)
            immunityIncreaseValue += valueToBeUpdate;
    }
}
   


   
