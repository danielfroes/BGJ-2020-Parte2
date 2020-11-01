using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    //processa um tipo específico de item
    [SerializeField] private InventoryControllerSO inventory = null;
    [SerializeField] private List<GarbageItemType> itemsToSell = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GarbagePile"))
        {
            GarbagePile gp = other.gameObject.GetComponent<GarbagePile>();

            int failCounter = 0;
            float finalPrice = 0;

            //verifica quantos items não podem ser processador por ela
            foreach (GarbageTypeComponent gtc in gp.items)
            {
                if (!itemsToSell.Contains(gtc.type))
                {
                    failCounter++;
                }
                else
                {
                    finalPrice += gtc.type.sellingPrice;
                }
            }
            if(gp.items.Count != 0)
            {
                float multiplier = 1f - (failCounter / gp.items.Count);
                inventory.SellPile(finalPrice * multiplier);
            }
            gp.StopAllCoroutines();
            Destroy(gp.gameObject);
        }
    }
}
