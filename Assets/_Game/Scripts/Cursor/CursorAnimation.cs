using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GUI/Cursor Animation", fileName = "CursorAnimation")]
public class CursorAnimation : ScriptableObject
{
    public CursorManager.CursorType cursorType;
    public Texture2D[] textures;
    public float frameRate;
    public Vector2 offset;
}
