using System;
using System.Collections;

using UnityEngine;



public class SpiderController : PlayerController
{
 

    public static Action OnTriggerCube;
 
    Vector3 moveDir;
    public float _speed = 3f;
   // public PlayerController playerController;
    public Vector3 moveDirection;
    [Header("Camera")]
    public SmoothCamera smoothCam;
    
   
    
    public bool stopLeft, stopRight, StopUp, StopBack;
    

    public float rotationAngle = 90f;
    private bool rotateClockwise = false;
    private bool rotateCounterClockwise = false;
    public Rigidbody rb;

    // Update is called once per frame
    private void Start()
    {
        isPlayerControl = true;
        //hologram = GameObject.Find("Capsule");//FindObjectOfType<GameObject>().;
        hologram.SetActive(false);
    
        touchingObject = true;
        //rayDirection = Vector3.down;
        movedirWall = Vector3.down;

        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
    }
    private void Update()
    {
        if (!GameManager.instance.isGameStart) return;

        PlayerInputControllerNew();

        RayCastRaydirection();
     
     
        HoloGramControl();
        RotateToWall();

        RaycastAndMoveTowardWall();
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {

            Jump();

        }
        anim.SetBool("InAir", !onGround);

        CheckGrounded();
       
       
    }

 

    //For rotation 
    IEnumerator RotateSmoothly(float targetAngle)
    {
        float currentAngle = 0f;
        float destinationAngle = targetAngle;

        while (Mathf.Abs(currentAngle) < Mathf.Abs(destinationAngle))
        {
            float step = Mathf.Sign(destinationAngle) * 450f * Time.deltaTime;
            transform.Rotate(Vector3.up, step, Space.Self);
            currentAngle += step;
            yield return null;
        }

        if (Mathf.Abs(currentAngle) >= Mathf.Abs(destinationAngle))
        {
            transform.Rotate(Vector3.up, destinationAngle - currentAngle, Space.Self);
        }

        if (targetAngle > 0f)
            rotateClockwise = false;
        else
            rotateCounterClockwise = false;
    }


    public void RayCastRaydirection()
    {

        StopUp = RayDirection(transform.position + transform.up, transform.forward , 0.5f);
        stopRight = RayDirection(transform.position + transform.up, transform.right, 0.5f);
        stopLeft = RayDirection(transform.position + transform.up, -transform.right, 0.5f);
        StopBack = RayDirection(transform.position + transform.up, -transform.forward, 0.5f);

    }
    protected void RotateToWall()
    {
       

        if (keyPressed != KeyCode.None)
        {
            anim.SetFloat("Movement", 0f, 0f, Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                touchingObject = true;
                isEnterPressed = true;

                if (keyPressed == KeyCode.UpArrow && !RayDirection(transform.position + transform.up, transform.forward, 1f))
                {

                    transform.Rotate(-rotationAmount, 0f, 0f, Space.Self);

                }
                else if (keyPressed == KeyCode.LeftArrow && !RayDirection(transform.position + transform.up, -transform.right, 1f))
                {
                    transform.Rotate(0f, 0f, -rotationAmount, Space.Self);

                }
                else if (keyPressed == KeyCode.RightArrow && !RayDirection(transform.position + transform.up, transform.right, 1f))
                {
                    transform.Rotate(0f, 0f, rotationAmount, Space.Self);

                }
                else if (keyPressed == KeyCode.DownArrow && !RayDirection(transform.position + transform.up, -transform.forward, 1f))
                {
                    transform.Rotate(rotationAmount, 0f, 0f, Space.Self);
                }
              
                keyPressed = KeyCode.None;
            }

        }
    }
    //player local Input
    public  void PlayerInputControllerNew()
    {
        if (!isPlayerControl ||
        !CheckWallRun())
            return;

        moveDir.y = 0f;

        if (Input.GetKey(KeyCode.W) && !StopUp )
        {
            moveDir = Vector3.forward;
            transform.Translate(moveDir * _speed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(KeyCode.S) && !StopBack )
        {
            moveDir = Vector3.back;
            transform.Translate(moveDir * _speed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(KeyCode.D) && !stopRight )
        {
            moveDir = Vector3.right;
            transform.Translate(moveDir * _speed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(KeyCode.A) && !stopLeft)
        {
            moveDir = Vector3.left;
            transform.Translate(moveDir * _speed * Time.deltaTime, Space.Self);
        }
        if (CheckGrounded() && keyPressed != KeyCode.None)
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
        if (Input.GetKeyDown(KeyCode.A) && !rotateCounterClockwise)
        {
            rotateCounterClockwise = true;
            StartCoroutine(RotateSmoothly(-rotationAngle));
        }
        else if (Input.GetKeyDown(KeyCode.D) && !rotateClockwise)
        {
            rotateClockwise = true;
            StartCoroutine(RotateSmoothly(rotationAngle));
        }

        
        float valueY = Input.GetAxis("Vertical");
        float valueX = Input.GetAxis("Horizontal");
        float movementAmount = Mathf.Clamp01(Mathf.Abs(valueX) + Mathf.Abs(valueY));

        anim.SetFloat("Movement", movementAmount, 0f, Time.deltaTime);
        // Move the player

    }

    bool RayDirection(Vector3 startDir , Vector3 enddir , float dist) 
    {
        return Physics.Raycast(startDir, enddir, dist);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CubeCollect")
        {

            //TODO increse point  
            GameManager.instance.score += 1;
            OnTriggerCube?.Invoke();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Lost")
        {


            GameManager.instance.ShowEndPanel(" Your Lost ");
        }
    }
}
