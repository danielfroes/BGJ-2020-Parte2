using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfDayUI : MonoBehaviour
{
    [SerializeField] private InventoryControllerSO Inventory = null;
    [SerializeField] private TMPro.TMP_Text endDayText = null;


    public void UpdateEndOfDayText()
    {
        float nWorkers = FindObjectsOfType<Worker>().Length;
        endDayText.text = $"Fim do dia, o salário será dividido entre os sócios:\n- Dinheiro restante: {Inventory.Money.ToString("{0:0.00}")} $\n-Número de sócios: {nWorkers}\n- Salário por sócio: {(Inventory.Money / nWorkers).ToString("{0:0.00}")} $";
    }
}
