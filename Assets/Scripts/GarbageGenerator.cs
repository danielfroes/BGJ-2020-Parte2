using UnityEngine;

public class GarbageGenerator : MonoBehaviour
{
    [SerializeField] private float garbagePerSecond = 0;
    [SerializeField] private GameObject garbagePilePrefab;
    [SerializeField] private int itemsPerPile;

    private GarbageItemType[] garbageItemTypes = null;
    private float time = 0;

    private void Awake()
    {
       garbageItemTypes = Resources.FindObjectsOfTypeAll<GarbageItemType>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(time >= 1 / garbagePerSecond)
        {
            CreateGarbagePile();
            time = 0;
        }
    }

    private void CreateGarbagePile()
    {
        GarbagePile garbagePile = Instantiate(garbagePilePrefab).GetComponent<GarbagePile>();
        garbagePile.transform.position = transform.position;
        
        for(int i = 0; i < itemsPerPile; i++)
        {
           garbagePile.AddItem(garbageItemTypes[Random.Range(0, garbageItemTypes.Length)]);
        }
    }
}
