using System;
using UnityEngine;

[CreateAssetMenu]
public class InventoryControllerSO : ScriptableObject
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
    [SerializeField] private int StarterMoney = 100;

    private int selectedItem = 0;
    private float moneyAmount = 0;

    public void Setup()
    {
        items = Resources.LoadAll<InventoryItem>("ScriptableObjects");
        Money = StarterMoney;
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

    public void SellPile(float price)
    {
        Money += price;
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

    public void ResetMoney()
    {
        Money = 0;
    }

#if UNITY_EDITOR
    public void GetMoney()
    {
        Money += 100;
    }
    
    public void BeRich()
    {
        Money += 10000000;
    }
    
    public void GetPoor()
    {
        Money = 0;
    }
#endif
}