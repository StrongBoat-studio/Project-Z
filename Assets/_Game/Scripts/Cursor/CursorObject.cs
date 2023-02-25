using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObject : MonoBehaviour
{
    [SerializeField] private CursorManager.CursorType _cursorType;

    private void OnMouseEnter()
    {
        CursorManager.Instance.SetActiveCursorAnimation(_cursorType);
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetActiveCursorAnimation(CursorManager.CursorType.Arrow);
    }
}
