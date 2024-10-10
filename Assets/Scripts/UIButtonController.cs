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
                text.color = new Color32(106, 151, 206, 255);
            }
        }
        else
        {
            foreach (TextMeshProUGUI text in texts)
            {
                text.fontSize = 96;
                text.color = Color.white;
            }
        }
        // check if button is selected
        // do something to the texts inside it
    }
}