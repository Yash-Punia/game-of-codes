using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class RabbitMovement : MonoBehaviour
{
    public float moveSpeed;
    

    public Animator rabbitAnim;
    private Vector3 moveSpot;
    private float waitTime;
    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;
    private bool rotationCalculated;
    private float startWaitTime;
    private Transform playerTransform;
    public int rabbitIndex { get; set; }

    private string[] m_buttonNames = new string[] { "Idle", "Run" };

    void Start()
    {
        rabbitAnim = GetComponent<Animator>();
        //setting up initial values
        minX = -5f + transform.position.x;
        maxX = 5f + transform.position.x;
        minZ = -5f + transform.position.z;
        maxZ = 5f + transform.position.z;
        startWaitTime = Random.Range(1, 5);
        moveSpot = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
    }

    // Update is called once per frame
    void Update()
    {
        MovementHandler();
    }
    private void MovementHandler()
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
                    rabbitAnim.SetTrigger("Next");// initiate running animation
            }
                else
                {
                waitTime -= Time.deltaTime;
                do
                {
                    rabbitAnim.SetTrigger("Next");
                }  while (waitTime<-1);
                     // initiate idle animation
                    
                }
            }
        
    }
}
