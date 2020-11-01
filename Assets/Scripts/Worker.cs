using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public float myPrecision = 0.5f;
    [SerializeField] private Work workData;
    public GameObject garbagePilePrefab;
    public Vector3 offset;
    public GridMap grid;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("GarbagePile"))
        {
            GarbagePile gp = other.gameObject.GetComponent<GarbagePile>();
            
            if (workData.jobType == Work.workType.CleanConveyor)
            {
                CleanConveyor(gp);
            }
            else
            {
                SeparateItem(gp);
            }
        }
    }

    private void CleanConveyor(GarbagePile gp)
    {
        for(int i = gp.items.Count - 1; i >=0; i--)
        {
            if (gp.items[i].type == workData.referTo)
            {
                float cleanChance = Random.Range(0f, 1f);
                GameObject objRemoved;
                
                if (cleanChance <= myPrecision)
                {
                    gp.Remove(gp.items[i].type, out objRemoved);

                    if(objRemoved != null)
                        Destroy(objRemoved);

                }
            }
        }
    }

    private void SeparateItem(GarbagePile gp)
    {
        List<GarbageTypeComponent> separatedItems = new List<GarbageTypeComponent>();
        
        for (int i = gp.items.Count - 1; i >= 0; i--)
        {
            if (gp.items[i].type == workData.referTo)
            {
                
                float separateChance = Random.Range(0f, 1f);
                GameObject objRemoved;

                if (separateChance <= myPrecision)
                {

                    gp.Remove(gp.items[i].type, out objRemoved);
                    separatedItems.Add(objRemoved.GetComponent<GarbageTypeComponent>());
                }
            
            }
        }
        
        AddObjectsInNextConveyor(separatedItems, gp);
    }

    private void AddObjectsInNextConveyor(List<GarbageTypeComponent> items, GarbagePile gp)
    {

        GarbagePile garbagePile = Instantiate(garbagePilePrefab).GetComponent<GarbagePile>();

        garbagePile.transform.position = transform.position + offset;

        garbagePile.NewList(items);
        
        Vector3 pos = GridMap.Static_GetNearestPointOnGrid(transform.position - transform.forward * GridMap.BaseGridSize);

        //Debug.Log(pos + " " + transform.position + " " + transform.forward * GridMap.BaseGridSize + " " + (transform.position - transform.forward * GridMap.BaseGridSize));
        garbagePile.StopAllCoroutines();
        StartCoroutine(garbagePile.MoveToNext(pos));

    }



}
