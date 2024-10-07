using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TeleporterController : MonoBehaviour
{
    public TeleporterController partner;
    public bool active;

    private void Start()
    {
        active = true;
    }

}
