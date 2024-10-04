using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody body;
    public bool canMove;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    public GameController gameController;
    public Collider col;

    public bool isScaling = false;
    public bool scaleUpQueued = false;
    public bool scaleDownQueued = false;
    public bool scaleLeftQueued = false;
    public bool scaleRightQueued = false;

    public bool scaleUpRevertQueued = false;
    public bool scaleDownRevertQueued = false;
    public bool scaleLeftRevertQueued = false;
    public bool scaleRightRevertQueued = false;

    public float scaleProgress;
    private readonly float scaleSpeed = 0.05f;

    public List<string> achievements;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            moveVelocity = moveInput * moveSpeed;
            
        }
        else
        {
            moveVelocity = Vector3.zero;
        }
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            body.velocity = moveVelocity*1.5f;
        }
        else
        {
            body.velocity = moveVelocity;
        }

        if (scaleUpQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x + scaleSpeed, transform.localScale.y, transform.localScale.z); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleUpQueued = false;
                scaleProgress = 0;
                isScaling = false;
                gameController.Rotate("up");
            }
        }
        if (scaleUpRevertQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x - scaleSpeed, transform.localScale.y, transform.localScale.z); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleUpRevertQueued = false;
                scaleProgress = 0;
                isScaling = false;
                transform.rotation = new Quaternion();
                canMove = true;
            }
        }
        if (scaleDownQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x + scaleSpeed, transform.localScale.y, transform.localScale.z); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleDownQueued = false;
                scaleProgress = 0;
                isScaling = false;
                gameController.Rotate("down");
            }
        }
        if (scaleDownRevertQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x - scaleSpeed, transform.localScale.y, transform.localScale.z); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleDownRevertQueued = false;
                scaleProgress = 0;
                isScaling = false;
                transform.rotation = new Quaternion();
                canMove = true;
            }
        }
        if (scaleLeftQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + scaleSpeed); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleLeftQueued = false;
                scaleProgress = 0;
                isScaling = false;
                gameController.Rotate("left");
            }
        }
        if (scaleLeftRevertQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - scaleSpeed); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleLeftRevertQueued = false;
                scaleProgress = 0;
                isScaling = false;
                transform.rotation = new Quaternion();
                canMove = true;
            }
        }
        if (scaleRightQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + scaleSpeed); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleRightQueued = false;
                scaleProgress = 0;
                isScaling = false;
                gameController.Rotate("right");
            }
        }
        if (scaleRightRevertQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - scaleSpeed); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleRightRevertQueued = false;
                scaleProgress = 0;
                isScaling = false;
                transform.rotation = new Quaternion();
                canMove = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.CompareTag("Teleporter"))
        {
            gameController.NextLevel();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("DoorTop") && canMove)
        {
            canMove = false;
            scaleUpQueued = true;
        }
        if (collision.gameObject.CompareTag("DoorBottom") && canMove)
        {
            canMove = false;
            scaleDownQueued = true;
        }
        if (collision.gameObject.CompareTag("DoorLeft") && canMove)
        {
            canMove = false;
            scaleLeftQueued = true;
        }
        if (collision.gameObject.CompareTag("DoorRight") && canMove)
        {
            canMove = false;
            scaleRightQueued = true;
        }
        if(collision.gameObject.CompareTag("Trigger") && canMove)
        {
            collision.gameObject.GetComponent<TriggerController>().Trigger();
        }
    }



}
