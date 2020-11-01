using System.Collections;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public InventoryControllerSO Inventory;

    private void Awake()
    {
        Inventory.Setup();
    }

#if UNITY_EDITOR
    [ContextMenu("Get 100 Moneys")]
    public void GetMoney() => Inventory.GetMoney();

    [ContextMenu("BE RICH")]
    public void BeRich() => Inventory.BeRich();

    [ContextMenu("Get Poor")]
    public void GetPoor() => Inventory.GetPoor();
#endif
}