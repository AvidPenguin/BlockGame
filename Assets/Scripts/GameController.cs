using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Core Variables
    private GameObject cube;                    // The cube the player is currently on
    public GameObject allLevels;                // The parent empty GameObject containing all the levels
    public List<GameObject> levels;             // Ordered list of levels
    private int level;                          // Current level
    public TextController textController;       // Manages all text and translations

    // Reset Variables
    public List<GameObject> resetDisables;      // List of all objects to disable on a reset
    public List<GameObject> resetEnables;       // List of all objects to enable on a reset
    public List<GameObject> resetRotates;       // List of all objects to rotate on a reset

    // UI Objects
    public TextMeshPro textLevel;           // UI Level Text Object
    public TextMeshPro textTimer;           // UI Timer Text Object

    // Option objects and variables
    public double timer;
    public bool timerOn;
    public bool isHardcore;
    public TextMeshPro textOptionTimer;
    public TextMeshPro textOptionMusic;
    public TextMeshPro textOptionSounds;
    public TextMeshPro textOptionHardcore;

    //Camera variables
    public Camera cam;
    public PlayerController player;
    public bool camFinishedMove;                // Used for FOV Sliding effect on launch

    // Rotation variables
    public bool isRotating = false;
    public bool rotateQueued = false;
    public string rotateWay;
    public float rotateProgress;

    // Rotation triggering objects
    public GameObject doors;                    // The parent empty GameObject containing all the doors
    public GameObject doorLeft;
    public GameObject doorRight;
    public GameObject doorTop;
    public GameObject doorBottom;

    // Level change variables
    public bool isMoving = false;
    public bool moveQueued = false;
    public float moveProgress;

    // Audio objects
    public bool musicOn;
    public bool soundOn;
    public AudioSource music;
    public AudioSource rotateSound;
    public AudioSource switchSound;
    public AudioSource nextLevelSound;

    // Internal Variables
    private Vector3 playerPOS;                  // Used for storing data during rotations
    private readonly float rotateSpeed = 1.8f;  // Has to be a divisible of 90 (1f ,0.9f etc)
    private readonly float moveSpeed = 0.5f;    // Has to be a divisible of 50 (1f ,0.5f etc)

    private void Start()
    {
        level = 0;
        music.time = 2;
        timerOn = false;
        musicOn = true;
        soundOn = true;
        isHardcore = false;

        music.Play();
        textController.Init();
        SetCubeControl();
    }

    void Update()
    {
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
        if (!camFinishedMove)
        {
            if (cam.fieldOfView <= 70)
            {
                cam.fieldOfView += 0.1f;
            }
            else
            {
                camFinishedMove = true;
                player.canMove = true;
            }
        }
        if (level != 0 && timerOn)
        {

            timer += Time.deltaTime;
        }

        if (moveQueued)
        {
            isMoving = true;
            allLevels.transform.Translate(-moveSpeed, 0, 0);
            moveProgress += moveSpeed;

            if (moveProgress == 50)
            {
                moveQueued = false;
                moveProgress = 0;
                isMoving = false;
                player.transform.position = new Vector3(0, 5.125f, 0);      //Sets new position to player
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
                rotateProgress += rotateSpeed;
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
                rotateProgress += rotateSpeed;
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
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;                         //Sets bool for rotation check
                    playerPOS.z = -5f;                          //Moves player to new position
                    player.transform.position = playerPOS;      //Sets new position to player
                    player.scaleUpRevertQueued = true;
                }
            }
        }
    }
    public void Rotate(string way) //Right: Z-, Left, Z+
    {
        if(soundOn)
        {
            rotateSound.Play();
        }
        rotateWay = way;                                        //Stores the rotation orientation
        rotateQueued = true;                                    //Tells the controller to start a rotation
        playerPOS = player.gameObject.transform.position;       //Stores the player position
    }

    public void NextLevel()
    {
        if (level<levels.Count-1)
        {
            if (soundOn)
            {
                nextLevelSound.Play();
            }
            moveQueued = true;
            level++;
            SetCubeControl();
            player.gameObject.SetActive(false);                     //Removes the player from view
        }
        else
        {
            //No more levels
        }
        

    }

    public void SetCubeControl()
    {
        cube = levels[level];
    }

    public void ResetLevel()
    {
        cube.transform.rotation = new Quaternion();
        foreach (GameObject go in resetDisables)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in resetEnables)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in resetRotates)
        {
            go.transform.rotation = new Quaternion();
        }
        textController.Init();

    }

    public void OptionChange(string option)
    {
        if(option == "timer")
        {
            if (timerOn)
            {
                timerOn = false;
                textOptionTimer.text = textController.GetOptionText(0) + " : " + textController.onoffEnglish[1];
            }
            else
            {
                timerOn = true;
                textOptionTimer.text = textController.GetOptionText(0) + " : " + textController.onoffEnglish[0];
            }
        }
        if (option == "music")
        {
            if (musicOn)
            {
                music.Stop();
                textOptionMusic.text = textController.GetOptionText(1) + " : " + textController.onoffEnglish[1];
                musicOn = false;
            }
            else
            {
                music.time = 2;
                music.Play();
                textOptionMusic.text = textController.GetOptionText(1) + " : " + textController.onoffEnglish[0];
                musicOn = true;
            }
        }
        if (option == "sounds")
        {
            if (soundOn)
            {
                textOptionSounds.text = textController.GetOptionText(2) + " : " + textController.onoffEnglish[1];
                soundOn = false;
            }
            else
            {
                textOptionSounds.text = textController.GetOptionText(2) + " : " + textController.onoffEnglish[0];
                soundOn = true;
            }
        }
        if (option == "hardcore")
        {
            if (isHardcore)
            {
                textOptionHardcore.text = textController.GetOptionText(3) + " : " + textController.onoffEnglish[1];
                textTimer.color = Color.white;
                textLevel.color = Color.white;
                isHardcore = false;
            }
            else
            {
                textOptionHardcore.text = textController.GetOptionText(3) + " : " + textController.onoffEnglish[0];
                textTimer.color = Color.red;
                textLevel.color = Color.red;
                isHardcore = true;
            }
        }
    }

    public void ResetGame()
    {
        level = 0;
        allLevels.transform.position = new Vector3();
        foreach(GameObject go in levels)
        {
            go.transform.rotation = new Quaternion();
        }
        foreach (GameObject go in resetDisables)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in resetEnables)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in resetRotates)
        {
            go.transform.rotation = new Quaternion();
        }
        SetCubeControl();
        textController.Init();
        timer = 0;
    }
}
