using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void CursorClick();
    public void CursorEnter();
    public void CursorExit();
}