using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class ItemWorld : MonoBehaviour, IInteractable
{
    public string questName;
    [SerializeField] private Item.ItemType _itemType;
    [SerializeField] private int _amount;
    private LocalKeyword _OUTLINE_ON;

    [SerializeField]
    private Color _canInteractColor;

    [SerializeField]
    private Color _cannotInteractColor;

    private void Awake()
    {
        _OUTLINE_ON = new LocalKeyword(
            GetComponent<SpriteRenderer>().material.shader,
            "_OUTLINE_ON"
        );

        if (_itemType != Item.ItemType.None)
            GetComponent<SpriteRenderer>().sprite = ItemRegister.Instance.items
                .Find(x => x.itemType == _itemType)
                .sprite;
    }

    private void Update()
    {
        if (QuestLineManager.Instance.Quests[0].Tasks[0].Title == questName)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CursorObject>().enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CursorObject>().enabled = false;
        }
    }

    ///<summary>
    ///Sets an item type and its amount.
    ///Sets correct sprite
    ///</summary>
    ///<param name="itemType"></param>
    ///<param name="amount"></param>
    public void SetItem(Item.ItemType itemType, int amount)
    {
        _itemType = itemType;
        _amount = amount;
        GetComponent<SpriteRenderer>().sprite = ItemRegister.Instance.items
            .Find(x => x.itemType == _itemType)
            .sprite;
    }

    ///<summary>
    ///Returns an Item from world item
    ///</summary>
    public Item GetItem()
    {
        Item item = ItemRegister.Instance.GetNewItem(_itemType);
        item.amount = _amount;
        return item;
    }

    public void CursorClick()
    {
        if (GameManager.Instance.player != null)
        {
            if (
                GameManager.Instance.player.GetComponent<Player>().GetInventory().AddItem(GetItem())
            )
            {
                //Find item in game save to update its load state
                LevelManagerData.ItemWorldState cmp = new LevelManagerData.ItemWorldState(
                    _itemType, _amount, transform.position, true, questName
                );

                if (GameSaveManager.Instance != null)
                {
                    bool isRemoved = FindObjectOfType<LevelManager>()
                        .GetLevelData()
                        .items.Remove(cmp);

                    //Item is added to inventory
                    if (isRemoved == true)
                    {
                        cmp.load = false;
                        FindObjectOfType<LevelManager>().GetLevelData().items.Add(cmp);

                        if (FMODEvents.Instance != null)
                        {
                            AudioManager.Instance?.PlayOneShot(
                                FMODEvents.Instance.ItemPickup,
                                transform.position
                            );
                        }
                    }
                }

                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogError("Player is not set in the GameManager singleton or is not set!");
        }
    }

    public void CursorEnter(bool canInteract)
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, true);
        GetComponent<SpriteRenderer>().material.SetColor(
            "_Color",
            canInteract ? _canInteractColor : _cannotInteractColor
        );
    }

    public void CursorExit()
    {
        GetComponent<SpriteRenderer>().material.SetKeyword(_OUTLINE_ON, false);
    }
}
