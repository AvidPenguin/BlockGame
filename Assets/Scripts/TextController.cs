using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public string language = "en";
    public List<TextMeshPro> textObjects;
    private readonly string[] textsEnglish = {
        // Level 0
        "Play", "Options", "Credits", "Quit", "A game by\n\nDan Woolford","Level Design\nDan Woolford\n\nProgramming\nDan Woolford\n\nMusic\nFlorian Stracker", "Testing","Sheona Brown", "Are you sure?",
        "The goal is to get to the green square to advance to the next cube", "It's that simple", "Options",
        // Level 1
        "Actually, it's not always that simple","That'd make the whole game a bit boring, no?","Is this the right way?","Is this the right way?","Is this the right way?","It certainly looks like it","Thrilling gameplay section","Nope","Nope",
        // Level 2
        "This is a locked door\n\nThere must be a way to unlock it","This is my switch and I love it\n\nI have no idea what it does though", "Oh, you're the exploring type",
        // Level 3
        "This switch is orientation based\n\nTry making the arrow face upwards","Try to not think in two dimensions","A cube is a three dimensional object, after all",
        // Level 4
        "If it isn't obvious, red = bad\n\nTry to avoid anything that's red","That switch looks useful...","After a rotation, you will always be safe if you don't move", "Use this time to learn the timings and make your move",
        "Gotta go fast",
        // Level 5
        "Teleporters aren't just used to finish a level\n\nThey can teleport you to their matching counterpart in a level", "Thank you for playing the demo!"
       };

    public readonly string[] retextsEnglish = {
        "This is a locked do...\nNevermind, you got it","Imagine an achievement popped up","Aren't switches unpredictable","Well, goodbye forever my love..."
    };

    public readonly string[] optionsEnglish = {
        "Timer", "Music", "Sounds","Hardcore","Language"
    };

    public readonly string[] onoffEnglish = {
        "On", "Off"
    };

    public readonly string[] languages = {
        "English", "Francais", "Espanol" , "Deutsch" , "Italiano"
    };


    public void Init()
    {
        int i = 0;
        foreach (TextMeshPro tmp in textObjects)
        {
            if (language=="en")
            {
                tmp.text = textsEnglish[i];
            }
            //Add other languages here
            else
            {
                tmp.text = "Lang missing";
            }
            i++;
        }
    }

    public string GetOptionText(int optionId)
    {
        string s = "";
        if (language == "en")
        {
            s = optionsEnglish[optionId];
        }
        return s;

    }
}
