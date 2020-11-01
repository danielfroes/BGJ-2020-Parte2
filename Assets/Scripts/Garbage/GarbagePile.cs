using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GarbagePile : MonoBehaviour
{
    private List<GarbageTypeComponent> items = new List<GarbageTypeComponent>();
    public List<GarbageItemType> testItems = null;
    public LayerMask conveyorLM = 0;

    private BoxCollider garbageCollider = null;
    private Vector3 moveDirection = Vector3.zero;
    private float moveSpeed = 1f;

    private void Awake()
    {
        garbageCollider = GetComponent<BoxCollider>();
        conveyorLM = LayerMask.GetMask("Conveyor");

    }
    // Start is called before the first frame update
    public void AddItem(GarbageItemType item)
    {
        Vector3 position =  RandomPointInBounds(garbageCollider.bounds);
        Quaternion rotation = Quaternion.identity;
        
        GarbageTypeComponent x = Instantiate(item.prefab, position, rotation, transform).GetComponent<GarbageTypeComponent>();
        x.transform.position = position + transform.position;
        x.transform.localRotation = rotation;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ConveyorBelt"))
        {
            moveDirection = collision.transform.forward;
            moveSpeed = collision.gameObject.GetComponent<ConveyorBelt>().Speed;
        }
    }

    public void CheckMovement()
    {
        if (!IsOnConveyor()) return;

        Vector3 finalPoint = GridMap.Static_GetNearestPointOnGrid(transform.position + moveDirection * GridMap.BaseGridSize);

        StartCoroutine(nameof(MoveToNext), finalPoint);
    }

    private bool IsOnConveyor()
    {
        return Physics.Raycast(transform.position, Vector3.down, 2100, conveyorLM);
    }

    public IEnumerator MoveToNext(Vector3 finalPoint)
    {
        Vector2 myPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 finalPosition = new Vector2(finalPoint.x, finalPoint.z);
        float travelDistance = (myPosition - finalPosition).magnitude;

        float travelFraction = 0f;
        // Lerp ateh o meio do belt
        while (myPosition != finalPosition)
        {
            travelFraction += (moveSpeed * Time.deltaTime) / travelDistance;

            myPosition = Vector2.Lerp(myPosition, finalPosition, travelFraction);

            transform.position = new Vector3(myPosition.x, transform.position.y, myPosition.y);
            yield return null;
        }
        // CheckMovement
        CheckMovement();
    }
}
