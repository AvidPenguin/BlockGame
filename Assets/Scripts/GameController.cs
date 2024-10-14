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


    [SerializeField]
    private Selectable selectable = null;
    // Controllers
    public PlayerController playerController;
    public RotationController rotationController;

    // Menu Elements
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject logo;
    public GameObject logoRed;
    public GameObject logoPause;
    public GameObject logoRedPause;
    public TextMeshProUGUI OptMusicOn;
    public TextMeshProUGUI OptMusicOff;
    public TextMeshProUGUI OptSoundsOn;
    public TextMeshProUGUI OptSoundsOff;
    public TextMeshProUGUI OptHardcoreOn;
    public TextMeshProUGUI OptHardcoreOff;
    public TextMeshProUGUI OptMusicOnPause;
    public TextMeshProUGUI OptMusicOffPause;
    public TextMeshProUGUI OptSoundsOnPause;
    public TextMeshProUGUI OptSoundsOffPause;
    public GameObject menuBackground;
    public GameObject menuBackgroundPause;
    public TextMeshProUGUI VersionText;
    public GameObject buttonPlay;
    public GameObject buttonContinue;

    // Core Variables
    public GameObject cube;                    // The cube the player is currently on
    public GameObject allLevels;                // The parent empty GameObject containing all the levels
    public List<GameObject> levels;             // Ordered list of levels
    public int level = 0;                          // Current level
    public int maxLevel;

    // Reset Variables
    public List<GameObject> resetDisables;      // List of all objects to disable on a reset
    public List<GameObject> resetEnables;       // List of all objects to enable on a reset
    public List<GameObject> resetRotates;       // List of all objects to rotate on a reset

    // In Game UI Objects
    public TextMeshProUGUI textLevel;           // UI Level Text Object
    public TextMeshProUGUI textTimer;           // UI Timer Text Object

    // Option objects and variables
    public float timer;
    public bool hardcoreOn = false;

    //Camera variables
    public Camera cam;
    public bool camFinishedMove;                // Used for FOV Sliding effect on launch


    // Audio objects
    public bool musicOn = true;
    public bool soundOn = true;
    public AudioSource music;
    public AudioSource rotateSound;
    public AudioSource switchSound;
    public AudioSource nextLevelSound;

    public int currentRunLevel;
    public float currentRunTimer;

    private void Start()
    {
        VersionText.text = "Build:" + Application.version;
        LoadData();
        UpdateUI();

        if (musicOn)
        {
            music.time = 2;
            music.Play();
        }

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

    public void Rotate(RotationController.Rotations way) //Right: Z-, Left, Z+
    {
        if(soundOn)
        {
            rotateSound.Play();
        }
        rotationController.rotateDirection = way;                                           //Stores the rotation orientation
        rotationController.rotateQueued = true;                                             //Tells the controller to start a rotation
        rotationController.playerPOS = playerController.gameObject.transform.position;      //Stores the player position
    }

    public void ResetLevels() //Resets Current Level (Does all but it doesn't matter)
    {
        foreach (GameObject go in levels)
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
    }

    public void ResetLevelsAndPlayer() //Resets all levels and sets player to level 1
    {
        level = 0;
        cube = levels[level];
        allLevels.transform.position = new Vector3();
        foreach (GameObject go in levels)
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
        playerController.transform.position = new Vector3(0, 5f, 0);
    }

    public void LoadData()
    {
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
            PlayerPrefs.SetInt("music", 1);
            musicOn = true;
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
            PlayerPrefs.SetInt("sounds", 1);
            soundOn = true;
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
            PlayerPrefs.SetInt("hardcore", 0);
            hardcoreOn = false;
        }
        //-----------------------------------------------
        if (PlayerPrefs.HasKey("language"))
        {
            SetLocale(PlayerPrefs.GetInt("language"));
        }
        else
        {
            PlayerPrefs.SetInt("language", GetLocale());
        }
        //-----------------------------------------------
        if (PlayerPrefs.HasKey("maxlevel"))
        {
            maxLevel = PlayerPrefs.GetInt("maxlevel");
        }
        else
        {
            PlayerPrefs.SetInt("maxlevel", 0);
        }
        //-----------------------------------------------
        if (PlayerPrefs.HasKey("currentlevel"))
        {
            currentRunLevel = PlayerPrefs.GetInt("currentlevel");
        }
        else
        {
            PlayerPrefs.SetInt("currentlevel", 0);
        }
        //-----------------------------------------------
        if (PlayerPrefs.HasKey("currenttimer"))
        {
            currentRunTimer = PlayerPrefs.GetFloat("currenttimer");
        }
        else
        {
            PlayerPrefs.SetFloat("currenttimer", 0);
        }
    }

    public int GetLocale()
    {
        return LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
    }

    public void ButtonNewRun()
    {
        ClearProgress();
        ResetLevelsAndPlayer();
        MenuToGame();
    }

    public void ButtonContinue()
    {
        ResetLevelsAndPlayer();
        rotationController.SetLevel(currentRunLevel);
        timer = currentRunTimer;
        MenuToGame();
    }

    public void ToggleHardcore()
    {
        hardcoreOn = !hardcoreOn;
        if (hardcoreOn)
        {
            PlayerPrefs.SetInt("hardcore", 1);
        }
        else
        {
            PlayerPrefs.SetInt("hardcore", 0);
        }
        UpdateUI();
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;
        if (musicOn)
        {
            music.time = 2;
            music.Play();
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            music.Stop();
            PlayerPrefs.SetInt("music", 0);
        }
        UpdateUI();
    }

    public void ToggleSounds()
    {
        soundOn = !soundOn;
        if (soundOn)
        {
            PlayerPrefs.SetInt("sounds", 1);
        }
        else
        {
            PlayerPrefs.SetInt("sounds", 0);
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        OptMusicOn.gameObject.SetActive(musicOn);
        OptMusicOff.gameObject.SetActive(!musicOn);
        OptSoundsOn.gameObject.SetActive(soundOn);
        OptSoundsOff.gameObject.SetActive(!soundOn);
        OptMusicOnPause.gameObject.SetActive(musicOn);
        OptMusicOffPause.gameObject.SetActive(!musicOn);
        OptSoundsOnPause.gameObject.SetActive(soundOn);
        OptSoundsOffPause.gameObject.SetActive(!soundOn);
        OptHardcoreOn.gameObject.SetActive(hardcoreOn);
        OptHardcoreOff.gameObject.SetActive(!hardcoreOn);
        
        if (!hardcoreOn)
        {
            menuBackground.GetComponent<Image>().color = new Color32(67, 67, 106, 255);
            menuBackgroundPause.GetComponent<Image>().color = new Color32(67, 67, 106, 255);
            cam.backgroundColor = new Color32(67, 67, 106, 255);
            logo.SetActive(true);
            logoRed.SetActive(false);
            logoPause.SetActive(true);
            logoRedPause.SetActive(false);
        }
        else
        {
            menuBackground.GetComponent<Image>().color = new Color32(106, 67, 67, 255);
            menuBackgroundPause.GetComponent<Image>().color = new Color32(106, 67, 67, 255);
            cam.backgroundColor = new Color32(106, 67, 67, 255);
            logo.SetActive(false);
            logoRed.SetActive(true);
            logoPause.SetActive(false);
            logoRedPause.SetActive(true);
        }
        if (currentRunLevel == 0 || hardcoreOn)
        {
            buttonContinue.SetActive(false);
        }
        else
        {
            buttonContinue.SetActive(true);
        }
    }
    
    public void ButtonRestart()
    {
        ClearProgress();
        ResetLevelsAndPlayer();
        PauseToGame();
    }

    public void MenuToGame()
    {
        mainMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseToGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameToMenu()
    {
        UpdateUI();
        mainMenu.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void GameToPause()
    {
        UpdateUI();
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void PauseToMenu()
    {
        UpdateUI();
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SetLocale(int localeId)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
    }

    public void NextLevel()
    {
        if (level < levels.Count - 1)                                                           // No more levels - Crash detection
        {
            if (soundOn){nextLevelSound.Play();}                                                // Plays next level sound effect
            rotationController.moveDirection = RotationController.MoveDirections.NextLevel;     // Tells the RC to move to the next level
            rotationController.moveQueued = true;                                               // Starts the RC move process
        } else { }                                                                              // No more levels - Crash detection
    }

    public void SaveProgress()
    {
        if (level > maxLevel)                                                                   // If the player reaches a new highest level, stores it in the prefs
        {
            maxLevel = level;
            PlayerPrefs.SetInt("maxlevel", level);
        }
        if (!hardcoreOn)
        {
            currentRunLevel = level;
            currentRunTimer = timer;
            PlayerPrefs.SetInt("currentlevel", level);
            PlayerPrefs.SetFloat("currenttimer", timer);
        }
        else
        {
            ClearProgress();
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

        PlayerPrefs.SetInt("language", GetLocale());
    }

    public void ClearProgress()
    {
        currentRunTimer = 0;
        currentRunLevel = 0;
        PlayerPrefs.SetFloat("currenttimer", 0);
        PlayerPrefs.SetInt("currentlevel", 0);
    }

    public void Hover()
    {
        selectable.Select();
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

    public void PauseMenu()
    {
        GameToPause();
    }
    
    public void Resume()
    {
        PauseToGame();
    }
    
    public void MainMenu()
    {
        ResetLevelsAndPlayer();
        PauseToMenu();
    }
}
