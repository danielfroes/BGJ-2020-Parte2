using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory Reference")]
    [SerializeField] private InventoryControllerSO Inventory = null;

    [Header("Serialized Properties")]
    [SerializeField] private GameObject itemBtnPrefab = null;
    [SerializeField] private Transform buttonLayout = null;
    [SerializeField] private TMPro.TMP_Text moneyText = null;
    [SerializeField] private List<ItemBtn> buttons = null;

    [System.Serializable]
    private class ItemBtn
    {
        public TMPro.TMP_Text priceText;
        public TMPro.TMP_Text itemNameText;
    }

    private void Awake()
    {
        buttons = new List<ItemBtn>();
    }

    private void Start()
    {
        for (int i = 0; i < Inventory.NumItems; i++)
        {
            var obj = Instantiate(itemBtnPrefab, buttonLayout);
            obj.name = i.ToString() + "_" + obj.name;
            obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Inventory.SelectItem(int.Parse(obj.name.Split('_')[0])));

            buttons.Add(new ItemBtn {
                itemNameText = obj.transform.Find("itemName").GetComponentInChildren<TMPro.TMP_Text>(),
                priceText = obj.transform.Find("priceText").GetComponentInChildren<TMPro.TMP_Text>()
            });

            var item = Inventory.GetItem(i);
            Inventory_OnItemUse(item, i);
            buttons[i].itemNameText.text = item.itemName;
        }

        Inventory.OnItemUse += Inventory_OnItemUse;
        Inventory.OnMoneyUpdate += Inventory_OnMoneyUpdate;
        Inventory_OnMoneyUpdate();
    }

    private void Inventory_OnMoneyUpdate()
    {
        moneyText.text = $"{Inventory.Money} $";
    }

    private void Inventory_OnItemUse(InventoryItem item, int index)
    {
        buttons[index].priceText.text = item.price.ToString() + "$";
    }
}
