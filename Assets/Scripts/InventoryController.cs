using System;
using System.Collections;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public event Action<InventoryItem> OnItemChange;
    public event Action<InventoryItem, int> OnItemUse;

    public float Money { get; private set; } = 0;
    public int NumItems { get => items.Length; }

    [SerializeField] private InventoryItem[] items = null;

    private int selectedItem = 0;

    private void Awake()
    {
        items = Resources.LoadAll<InventoryItem>("ScriptableObjects");
    }

    public bool DecreaseCount()
    {
        bool worked = false;
        if (items[selectedItem].count == 0)
        {
            worked = BuyItem();
        }
        else 
        {
            items[selectedItem].count--;
            worked = true;
        }

        if(worked)
            OnItemUse?.Invoke(items[selectedItem], selectedItem);
        return worked;
    }

    public void IncreaseCount()
    {
        items[selectedItem].count++;
        OnItemUse?.Invoke(items[selectedItem], selectedItem);
    }

    public bool BuyItem()
    {
        if (items[selectedItem].price > Money)
            return false;

        Money -= items[selectedItem].price;
        return true;
    }

    public void SellItem(InventoryItem item)
    {
        Money += item.price;
    }


    public void SelectItem(int _index)
    {
        selectedItem = _index;
        Debug.Log("Coming "+ _index);
        OnItemChange?.Invoke(items[selectedItem]);
    }

    public InventoryItem GetItem(int i)
    {
        return items[i];
    }
}