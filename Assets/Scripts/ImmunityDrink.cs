using System.Collections;
using UnityEngine;

public class ImmunityDrink : MonoBehaviour
{
    bool moveAllowed = false;
    public Camera cam;
    private float speedModifier = 0.05f;
    private LineRenderer laserLine;

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
                GameObject infectedPerson = hit.collider.gameObject;
                infectedPerson.GetComponent<Person>().disableInfectionForceField();
                infectedPerson.GetComponent<Person>().enablePowerUpForceField();
                StartCoroutine(UsePowerUp());

                IEnumerator UsePowerUp()
                {
                    yield return new WaitForSeconds(0.5f);
                    infectedPerson.GetComponent<Person>().disablePowerUpForceField();
                    //
                    // Write Code for the actual impact of power up
                    infectedPerson.GetComponent<Person>().SetImmunity(immunityIncreaseValue);
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

    public void UpdateImmunityDrink()
    {
        if(immunityIncreaseValue <= 95f)
            immunityIncreaseValue += valueToBeUpdate;
    }
}
   


   
