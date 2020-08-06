using System.Collections;
using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    /*
        public string draggingTag;
        public Camera cam;

        private Vector3 dis;
        private float posX;
        private float posY;

        private bool touched = false;
        private bool dragging = false;

        private Transform toDrag;
        private Rigidbody toDragRigidbody;
        private Vector3 previousPosition;

        void FixedUpdate()
        {

            if (Input.touchCount != 1)
            {
                dragging = false;
                touched = false;
                if (toDragRigidbody)
                {
                    SetDraggingProperties(toDragRigidbody);
                }
                return;
            }

            Touch touch = Input.touches[0];
            Vector3 pos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(pos);

                if (Physics.Raycast(ray, out hit) && hit.collider.tag == draggingTag)
                {
                    toDrag = hit.transform;
                    previousPosition = toDrag.position;
                    toDragRigidbody = toDrag.GetComponent<Rigidbody>();

                    dis = cam.WorldToScreenPoint(previousPosition);
                    posX = Input.GetTouch(0).position.x - dis.x;
                    posY = Input.GetTouch(0).position.y - dis.y;

                    SetDraggingProperties(toDragRigidbody);

                    touched = true;
                }
            }

            if (touched && touch.phase == TouchPhase.Moved)
            {
                dragging = true;

                float posXNow = Input.GetTouch(0).position.x - posX;
                float posYNow = Input.GetTouch(0).position.y - posY;
                Vector3 curPos = new Vector3(posXNow, posYNow, dis.z);

                Vector3 worldPos = cam.ScreenToWorldPoint(curPos) - previousPosition;
                worldPos = new Vector3(worldPos.x, worldPos.y, 0.0f);

                toDragRigidbody.velocity = worldPos / (Time.deltaTime * 10);

                previousPosition = toDrag.position;
            }

            if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                dragging = false;
                touched = false;
                previousPosition = new Vector3(0.0f, 0.0f, 0.0f);
                SetDraggingProperties(toDragRigidbody);
            }

        }

        private void SetDraggingProperties(Rigidbody rb)
        {
            rb.useGravity = false;
            rb.drag = 20;
        }*/

    float deltaX, deltaY;
    Rigidbody rb;
    bool moveAllowed = false;
    public string draggingTag;
    public Camera cam;
    private float speedModifier = 0.05f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(Input.touchCount ==1)
        {
            Touch touch = Input.GetTouch(0);
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out hit) && hit.collider.tag == draggingTag)
            {
                FindObjectOfType<CameraController>().isPanning = false;
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        {
                            moveAllowed = true;
                            rb.freezeRotation = true;
                            break;
                        }
                    case TouchPhase.Moved:
                        {
                            if (moveAllowed)
                            {
                                transform.position = new Vector3((transform.position.x + touch.deltaPosition.x*speedModifier),transform.position.y,
                                    (transform.position.z + touch.deltaPosition.y*speedModifier));
                            }
                            break;
                        }
                    case TouchPhase.Ended:
                        {
                            Debug.Log("Touch phase has ended");
                            FindObjectOfType<CameraController>().isPanning = true;
                            moveAllowed = false;

                            break;
                        }
                }
                
            }
            
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Person>())
        {
            FindObjectOfType<CameraController>().isPanning = true;
            Destroy(gameObject);
        }
    }
}