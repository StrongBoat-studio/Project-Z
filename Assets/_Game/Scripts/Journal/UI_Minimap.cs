using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Minimap : MonoBehaviour
{
    [System.Serializable]
    public struct MinimapLink
    {
        public MinimapLink(SceneRegister.Scenes sceneID, Vector2 mapCoord)
        {
            this.sceneID = sceneID;
            this.mapCoord = mapCoord;
        }

        public SceneRegister.Scenes sceneID;
        public Vector2 mapCoord;
    }

    [SerializeField]
    private List<MinimapLink> _minimapLocations = new();

    [SerializeField]
    private RectTransform _marker;

    public void UpdateMarker()
    {
        _marker.anchoredPosition = _minimapLocations.Find(x => (int)x.sceneID == GameSaveManager.Instance.currentSave.locationIndex).mapCoord;
    }
}
