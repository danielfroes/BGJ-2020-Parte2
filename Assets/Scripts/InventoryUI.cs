using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryController Inventory = null;

    [Header("Serialized Properties")]
    [SerializeField] private GameObject itemBtnPrefab = null;
    [SerializeField] private Transform buttonLayout = null;
    [SerializeField] private List<ItemBtn> buttons = null;

    [System.Serializable]
    private class ItemBtn
    {
        public Image image;
        public TMPro.TMP_Text quantityText;
        public TMPro.TMP_Text priceText;
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
                image = obj.transform.Find("image").GetComponent<Image>(),
                quantityText = obj.transform.Find("quantityText").GetComponentInChildren<TMPro.TMP_Text>(),
                priceText = obj.transform.Find("priceText").GetComponentInChildren<TMPro.TMP_Text>()
            });

            var item = Inventory.GetItem(i);
            buttons[i].image.sprite = item.sprite;
            Inventory_OnItemUse(item, i);
        }

        Inventory.OnItemUse += Inventory_OnItemUse;
    }

    private void Inventory_OnItemUse(InventoryItem item, int index)
    {
        buttons[index].priceText.text = item.price.ToString() + "$";
        buttons[index].quantityText.text = item.count.ToString() + "x";
    }
}
