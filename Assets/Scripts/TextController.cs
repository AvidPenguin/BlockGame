using Palmmedia.ReportGenerator.Core.Reporting.Builders;
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
        "Play", "Options", "Credits", "Quit", "A game by\n\nDan Woolford","Level Design\nDan Woolford\n\nProgramming\nDan Woolford\n\nMusic\nFlorian Stracker",
        "Testers","Sheona Brown", "Are you sure?","The goal is to get to the green square to advance to the next cube", "It's that simple", "Options",
        "Actually, it's not always that simple","That'd make the whole game a bit boring, no","Is this the right way?","Is this the right way?","Is this the right way?",
        "It certainly looks like it","Thrilling gameplay section","Nope","Nope","This is a locked door\n\nThere must be a way to unlock it","This is my switch and I love it\n\nI have no idea what it does though",
        "Oh, you're the exploring type","This switch is orientation based\n\nTry making the arrow face upwards","Try to not think in two dimensions","A cube is a three dimensional object, after all",
        "If it isn't obvious, red = bad\n\nTry to avoid anything that's red","That switch looks useful..."
       };

    public readonly string[] retextsEnglish = {
        "This is a locked do...\nNevermind, you got it","Imagine an achievement popped up","That switch was useful","Well, goodbye forever my love..."
    };

    public readonly string[] optionsEnglish = {
        "Timer", "Music", "Sounds","Hardcore"
    };

    public readonly string[] onoffEnglish = {
        "On", "Off"
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
