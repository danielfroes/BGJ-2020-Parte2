using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfDayUI : MonoBehaviour
{
    [SerializeField] private InventoryControllerSO Inventory = null;
    [SerializeField] private TMPro.TMP_Text endDayText = null;
    [SerializeField] private GameObject HUD = null;
    [SerializeField] private DayCycleController dayController = null;


    public void UpdateEndOfDayText()
    {
        HUD.SetActive(false);
        float nWorkers = FindObjectsOfType<Worker>().Length;

        if (nWorkers == 0)
        {
            endDayText.text = $"Fim do dia, você é o único sócio. Os lucros de {Inventory.Money} são seus.";
        }
        else
        {
            endDayText.text = $"Fim do dia, o salário será dividido entre os sócios:\n- Dinheiro restante: {Inventory.Money} $\n-Número de sócios: {nWorkers}\n- Salário por sócio: {(Inventory.Money / nWorkers).ToString("0.00")} $";
        }
    }

    public void StartNextDay()
    {
        Inventory.ResetMoney();
        HUD.SetActive(true);
        dayController.ResetInitTime();
        gameObject.SetActive(false);
    }
}
