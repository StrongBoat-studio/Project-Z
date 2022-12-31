using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public enum CursorType
    {
        Arrow,
        Grab,
        Attack
    }

    public static CursorManager Instance { get; private set; }

    [SerializeField] private List<CursorAnimation> _cursorAnimations;
    private CursorAnimation _currentAnimation;
    private int _frameCount;
    private int _currentFrame;
    private float _frameTimer;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetActiveCursorAnimation(CursorType.Arrow);
    }

    private void Update()
    {
        _frameTimer -= Time.deltaTime;
        if (_frameTimer <= 0)
        {
            _frameTimer += _currentAnimation.frameRate;
            _currentFrame = (_currentFrame + 1) % _frameCount;
            Cursor.SetCursor(_currentAnimation.textures[_currentFrame], _currentAnimation.offset, CursorMode.Auto);
        }
    }

    private CursorAnimation GetCursorAnimation(CursorType type)
    {
        return _cursorAnimations.Find(x => x.cursorType == type);
    }

    private void SetActiveCursorAnimation(CursorAnimation cursorAnimation) 
    {
        _currentAnimation = cursorAnimation;
        _currentFrame = 0;
        _frameTimer = 0;
        _frameCount = cursorAnimation.textures.Length;
    }

    ///<summary>
    ///Change cursor animation
    ///</summary>
    ///<param name="type"></param>
    public void SetActiveCursorAnimation(CursorType type)
    {
        SetActiveCursorAnimation(GetCursorAnimation(type));
    }
}
