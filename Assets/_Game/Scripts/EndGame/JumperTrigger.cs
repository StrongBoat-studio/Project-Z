using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumperTrigger : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    private bool _isTriger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isTriger == false)
        {
            Debug.Log("Collision with player");

            GameObject.FindGameObjectWithTag("PlayerCanvas").SetActive(false);
            _canvasGroup.DOFade(1f, .5f).WaitForCompletion();

            StartCoroutine(Quit());
        }
    }

    private IEnumerator Quit()
    {
        yield return new WaitForSeconds(3f);

        Application.Quit();
    }
}
