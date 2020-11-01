using System.ComponentModel;
using UnityEngine;

public class GarbageGenerator : MonoBehaviour
{
    [SerializeField] private float garbagePerSecond = 1;
    [SerializeField] private GameObject garbagePilePrefab = null;
    [SerializeField] private int itemsPerPile = 5;
    [SerializeField] private GridMap grid = null;

    [SerializeField] private GarbageItemType[] garbageItemTypes = null;
    private float time = 0;

    private void Awake()
    {
       if(garbageItemTypes.Length == 0)
            garbageItemTypes = Resources.FindObjectsOfTypeAll<GarbageItemType>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(time >= 1 / garbagePerSecond)
        {
            Vector3 finalPosition = transform.position + transform.forward * grid.size;
            finalPosition.y = 0;//to find the conveyors
            if (!grid.IsPositionFree(finalPosition))
            {
                CreateGarbagePile(finalPosition, itemsPerPile, 0.6f);
                time = 0;
            }
        }
    }

    private void CreateGarbagePile(Vector3 finalPosition)
    {
        GarbagePile garbagePile = Instantiate(garbagePilePrefab).GetComponent<GarbagePile>();
        garbagePile.transform.position = transform.position;

        AddItems(itemsPerPile, garbagePile);

        garbagePile.StartCoroutine(nameof(garbagePile.MoveToNext), finalPosition);
    }

    private void CreateGarbagePile(Vector3 finalPosition, int numItems, float percBadItems)
    {
        GarbagePile garbagePile = Instantiate(garbagePilePrefab).GetComponent<GarbagePile>();
        garbagePile.transform.position = transform.position;

        AddGoodItems(Mathf.RoundToInt((1f - percBadItems) * numItems), garbagePile);
        AddBadItems(Mathf.RoundToInt(percBadItems * numItems), garbagePile);

        //Debug.Log($"Bad Items Percent: {percBadItems} NumItems: {numItems} Good Items: {Mathf.RoundToInt((1 - percBadItems) * numItems)} Bad Items: {Mathf.RoundToInt(percBadItems * numItems)}");

        garbagePile.StartCoroutine(nameof(garbagePile.MoveToNext), finalPosition);
    }


    private void AddGoodItems(int numItems, GarbagePile garbagePile)
    {
        for (int i = 0; i < numItems; i++)
        {
            garbagePile.AddItem(garbageItemTypes[Random.Range(0, garbageItemTypes.Length - 1)]);
        }
    }

    private void AddBadItems(int numItems, GarbagePile garbagePile)
    {
        for (int i = 0; i < numItems; i++)
        {
            garbagePile.AddItem(garbageItemTypes[garbageItemTypes.Length-1]);
        }
    }

    private void AddItems(int numItems, GarbagePile garbagePile)
    {
        for (int i = 0; i < itemsPerPile; i++)
        {
            garbagePile.AddItem(garbageItemTypes[Random.Range(0, garbageItemTypes.Length)]);
        }
    }
}
