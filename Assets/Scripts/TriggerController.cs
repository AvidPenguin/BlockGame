using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TriggerController : MonoBehaviour
{
    public bool show;
    public List<GameObject> showTargets;
    public bool hide;
    public List<GameObject> hideTargets;
    public bool rotate;
    public List<GameObject> rotateTargets;
    public bool retext;
    public List<int> textIds;
    public List<TextMeshPro> retextTargets;
    public bool achievement;
    public string achievementTitle;
    public bool keep;
    public bool optionToggle;
    public string option;
    public bool quit;
    public bool rotationbased;
    public bool playSound;
    public bool rotateCube;
    public bool clockwise;


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
        if (show)
        {
            foreach(GameObject go in showTargets)
            {
                go.SetActive(true);
            }
        }
        if (hide)
        {
            foreach (GameObject go in hideTargets)
            {
                go.SetActive(false);
            }
        }
        if (rotate)
        {
            foreach (GameObject go in rotateTargets)
            {
                go.transform.Rotate(Vector3.up, 90);
            }
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
            if(gc.soundOn)
            {
                gc.switchSound.Play();
            }
        }
        if (!keep)
        {
            gameObject.SetActive(false);
        }
        if(rotateCube)
        {
            RotationController rc = FindAnyObjectByType<RotationController>();
            if(!rc.rotateQueued)
            {
                rc.rotateQueued = true;
                if(clockwise)
                {
                    rc.rotateDirection = RotationController.Rotations.Clockwise;
                }
                else
                {
                    rc.rotateDirection = RotationController.Rotations.CounterClockwise;
                }
            }
        }
    }
}