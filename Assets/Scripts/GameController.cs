using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject cube;

    public GameObject levels;
    public GameObject level0;
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject level4;

    //Objects to reset
    public GameObject switch4;

    public GameObject doors;
    public GameObject doorLeft;
    public GameObject doorRight;
    public GameObject doorTop;
    public GameObject doorBottom;

    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textTimer;
    public double timer;
    public bool timerOn;

    public TextMeshPro textOptionTimer;

    public Material doorMaterial;
    public Material playerMaterial;

    public bool gameActive;

    public Camera cam;
    public PlayerController player;
    public bool camFinishedMove;
    public int level = 0;

    public bool isRotating = false;
    public bool rotateQueued = false;
    public string rotateWay;
    public float rotateProgress;

    public bool isMoving = false;
    public bool moveQueued = false;
    public float moveProgress;

    public Vector3 playerPOS;

    public AudioSource music;
    public AudioSource rotateSound;
    public AudioSource switchSound;
    public AudioSource nextLevelSound;

    private readonly float rotateSpeed = 1.8f; // Has to be a divisible of 90 (1f ,0.9f etc)
    private readonly float moveSpeed = 0.5f; // Has to be a divisible of 50 (1f ,0.5f etc)

    private void Start()
    {
        music.time = 2;
        music.Play();
        SetCubeControl();
        gameActive = true;
        timerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!camFinishedMove)
        {
            if (cam.fieldOfView <= 70)
            {
                cam.fieldOfView += 0.5f;
            }
            else
            {
                camFinishedMove = true;
                player.canMove = true;
            }
        }
        if (level == 0) 
        {
            textLevel.text = "";
        }
        else
        {
            textLevel.text = "Level " + level.ToString();
            if(timerOn)
            {
                textTimer.text = "Timer: " + timer.ToString("N1");
            }
        }
    }

    private void FixedUpdate()
    {
        
        if (level != 0 && gameActive && timerOn)
        {

            timer += Time.deltaTime;
        }

        if (moveQueued)
        {
            isMoving = true;
            levels.transform.Translate(-moveSpeed, 0, 0);
            moveProgress += moveSpeed;

            if (moveProgress == 50)
            {
                moveQueued = false;
                moveProgress = 0;
                isMoving = false;
                player.transform.position = new Vector3(0, 5, 0);      //Sets new position to player
                player.gameObject.SetActive(true);          //Reinstates the player to view
            }
        }

        if (rotateQueued)
        {
            isRotating = true;

            if (rotateWay == "right")
            {
                cube.transform.RotateAround(cube.transform.position, Vector3.forward, rotateSpeed);
                player.transform.RotateAround(cube.transform.position, Vector3.forward, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;                         //Sets bool for rotation check
                    playerPOS.x = -5f;                       //Moves player to new position
                    player.transform.position = playerPOS;      //Sets new position to player
                    player.scaleRightRevertQueued = true;
                }
            }
            if (rotateWay == "left")
            {
                cube.transform.RotateAround(cube.transform.position, Vector3.back, rotateSpeed);
                player.transform.RotateAround(cube.transform.position, Vector3.back, rotateSpeed);
                rotateProgress = rotateProgress + rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;                         //Sets bool for rotation check
                    playerPOS.x = 5f;                       //Moves player to new position
                    player.transform.position = playerPOS;      //Sets new position to player
                    player.scaleLeftRevertQueued = true;
                }
            }
            if (rotateWay == "down")
            {
                cube.transform.RotateAround(cube.transform.position, Vector3.right, rotateSpeed);
                player.transform.RotateAround(cube.transform.position, Vector3.right, rotateSpeed);
                rotateProgress = rotateProgress + rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;                         //Sets bool for rotation check
                    playerPOS.z = 5f;                       //Moves player to new position
                    player.transform.position = playerPOS;      //Sets new position to player
                    player.scaleDownRevertQueued = true;
                }
            }
            if (rotateWay == "up")
            {
                cube.transform.RotateAround(cube.transform.position, Vector3.left, rotateSpeed);
                player.transform.RotateAround(cube.transform.position, Vector3.left, rotateSpeed);
                rotateProgress = rotateProgress + rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;                         //Sets bool for rotation check
                    playerPOS.z = -5f;                       //Moves player to new position
                    player.transform.position = playerPOS;      //Sets new position to player
                    player.scaleUpRevertQueued = true;
                }
            }
        }
    }
    public void Rotate(string way) //Right: Z-, Left, Z+
    {
        rotateSound.Play();
        rotateWay = way;                                        //Stores the rotation orientation
        rotateQueued = true;                                    //Tells the controller to start a rotation
        playerPOS = player.gameObject.transform.position;       //Stores the player position
    }

    public void NextLevel()
    {
        nextLevelSound.Play();
        moveQueued = true;
        level++;
        SetCubeControl();
        player.gameObject.SetActive(false);                     //Removes the player from view

    }

    public void SetCubeControl()
    {
        switch(level)
        {
            case 0:
                cube = level0;
                break;
            case 1:
                cube = level1;
                break;
            case 2:
                cube = level2;
                break;
            case 3:
                cube = level3;
                break;
        }
    }

    public void ResetLevel(int i)
    {
        if (i == 4)
        {

        }
    }

    public void OptionChange(string option)
    {
        if(option == "timer")
        {
            if(timerOn)
            {
                timerOn = false;
                textOptionTimer.text = "Timer: Off";
            }
            else
            {
                timerOn = true;
                textOptionTimer.text = "Timer: On";
            }
        }
    }
}
