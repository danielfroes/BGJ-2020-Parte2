using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GarbagePile : MonoBehaviour
{
    private List<GarbageTypeComponent> items = new List<GarbageTypeComponent>();
    public List<GarbageItemType> testItems;
    private BoxCollider garbageCollider;
    [SerializeField] private float distanceFromPile;

    private void Start()
    {
        garbageCollider = GetComponent<BoxCollider>();
    }
    // Start is called before the first frame update
    public void AddItem(GarbageItemType item)
    {
        Vector3 position =  RandomPointInBounds(garbageCollider.bounds);
        Quaternion rotation = Quaternion.identity;

        GarbageTypeComponent x = Instantiate(item.prefab, position, rotation, transform).GetComponent<GarbageTypeComponent>();
        items.Add(x);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddItem(testItems[Random.Range(0, testItems.Count)]);
        }else if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject _g;
            if(Remove(testItems[Random.Range(0, testItems.Count)], out _g))
            {
                Destroy(_g);
            }
            
        }
    }

    public bool Remove(GarbageItemType item, out GameObject objRemoved)
    {
        GarbageTypeComponent _item = items.Find((match) => match.type == item);

        if(_item == null)
        {
            objRemoved = null;
            return false;
        }

        objRemoved = _item.gameObject;
        return items.Remove(_item);
    }

    private Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.min.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
