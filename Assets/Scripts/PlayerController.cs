using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody body;
    public bool canMove;

    public float axisX;
    public float axisZ;

    public bool godMode = false;

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
    private readonly float scaleSpeed = 0.025f;

    public List<string> achievements;

    public bool isDying = false;
    public bool isReviving = false;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!gameController.mainMenu.activeSelf && !gameController.pauseMenu.activeSelf)
        {
            axisX = Input.GetAxisRaw("Horizontal");
            axisZ = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.L))
            {
                gameController.ToggleLanguage();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                gameController.NextLevel();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                godMode = !godMode;
            }
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause"))
            {
                gameController.PauseMenu();
            }
        }
        
    }

    

    void FixedUpdate()
    {
        if (!isScaling && !gameController.rotationController.isRotating && !gameController.rotationController.isMoving && !isDying)
        {
            body.velocity = new Vector3(axisX, 0f, axisZ) * moveSpeed;
        }
        else
        {
            body.velocity = Vector3.zero;
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
                gameController.Rotate(RotationController.Rotations.Up);
            }
        }
        else if (scaleUpRevertQueued)
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
                transform.Translate(new Vector3(0f, 0f, 0.005f));
                canMove = true;
            }
        }
        else if (scaleDownQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x + scaleSpeed, transform.localScale.y, transform.localScale.z); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleDownQueued = false;
                scaleProgress = 0;
                isScaling = false;
                gameController.Rotate(RotationController.Rotations.Down);
            }
        }
        else if (scaleDownRevertQueued)
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
                transform.Translate(new Vector3(0f, 0f, -0.005f));
                canMove = true;
            }
        }
        else if (scaleLeftQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + scaleSpeed); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleLeftQueued = false;
                scaleProgress = 0;
                isScaling = false;
                gameController.Rotate(RotationController.Rotations.Left);
            }
        }
        else if (scaleLeftRevertQueued)
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
                transform.Translate(new Vector3(-0.005f, 0f, 0f));
                canMove = true;
            }
        }
        else if (scaleRightQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + scaleSpeed); ;
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleRightQueued = false;
                scaleProgress = 0;
                isScaling = false;
                gameController.Rotate(RotationController.Rotations.Right);
            }
        }
        else if (scaleRightRevertQueued)
        {
            isScaling = true;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - scaleSpeed);
            scaleProgress += scaleSpeed;

            if (scaleProgress >= 1)
            {
                scaleRightRevertQueued = false;
                scaleProgress = 0;
                isScaling = false;
                transform.rotation = new Quaternion();
                transform.Translate(new Vector3(0.005f, 0f, 0f));
                canMove = true;
            }
        }
        if (isDying)
        {
            canMove = false;
            transform.localScale = new Vector3(transform.localScale.x - (scaleSpeed / 4), transform.localScale.y - (scaleSpeed / 4), transform.localScale.z - (scaleSpeed / 4));
            scaleProgress += scaleSpeed;
            if (scaleProgress >= 2)
            {
                if (gameController.hardcoreOn)
                {
                    gameController.ResetGame();
                }
                else
                {
                    gameController.ResetLevel();
                }
                this.transform.position = new Vector3(0, 5, 0);
                isReviving = true;
                isDying = false;
                scaleProgress = 0;
            }
        }
        if (isReviving)
        {
            transform.localScale = new Vector3(transform.localScale.x + (scaleSpeed / 4), transform.localScale.y + (scaleSpeed / 4), transform.localScale.z + (scaleSpeed / 4));
            scaleProgress += scaleSpeed;
            if (scaleProgress >= 2)
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                canMove = true;
                isReviving = false;
                scaleProgress = 0;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("TeleporterLocal"))
        {
            collision.gameObject.GetComponent<TeleporterController>().enabled = true;               // Resets teleporter upon leaving to prevent being stuck in a loop
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(canMove)
        {
            if (collision.gameObject.CompareTag("TeleporterLocal"))
            {
                if(collision.gameObject.GetComponent<TeleporterController>().enabled)
                {
                    Vector3 xyz = collision.gameObject.GetComponent<TeleporterController>().partner.transform.position;
                    collision.gameObject.GetComponent<TeleporterController>().partner.GetComponent<TeleporterController>().enabled = false;

                    transform.position = new Vector3(xyz.x, transform.position.y, xyz.z);
                }
            }
            if (collision.gameObject.CompareTag("Teleporter"))
            {
                gameController.NextLevel();
            }
            if (collision.gameObject.CompareTag("DoorTop"))
            {
                canMove = false;
                scaleUpQueued = true;
            }
            if (collision.gameObject.CompareTag("DoorBottom"))
            {
                canMove = false;
                scaleDownQueued = true;
            }
            if (collision.gameObject.CompareTag("DoorLeft"))
            {
                canMove = false;
                scaleLeftQueued = true;
            }
            if (collision.gameObject.CompareTag("DoorRight"))
            {
                canMove = false;
                scaleRightQueued = true;
            }
            if (collision.gameObject.CompareTag("Trigger"))
            {
                collision.gameObject.GetComponent<TriggerController>().Trigger();
            }

            if (collision.gameObject.CompareTag("Enemy") )
            {
                if(!godMode)
                {
                    isDying = true;
                }
            }
        }   
    }
}
