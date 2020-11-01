﻿using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private LayerMask lm = 0;
    [SerializeField] private InventoryControllerSO inventory = null;
    // Struct para mapear quais plantas estão em qual lugar, para salvar o progresso

    [SerializeField] private InventoryItem objSelected = null;
    [SerializeField] private Material objHoverFree = null;
    [SerializeField] private Material objHoverOcc = null;

    private GameObject objHover = null;
    private MeshRenderer objHoverRenderer = null;

    private GridMap grid = null;
    
    void Start()
    {
        grid = FindObjectOfType<GridMap>();
        inventory.OnItemChange += Inventory_OnItemChange;
    }

    private void Inventory_OnItemChange(InventoryItem _obj)
    {
        if(objHover != null)
        {
            Destroy(objHover);
        }

        objSelected = _obj;
        objHover = Instantiate(objSelected.prefab, new Vector3(50, 50, 50), transform.rotation);
        objHoverRenderer = objHover.GetComponentInChildren<MeshRenderer>();
        objHover.GetComponentInChildren<Collider>().enabled = false;
    }

    void Update()
    {
        if (objHover == null || objSelected == null)
            return;

        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo, 2100, lm) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                PutObject(hitInfo);
            }else if (Input.GetMouseButtonDown(1))
            {
                RemoveObject(hitInfo);
            }else if (Input.GetKeyDown(KeyCode.R))
            {
                objHover.transform.Rotate(Vector3.up, 90f);
            }
            else
            {
                HoverObject(hitInfo);
            }
        }
        else
        {
            objHover.SetActive(false);
        }
        
    }

    private void RemoveObject(RaycastHit hitInfo)
    {
        GameObject g;
        if (grid.RemoveObject(hitInfo.point, out g))
        {
            inventory.SellItem(g.GetComponent<InventoryItemComponent>().type);
            Destroy(g);
        }
    }

    private void PutObject(RaycastHit hitInfo)
    {
        if (grid.IsPositionFree(hitInfo.point))
        {
            if (inventory.DecreaseCount())
            {
                grid.PutObjectOngrid(hitInfo.point, objHover.transform.rotation, objSelected.prefab);
                grid.GetObjectAtPosition(hitInfo.point).AddComponent<InventoryItemComponent>().type = objSelected; 
            }
        }
    }

    private void HoverObject(RaycastHit hitInfo)
    {
        if (objHover == null) return;

        objHover.SetActive(true);
        Vector3 finalPos = grid.GetNearestPointOnGrid(hitInfo.point);
        if (grid.IsPositionFree(finalPos))
        {
            objHoverRenderer.material = objHoverFree;
        }
        else
        {
            objHoverRenderer.material = objHoverOcc;
        }

        objHover.transform.position = grid.GetNearestPointOnGrid(hitInfo.point);
    }
}
