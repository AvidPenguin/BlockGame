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
    public int level;                          // Current level
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

    public int currentRunLevel;
    public float currentRunTimer;

    private void Start()
    {
        level = 0;
        LoadGame();
        UpdateOptionsUI();
        VersionText.text = "Build:" + Application.version;
        SetCubeControl();
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

    public void NextLevel()
    {
        if (level<levels.Count-1)
        {
            if (soundOn)
            {
                nextLevelSound.Play();
            }
            rotationController.moveDirection = RotationController.MoveDirections.NextLevel;
            rotationController.moveQueued = true;
            level++;
            if(level > maxLevel) //Player reaches a new highest level
            {
                maxLevel = level;
                PlayerPrefs.SetInt("maxlevel", level);
            }
            SetCubeControl();

            if(!hardcoreOn)
            {
                currentRunLevel = level;
                PlayerPrefs.SetInt("currentlevel", level);
                PlayerPrefs.SetFloat("currenttimer", timer);
            }
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

    public void ResetLevel() //Resets all levels, but keeps player in game
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

    public void ResetGame() //Resets all levels, and clears run progress
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
        timer = 0;
        currentRunLevel = 0;
        currentRunTimer = 0;
        PlayerPrefs.SetFloat("currenttimer", 0);
        PlayerPrefs.SetInt("currentlevel", 0);
        UpdateOptionsUI();
        mainMenu.SetActive(true);
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
        PlayerPrefs.SetInt("language", GetLocale());
    }
    public void LoadGame()
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
            if (soundOn)
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

    public void ChangeLanguage(string locale)
    {
        if (locale == "english")
        {
            SetLocale(0);
        }
        if (locale == "french")
        {
            SetLocale(1);
        }
        if (locale == "german")
        {
            SetLocale(2);
        }
        if (locale == "polish")
        {
            SetLocale(3);
        }
        if (locale == "portuguese")
        {
            SetLocale(4);
        }
        if (locale == "russian")
        {
            SetLocale(5);
        }
        if (locale == "spanish")
        {
            SetLocale(6);
        }
    }

    public void SetLocale(int localeId)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
    }

    public int GetLocale()
    {
        return LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
    }

    public void PlayButton()
    {
        PlayerPrefs.SetInt("currentlevel", 0);
        PlayerPrefs.SetFloat("currenttimer", 0);
        rotationController.SetLevel(0);
        mainMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ContinueButton()
    {
        rotationController.SetLevel(PlayerPrefs.GetInt("currentlevel"));
        timer = PlayerPrefs.GetFloat("currenttimer");
        mainMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ToggleHardcore()
    {
        hardcoreOn = !hardcoreOn;
        
        UpdateOptionsUI();
        SaveGame();
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
        SaveGame();
    }
    public void ToggleSounds()
    {
        soundOn = !soundOn;
        UpdateOptionsUI();
        SaveGame();
    }

    public void UpdateOptionsUI()
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

        SaveGame();
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
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        level = 0;
        allLevels.transform.position = new Vector3();
        ResetLevel();
        SetCubeControl();
        timer = 0;
        currentRunLevel = 0;
        currentRunTimer = 0;
        PlayerPrefs.SetFloat("currenttimer", 0);
        PlayerPrefs.SetInt("currentlevel", 0);
        UpdateOptionsUI();
        Resume();
    }

    public void MainMenu()
    {
        PlayerPrefs.SetFloat("currenttimer", timer);
        level = 0;
        timer = 0;
        ResetLevel();
        UpdateOptionsUI();
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    [SerializeField]
    private Selectable selectable = null;
    public void Hover()
    {
        selectable.Select();
    }

}
