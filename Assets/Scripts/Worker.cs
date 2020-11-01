using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public float myPrecision;
    [SerializeField] private Work workData;

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
                float cleanChance = Random.Range(0, 1);
                GameObject objRemoved;
                
                if (cleanChance <= myPrecision)
                {
                    gp.Remove(gp.items[i].type, out objRemoved);

                    if(objRemoved != null)
                        Destroy(objRemoved);
                    Debug.Log("limpei um lixo");
                }
            }
        }
    }

    private void SeparateItem(GarbagePile gp)
    {
        foreach (GarbageTypeComponent gtc in gp.items)
        {
            if (gtc.type == workData.referTo)
            {
                float separateChance = Random.Range(0, 1);
                GameObject objRemoved;

                if (separateChance >= myPrecision)
                {
                    gp.Remove(gtc.type, out objRemoved);

                    AddObjectInNextConveyor(objRemoved);
                }
            }
        }
    }

    private void AddObjectInNextConveyor(GameObject obj)
    {
        
    }



}
