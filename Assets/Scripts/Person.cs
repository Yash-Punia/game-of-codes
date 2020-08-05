using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;
    [SerializeField] float moveSpeed;
    [SerializeField] float startWaitTime;

    private Animator anim;
    private Vector3 moveSpot;
    private float waitTime;

    void Start()
    {
        moveSpot = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
        anim = GetComponent<Animator>();
        waitTime = startWaitTime;
    }

    void Update()
    {
        MovementHandler();
    }

    private void MovementHandler()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot, moveSpeed * Time.deltaTime);
        if(moveSpot - transform.position != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveSpot - transform.position);
        }
        if (Vector3.Distance(transform.position, moveSpot) < 0.2f)
        {
            if (waitTime <= 0)
            {
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
