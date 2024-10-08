using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

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
    public int movesToDo = 1;
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
        LoadGame();
        OptionUpdateTexts();
    }

    void Update()
    {
        if (level == 0) 
        {
            textLevel.text = "";
        }
        else //Add language options
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

            if (moveProgress == (50*movesToDo))
            {
                moveQueued = false;
                moveProgress = 0;
                isMoving = false;
                player.transform.position = new Vector3(0, 5.125f, 0);      //Sets new position to player
                player.gameObject.SetActive(true);          //Reinstates the player to view
                movesToDo = 1;
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
                    rotateWay = "";
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
                    rotateWay = "";
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
                    rotateWay = "";
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
                    rotateWay = "";
                }
            }
            if (rotateWay == "clockwise")
            {
                doors.SetActive(false);
                cube.transform.RotateAround(cube.transform.position, Vector3.up, rotateSpeed);
                player.transform.RotateAround(cube.transform.position, Vector3.up, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;
                    doors.SetActive(true);
                }
            }
            if (rotateWay == "counterclockwise")
            {
                doors.SetActive(false);
                cube.transform.RotateAround(cube.transform.position, Vector3.down, rotateSpeed);
                player.transform.RotateAround(cube.transform.position, Vector3.down, rotateSpeed);
                rotateProgress += rotateSpeed;
                if (rotateProgress >= 90)
                {
                    rotateQueued = false;                       //Ends rotation
                    rotateProgress = 0;                         //Resets counter for next rotation
                    isRotating = false;
                    doors.SetActive(true);
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
            timerOn = !timerOn;
        }
        if (option == "music")
        {
            musicOn = !musicOn;
        }
        if (option == "sounds")
        {
            soundOn = !soundOn;
        }
        if (option == "hardcore")
        {
            isHardcore = !isHardcore;
        }

        OptionUpdateTexts();
        SaveGame();
    }

    public void OptionUpdateTexts()
    {
        if (!isHardcore)
        {
            textOptionHardcore.text = textController.GetOptionText(3) + " : " + textController.onoffEnglish[1];
            textTimer.color = Color.white;
            textLevel.color = Color.white;
        }
        else
        {
            textOptionHardcore.text = textController.GetOptionText(3) + " : " + textController.onoffEnglish[0];
            textTimer.color = Color.red;
            textLevel.color = Color.red;
        }
        if (!soundOn)
        {
            textOptionSounds.text = textController.GetOptionText(2) + " : " + textController.onoffEnglish[1];
        }
        else
        {
            textOptionSounds.text = textController.GetOptionText(2) + " : " + textController.onoffEnglish[0];
        }
        if (!musicOn)
        {
            music.Stop();
            textOptionMusic.text = textController.GetOptionText(1) + " : " + textController.onoffEnglish[1];
        }
        else
        {
            music.time = 2;
            music.Play();
            textOptionMusic.text = textController.GetOptionText(1) + " : " + textController.onoffEnglish[0];
        }
        if (!timerOn)
        {
            textOptionTimer.text = textController.GetOptionText(0) + " : " + textController.onoffEnglish[1];
        }
        else
        {
            textOptionTimer.text = textController.GetOptionText(0) + " : " + textController.onoffEnglish[0];
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

    public void SaveGame()
    {
        if (timerOn)
        {
            PlayerPrefs.SetInt("timer", 1);
        }
        else
        {
            PlayerPrefs.SetInt("timer", 0);
        }
        if (musicOn)
        {
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            PlayerPrefs.SetInt("music", 0);
        }
        if (soundOn)
        {
            PlayerPrefs.SetInt("sounds", 1);
        }
        else
        {
            PlayerPrefs.SetInt("sounds", 0);
        }
        if (isHardcore)
        {
            PlayerPrefs.SetInt("hardcore", 1);
        }
        else
        {
            PlayerPrefs.SetInt("hardcore", 0);
        }
    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("timer"))
        {
            if (PlayerPrefs.GetInt("timer") == 1)
            {
                timerOn = true;
            }
            else
            {
                timerOn = false;
            }
        }
        else
        {
            if (timerOn)
            {
                PlayerPrefs.SetInt("timer", 1);
            }
            else
            {
                PlayerPrefs.SetInt("timer", 0);
            }
        }
        //-----------------------------------------------
        if (PlayerPrefs.HasKey("music"))
        {
            if (PlayerPrefs.GetInt("music") == 1)
            {
                musicOn = true;
            }
            else
            {
                musicOn = false;
            }
        }
        else
        {
            if (musicOn)
            {
                PlayerPrefs.SetInt("music", 1);
            }
            else
            {
                PlayerPrefs.SetInt("music", 0);
            }
        }
        //-----------------------------------------------
        if (PlayerPrefs.HasKey("sounds"))
        {
            if (PlayerPrefs.GetInt("sounds") == 1)
            {
                soundOn = true;
            }
            else
            {
                soundOn = false;
            }
        }
        else
        {
            if (soundOn)
            {
                PlayerPrefs.SetInt("sounds", 1);
            }
            else
            {
                PlayerPrefs.SetInt("sounds", 0);
            }
        }
        //-----------------------------------------------
        if (PlayerPrefs.HasKey("hardcore"))
        {
            if (PlayerPrefs.GetInt("hardcore") == 1)
            {
                isHardcore = true;
            }
            else
            {
                isHardcore = false;
            }
        }
        else
        {
            if (isHardcore)
            {
                PlayerPrefs.SetInt("hardcore", 1);
            }
            else
            {
                PlayerPrefs.SetInt("hardcore", 0);
            }
        }
    }
}
