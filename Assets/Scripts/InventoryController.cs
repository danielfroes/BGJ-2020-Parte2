﻿using System;
using System.Collections;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public event Action<InventoryItem> OnItemChange;
    public event Action<InventoryItem, int> OnItemUse;
    public event Action OnMoneyUpdate;

    public float Money
    {
        get => moneyAmount;
        private set
        {
            moneyAmount = value;
            OnMoneyUpdate?.Invoke();
        }
    }
    public int NumItems { get => items.Length; }

    [SerializeField] private InventoryItem[] items = null;

    private int selectedItem = 0;
    private float moneyAmount = 0;

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
        Money += item.price * 0.8f;
    }


    public void SelectItem(int _index)
    {
        if (_index < 0 || _index > NumItems)
        {
            Debug.LogError("Selecting Out Of Range");
            return;
        }

        selectedItem = _index;
        OnItemChange?.Invoke(items[selectedItem]);
    }

    public InventoryItem GetItem(int i)
    {
        return items[i];
    }

#if UNITY_EDITOR
    [ContextMenu("Get 100 Moneys")]
    public void GetMoney()
    {
        Money += 100;
    }

    [ContextMenu("BE RICH")]
    public void BeRich()
    {
        Money += 10000000;
    }

    [ContextMenu("Get Poor")]
    public void GetPoor()
    {
        Money = 0;
    }
#endif
}