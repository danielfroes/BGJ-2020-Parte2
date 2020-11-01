﻿using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private LayerMask lm = 0;
    // Struct para mapear quais plantas estão em qual lugar, para salvar o progresso

    [SerializeField] private GameObject objSelectedPrefab = null;
    [SerializeField] private GameObject objHover = null;
    [SerializeField] private MeshRenderer objHoverRenderer = null;
    [SerializeField] private Material objHoverFree = null;
    [SerializeField] private Material objHoverOcc = null;
    [SerializeField] private InventoryController inventory = null;

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

        objSelectedPrefab = _obj.prefab;
        objHover = Instantiate(objSelectedPrefab, new Vector3(50, 50, 50), transform.rotation);
        objHoverRenderer = objHover.GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (objHover == null || objSelectedPrefab == null)
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
            Destroy(g);
        }
    }

    private void PutObject(RaycastHit hitInfo)
    {
        if (inventory.DecreaseCount())
        {
            grid.PutObjectOngrid(hitInfo.point, objHover.transform.rotation, objSelectedPrefab);
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
