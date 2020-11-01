using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryItem[] items = null;

    public event Action<GameObject> OnItemChange;
    private int selectedItem = 0;

    private void Awake()
    {
        items = Resources.LoadAll<InventoryItem>("ScriptableObjects");
    }

    private void DecreaseCount()
    {
        if (items[selectedItem].count == 0)
        {
            BuyItem();
        }
        else 
        {
            items[selectedItem].count--;
        }
    }

    private void BuyItem()
    {
        throw new NotImplementedException();
    }

    private void IncreaseCount()
    {
        items[selectedItem].count++;
    }

    public void SelectItem(int _index)
    {
        selectedItem = _index;
        OnItemChange?.Invoke(items[selectedItem].prefab);
    }

    


}
