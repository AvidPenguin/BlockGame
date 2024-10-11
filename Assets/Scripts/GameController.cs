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

    // Menu Elements
    public bool inMenu;
    public GameObject mainMenu;
    public GameObject logo;
    public GameObject logoRed;
    public TextMeshProUGUI OptMusicOn;
    public TextMeshProUGUI OptMusicOff;
    public TextMeshProUGUI OptSoundsOn;
    public TextMeshProUGUI OptSoundsOff;
    public TextMeshProUGUI OptHardcoreOn;
    public TextMeshProUGUI OptHardcoreOff;
    public GameObject menuBackground;

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
    public bool hardcoreOn;

    //Camera variables
    public Camera cam;
    public bool camFinishedMove;                // Used for FOV Sliding effect on launch


    // Audio objects
    public bool musicOn;
    public bool soundOn;
    public AudioSource music;
    public AudioSource rotateSound;
    public AudioSource switchSound;
    public AudioSource nextLevelSound;

    private void Start()
    {
        inMenu = true;
        level = 0;
        musicOn = true;
        soundOn = true;
        hardcoreOn = false;
        SetCubeControl();
        LoadGame();
        if (musicOn)
        {
            music.time = 2;
            music.Play();
        }

        UpdateOptionsUI();
    }

    void Update()
    {
        if (level == 0)
        {
            textLevel.text = "";
            textTimer.text = "";
        }
        else
        {
            textLevel.text = level.ToString();
            textTimer.text = timer.ToString("N1");
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
            hardcoreOn = !hardcoreOn;
        }

        SaveGame();
        UpdateOptionsUI();
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
        if (hardcoreOn)
        {
            PlayerPrefs.SetInt("hardcore", 1);
        }
        else
        {
            PlayerPrefs.SetInt("hardcore", 0);
        }
        PlayerPrefs.SetInt("language", LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale));
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
                hardcoreOn = true;
            }
            else
            {
                hardcoreOn = false;
            }
        }
        else
        {
            if (hardcoreOn)
            {
                PlayerPrefs.SetInt("hardcore", 1);
            }
            else
            {
                PlayerPrefs.SetInt("hardcore", 0);
            }
        }
        //-----------------------------------------------
        if (PlayerPrefs.HasKey("language"))
        {
            StartCoroutine(SetLocale(PlayerPrefs.GetInt("language")));
        }
        else
        {
            PlayerPrefs.SetInt("language", LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale));
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
        inMenu = false;
    }

    public void ToggleHardcore()
    {
        hardcoreOn = !hardcoreOn;
        
        UpdateOptionsUI();
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;
        if (musicOn)
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
    public void ToggleSounds()
    {
        soundOn = !soundOn;
        UpdateOptionsUI();
    }

    public void UpdateOptionsUI()
    {
        OptMusicOn.gameObject.SetActive(musicOn);
        OptMusicOff.gameObject.SetActive(!musicOn);
        OptSoundsOn.gameObject.SetActive(soundOn);
        OptSoundsOff.gameObject.SetActive(!soundOn);
        OptHardcoreOn.gameObject.SetActive(hardcoreOn);
        OptHardcoreOff.gameObject.SetActive(!hardcoreOn);
        
        if (!hardcoreOn)
        {
            menuBackground.GetComponent<Image>().color = new Color32(67, 67, 106, 255);
            cam.backgroundColor = new Color32(67, 67, 106, 255);
            logo.SetActive(true);
            logoRed.SetActive(false);
        }
        else
        {
            menuBackground.GetComponent<Image>().color = new Color32(106, 67, 67, 255);
            cam.backgroundColor = new Color32(106, 67, 67, 255);
            logo.SetActive(false);
            logoRed.SetActive(true);
        }
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
