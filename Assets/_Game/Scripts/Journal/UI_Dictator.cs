using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;

public class UI_Dictator : MonoBehaviour
{
    private Story _story;
    [SerializeField] private float _typeSpeed;
    [SerializeField] private TextAsset storyTextAsset;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _dictator;

    private void ContinueStory()
    {
        if (_story.canContinue)
        {
            string text = _story.Continue();

            StartCoroutine("TypeMessage", text);
        }
        else
        {
            StartCoroutine(EndStory());
        }
    }

    private IEnumerator TypeMessage(string message)
    {
        _text.text = "";
        foreach (char c in message)
        {
            _text.text += c;
            yield return new WaitForSeconds(1f / _typeSpeed);
        }
        ContinueStory();
    }

    private IEnumerator EndStory()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }

    public void End()
    {
        _story = new Story(storyTextAsset.text);
        Debug.Log("close");
        _dictator.SetActive(false);
    }

    public void StartVideo()
    {
        _story = new Story(storyTextAsset.text);
        ContinueStory();
    }
}
