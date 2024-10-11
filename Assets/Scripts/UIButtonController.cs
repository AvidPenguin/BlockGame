using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private Selectable selectable = null;
    public List<TextMeshProUGUI> texts;
    public EventSystem es;

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectable.Select();
    }

    public void Start()
    {
        es = FindAnyObjectByType<EventSystem>();
    }

    public void Update()
    {
        if (es.currentSelectedGameObject == this.gameObject)
        {
            foreach (TextMeshProUGUI text in texts)
            {
                text.fontSize = 120;
                if(FindObjectOfType<GameController>().hardcoreOn)
                {
                    text.color = new Color32(206, 151, 106, 255);
                }
                else
                {
                    text.color = new Color32(106, 151, 206, 255);
                }
            }
        }
        else
        {
            try
            {
                foreach (TextMeshProUGUI text in texts)
                {
                    text.fontSize = 96;
                    text.color = Color.white;
                }
            }
            catch
            {
                // Stops error when nothing is selected
            }
            
        }
        // check if button is selected
        // do something to the texts inside it
    }
}