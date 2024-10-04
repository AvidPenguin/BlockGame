using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TriggerController : MonoBehaviour
{
    public bool show;
    public GameObject showTarget;
    public bool hide;
    public GameObject hideTarget;
    public bool retext;
    public string text;
    public TextMeshPro retextTarget;
    public bool achievement;
    public string achievementTitle;
    public bool keep;
    public bool optionToggle;
    public string option;
    public bool quit;
    public bool rotationbased;
    public bool playSound;


    // Start is called before the first frame update
    public void Trigger()
    {
        if (rotationbased)
        {
            if(gameObject.transform.rotation.y < 0.1f && gameObject.transform.rotation.y > -0.1f) //Checks orientation with error margin
            {
                Action();
            }
        }
        else
        {
            Action();
        }
    }

    void Action()
    {
        if (retext)
        {
            retextTarget.text = text;
        }
        if (show)
        {
            showTarget.SetActive(true);
        }
        if (hide)
        {
            hideTarget.SetActive(false);
        }
        if (achievement)
        {
            PlayerController pc = FindAnyObjectByType<PlayerController>();
            pc.achievements.Add(achievementTitle);
        }
        if (quit)
        {
            #if UNITY_STANDALONE
                        Application.Quit();
            #endif
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        if (optionToggle)
        {
            GameController gc = FindAnyObjectByType<GameController>();
            gc.OptionChange(option);

        }
        if(playSound)
        {
            GameController gc = FindAnyObjectByType<GameController>();
            gc.switchSound.Play();
        }
        if (!keep)
        {
            gameObject.SetActive(false);
        }
    }
}