using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Note : MonoBehaviour
{
    [SerializeField] private RectTransform _text;
    [SerializeField] private TextMeshProUGUI _title;

    public void Init(Note note)
    {
        _title.text = note.noteTitle;
        _text.GetComponent<TextMeshProUGUI>().text = note.noteText.text;
        _text.anchoredPosition = new Vector3(10f, 0f);
    }
}
