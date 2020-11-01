using UnityEngine;

public class GarbageGenerator : MonoBehaviour
{
    [SerializeField] private float garbagePerSecond = 1;
    [SerializeField] private GameObject garbagePilePrefab = null;
    [SerializeField] private int itemsPerPile = 5;
    [SerializeField] private GridMap grid = null;

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
            Vector3 finalPosition = transform.position + transform.forward * grid.size;
            finalPosition.y = 0;//to find the conveyors
            Debug.Log(finalPosition);
            if (!grid.IsPositionFree(finalPosition))
            {
                CreateGarbagePile(finalPosition);
                time = 0;
            }
        }
    }

    private void CreateGarbagePile(Vector3 finalPosition)
    {
        GarbagePile garbagePile = Instantiate(garbagePilePrefab).GetComponent<GarbagePile>();
        garbagePile.transform.position = transform.position;
        
        for(int i = 0; i < itemsPerPile; i++)
        {
           garbagePile.AddItem(garbageItemTypes[Random.Range(0, garbageItemTypes.Length)]);
        }

        garbagePile.StartCoroutine(nameof(garbagePile.MoveToNext), finalPosition);
    }
}
