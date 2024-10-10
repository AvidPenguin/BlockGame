using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Controllers
    public PlayerController playerController;
    public RotationController rotationController;

    // UI Elements
    public GameObject mainMenu;
    public GameObject OptAudioOn;
    public GameObject OptAudioOff;

    // Core Variables
    public GameObject cube;                    // The cube the player is currently on
    public GameObject allLevels;                // The parent empty GameObject containing all the levels
    public List<GameObject> levels;             // Ordered list of levels
    private int level;                          // Current level

    // Reset Variables
    public List<GameObject> resetDisables;      // List of all objects to disable on a reset
    public List<GameObject> resetEnables;       // List of all objects to enable on a reset
    public List<GameObject> resetRotates;       // List of all objects to rotate on a reset

    // In Game UI Objects
    public TextMeshPro textLevel;           // UI Level Text Object
    public TextMeshPro textTimer;           // UI Timer Text Object

    // Option objects and variables
    public double timer;
    public bool isHardcore;
    public string language;

    //Camera variables
    public Camera cam;
    public bool camFinishedMove;                // Used for FOV Sliding effect on launch


    // Audio objects
    public bool soundOn;
    public AudioSource music;
    public AudioSource rotateSound;
    public AudioSource switchSound;
    public AudioSource nextLevelSound;

    private void Start()
    {
        level = 0;
        music.time = 2;
        soundOn = true;
        isHardcore = false;

        music.Play();
        SetCubeControl();
        LoadGame();
        UpdateOptionsUI();
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
            textTimer.text = "Timer: " + timer.ToString("N1");
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
                playerController.canMove = true;
            }
        }
        if (level != 0)
        {

            timer += Time.deltaTime;
        }
    }
    public void Rotate(string way) //Right: Z-, Left, Z+
    {
        if(soundOn)
        {
            rotateSound.Play();
        }
        rotationController.rotateDirection = way;                                        //Stores the rotation orientation
        rotationController.rotateQueued = true;                                    //Tells the controller to start a rotation
        rotationController.playerPOS = playerController.gameObject.transform.position;       //Stores the player position
    }

    public void NextLevel()
    {
        if (level<levels.Count-1)
        {
            if (soundOn)
            {
                nextLevelSound.Play();
            }
            rotationController.moveQueued = true;
            level++;
            SetCubeControl();
            playerController.gameObject.SetActive(false);                     //Removes the player from view
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
        //textController.Init();

    }

    public void OptionChange(string option)
    {
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
        /**
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
        */
    }

    public void ResetLevels()
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
        //textController.Init();
        timer = 0;
    }

    public void SaveGame()
    {
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

    public void ChangeLanguage(string locale)
    {
        if (locale == "english")
        {
            StartCoroutine(SetLocale(0));
        }
        if (locale == "french")
        {
            StartCoroutine(SetLocale(1));
        }
        if (locale == "german")
        {
            StartCoroutine(SetLocale(2));
        }
        if (locale == "polish")
        {
            StartCoroutine(SetLocale(3));
        }
        if (locale == "portuguese")
        {
            StartCoroutine(SetLocale(4));
        }
        if (locale == "russian")
        {
            StartCoroutine(SetLocale(5));
        }
        if (locale == "spanish")
        {
            StartCoroutine(SetLocale(6));
        }
    }
    
    IEnumerator SetLocale(int localeId)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
    }

    public void PlayButton()
    {
        mainMenu.SetActive(false);
    }

    public void OptionsButton()
    {
        mainMenu.SetActive(false);
    }

    public void ToggleHardcore()
    {
        isHardcore = !isHardcore;
        UpdateOptionsUI();
    }

    public void ToggleAudio()
    {
        soundOn = !soundOn;
        if (soundOn)
        {
            music.time = 2;
            music.Play();
        }
        else
        {
            music.Stop();
        }
        UpdateOptionsUI();
    }

    public void UpdateOptionsUI()
    {
        OptAudioOn.SetActive(soundOn);
        OptAudioOff.SetActive(!soundOn);
    }

    public void ToggleLanguage()
    {
        int i = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        if (i > 5) //Catches int overflow
        {
            i = 0;
        }
        else
        {
            i++;
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
    }

    public void QuitGame()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    [SerializeField]
    private Selectable selectable = null;
    public void Hover()
    {
        selectable.Select();
    }

}
