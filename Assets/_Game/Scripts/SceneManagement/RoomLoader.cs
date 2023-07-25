using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class RoomLoader : MonoBehaviour, IInteractable
{
    [Tooltip("This scene")]
    [SerializeField] private SceneRegister.Scenes _originScene;

    [Tooltip("ID of this door (can be repeted on different scene")]
    [SerializeField] private int _id;

    [Tooltip("Spawn location of this door")]
    [SerializeField] private Transform _spawnPoint;

    [Tooltip("Tageted scene for destination door")]
    [SerializeField] private SceneRegister.Scenes _targetScene;

    [Tooltip("Door on target scene")]
    [SerializeField] private int _targetDoorId;

    private LocalKeyword _OUTLINE_ON;

    [SerializeField] private Color _canInteractColor;
    [SerializeField] private Color _cannotInteractColor;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(GetComponent<SpriteRenderer>().material.shader, "_OUTLINE_ON");
    }

    public void CursorClick()
    {
        SceneRegister.Instance.LoadNextLevel(this);

        //Update quests
        if(GetComponents<QuestObjective>().Length <= 0) return;
        if(QuestLineManager.Instance == null) 
        {
            Debug.Log("QuestLineManager Instance is null");
            return;
        }
        foreach(QuestObjective qo in GetComponents<QuestObjective>())
        {
            QuestLineManager.Instance.CheckQuest(qo);
        }
    }

    public void CursorEnter(bool canInteract)
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
        GetComponent<SpriteRenderer>().material.SetColor("_Color", canInteract ? _canInteractColor : _cannotInteractColor);
    }

    public void CursorExit()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, false);
    }

    public (int id, SceneRegister.Scenes scene) GetOriginDoor()
    {
        return (_id, _originScene);
    }

    public (int id, SceneRegister.Scenes scene) GetTargetDoor()
    {
        return (_targetDoorId, _targetScene);
    }

    public Vector3 GetSpawnPosition()
    {
        return _spawnPoint.position;
        //return new Vector3(transform.position.x - _spawnPoint.localPosition.x, transform.position.y + _spawnPoint.localPosition.y, _spawnPoint.position.z);
    }

    public void SetTargetScene(SceneRegister.Scenes scene)
    {
        _targetScene = scene;
    }
}
