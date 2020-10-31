using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private LayerMask lm;
    // Struct para mapear quais plantas estão em qual lugar, para salvar o progresso

    public struct ObjectInfo
    {
        public GameObject obj;
        public GameObject objHover;
        public Material objHoverFree;
        public Material objHoverOcc;
    }

    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject objHover;
    [SerializeField] private Material objHoverFree;
    [SerializeField] private Material objHoverOcc;

    private Grid grid = null;
    //private GameObject[,] plantsPos = new GameObject[50, 50];
    
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        objHover = Instantiate(obj, new Vector3(50, 50, 50), transform.rotation);
    }

    void Update()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo, 2100, lm))
        {
            if (Input.GetMouseButtonDown(0))
            {
                PutObject(hitInfo);
            }else if (Input.GetMouseButtonDown(1))
            {
                RemoveObject(hitInfo);
            }
            else
            {
                HoverObject(hitInfo);
            }
        }
        
    }

    private void RemoveObject(RaycastHit hitInfo)
    {
        GameObject g;
        if (grid.RemoveObject(hitInfo.point, out g))
        {
            Debug.Log(g.name);
            Destroy(g);
        }
    }

    private void PutObject(RaycastHit hitInfo)
    {
        grid.PutObjectOngrid(hitInfo.point, obj);
    }

    private void HoverObject(RaycastHit hitInfo)
    {
        Vector3 finalPos = grid.GetNearestPointOnGrid(hitInfo.point);
        if (grid.IsPositionFree(finalPos))
        {
            objHover.GetComponent<MeshRenderer>().material = objHoverFree;
        }
        else
        {
            objHover.GetComponent<MeshRenderer>().material = objHoverOcc;
        }

        objHover.transform.position = grid.GetNearestPointOnGrid(hitInfo.point);
    }
}
