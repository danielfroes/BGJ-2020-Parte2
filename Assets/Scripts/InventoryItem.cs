using UnityEngine;

[CreateAssetMenu]
public class InventoryItem : ScriptableObject
{
    public int count;
    public float price;
    public GameObject prefab;
    public Sprite sprite;
}
