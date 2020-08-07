using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SelfQuarantine : MonoBehaviour
{
    bool moveAllowed = false;
    public Camera cam;
    private float speedModifier = 0.05f;
    private LineRenderer laserLine;
    [SerializeField] GameObject selfQuarantinePowerUp;

    //
    //Power up variables
    [SerializeField] float valueToBeUpdate = 5f;
    [SerializeField] float maxTime = 10f;

    [SerializeField] float quarantineTime = 60f;

    private void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        FindObjectOfType<CameraController>().isPanning = false;
        laserLine.enabled = false;
        StartCoroutine(PowerUpTimeHandler());
    }

    IEnumerator PowerUpTimeHandler()
    {
        yield return new WaitForSeconds(maxTime);
        FindObjectOfType<CameraController>().isPanning = true;
        Destroy(gameObject);
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
                    infectedPerson.GetComponent<Person>().QuarantineEffect(quarantineTime, infectedPerson.GetComponent<Transform>().position, selfQuarantinePowerUp);
  
                    //
                    laserLine.enabled = false;
                    FindObjectOfType<CameraController>().isPanning = true;
                    Destroy(gameObject);
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
}
   


   
