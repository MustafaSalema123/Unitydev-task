using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
 

    public Animator anim;
    public Animator holoAnim;

    [Header("Player ")]
    [SerializeField] float movementSpeed;
    public bool isPlayerControl = true;


    [Header("PLayer Surface and Gravity")]
    public LayerMask surfaceLayer;
    public bool onGround;
    protected Vector3 movedirWall;
    private Vector3 gravityDirection;
    






    [Header("Gaming Machanic")]
    public GameObject hologram;
    public Transform   hologramtarget;
    private bool isGravityManipulating;
    public Transform legsTransform;
    public LayerMask obstacleLayer;
  //  protected Vector3 rayDirection ;
   public Vector3 movementDirection = Vector3.zero;
 

 

    protected bool touchingObject ;
    protected bool isEnterPressed ;


   [Header("Jump ")]
    public float jumpForce = 5f;
    public float jumpDuration = 0.5f;
    private bool isJumping;

    public KeyCode keyPressed = KeyCode.None;

   
    public float rotationAmount = 90f;
    // Update is called once per frame

   
    
    protected void Jump()
    {
     
        isJumping = true;
        Vector3 jumpVector = Vector3.up * jumpForce;
        StartCoroutine(JumpCoroutine(jumpVector));
    }

    protected IEnumerator JumpCoroutine(Vector3 jumpVector)
    {
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            Vector3 offset = jumpVector * (1f - t);
            transform.Translate(offset, Space.Self);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isJumping = false;
    }

    protected bool CheckGrounded()
    {
        return onGround = Physics.CheckSphere(legsTransform.position, 0.1f, surfaceLayer);
    }
    protected bool CheckWallRun()
    {
      
        bool checkSphere = Physics.CheckSphere(legsTransform.position, 0.1f, obstacleLayer);
        if (checkSphere) { 
            //isPlayerControl = true;
            return true;

        }
 
      
        return false;
    }



    protected void RaycastAndMoveTowardWall()
    {

        if (isJumping || CheckWallRun()) return;


        Vector3 rotatedDirection = hologramtarget.rotation * movedirWall;
        if (Physics.Raycast(hologramtarget.position, rotatedDirection, out RaycastHit hit, 100))

        {

       
                Debug.DrawRay(hologramtarget.position, rotatedDirection * hit.distance, Color.red);
                movementDirection = hit.point;

            }
            else
            {

                Debug.DrawRay(hologramtarget.position, rotatedDirection * hit.distance, Color.green);
            }

            MoveTowards(hit.normal);


           
            if (Vector3.Distance(transform.position, hit.point) > 0.1f)
            {
               
                // isEnterPressed = false;
                touchingObject = true;

            }
            else
            {
                touchingObject = false;
            }

        
    }
     void MoveTowards( Vector3 hitNormal)
    {
       
        // Determine the movement direction based on the hit normal
        if (Mathf.Abs(hitNormal.x) > Mathf.Abs(hitNormal.z))
        {
            movementDirection = new Vector3(-hitNormal.x, -hitNormal.y, 0f).normalized;
        }
        else
        {
            movementDirection = new Vector3(0f, -hitNormal.y , -hitNormal.z).normalized;
        }
        if (touchingObject && isEnterPressed || !isJumping)
        {
            Vector3 movement = movementDirection * movementSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }
    }

    protected void HoloGramControl()
    {

        if(CheckWallRun() ||
            CheckGrounded()) { 
        // targetRotation = Quaternion.FromToRotation(Vector3.up, gravityDirection);
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            // if (keyPressed == KeyCode.None)
            //{
            // Store the last arrow key pressed
            //keyPressed = Event.current.keyCode;
            isPlayerControl = false;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    movedirWall = Vector3.forward;
                    gravityDirection = Vector3.back;
                    StartGravityManipulation();
                   

                   keyPressed = KeyCode.UpArrow;
                   
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    gravityDirection = Vector3.right;
                    movedirWall = Vector3.left;
                    StartGravityManipulation();
                    
                   
                    keyPressed = KeyCode.LeftArrow;
                
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    gravityDirection = Vector3.left;
                    movedirWall = Vector3.right;
                    StartGravityManipulation();
                  
                    keyPressed = KeyCode.RightArrow;
                
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    movedirWall = Vector3.back;
                   
                    gravityDirection = Vector3.forward;
                    StartGravityManipulation();
                 
                    keyPressed = KeyCode.DownArrow;
                    
                }
           }
        }

       
        Quaternion newTargetLocation = Quaternion.FromToRotation(-Vector3.up, gravityDirection);
       hologramtarget.localRotation = newTargetLocation;
        if (/*keyPressed == KeyCode.None */Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            isGravityManipulating = false;
            hologram.transform.localRotation = Quaternion.Euler(0, 0, 0);
            keyPressed = KeyCode.None;
            // hologram.transform.rotation = Quaternion.identity ;
            hologram.SetActive(false);
            isPlayerControl = true;
        }

       Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, gravityDirection);
        if (isGravityManipulating)
        {

           
            //targetRotation = Quaternion.FromToRotation(Vector3.up, gravityDirection);
             //   hologram.transform.localRotation = Quaternion.RotateTowards(hologram.transform.localRotation, targetRotation, 150f * Time.deltaTime);
            hologram.transform.localRotation = Quaternion.RotateTowards(hologram.transform.localRotation, targetRotation,200f * Time.deltaTime);


        }

       
    }

    protected void StartGravityManipulation()
    {
        isGravityManipulating = true;
        hologram.SetActive(true);
 
    }

  


}
