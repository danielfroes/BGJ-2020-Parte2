﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private float size = 1f;
    [SerializeField] private Dictionary<Vector3, GameObject> grid;

    void Start()
    {
        grid = new Dictionary<Vector3, GameObject>();
    }
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(xCount * size, yCount * size, zCount * size);

        result += transform.position;

        return result;
    }

    public bool PutObjectOngrid(Vector3 ClickPoint, GameObject g)
    {
        Vector3 finalPos = GetNearestPointOnGrid(ClickPoint);
        
        if (IsPositionFree(finalPos))
        { 
            grid.Add(finalPos, Instantiate(g, finalPos, transform.rotation));
            return true;
        }

        return false;
        
    }

    public bool RemoveObject(Vector3 ClickPoint, out GameObject objectInGrid)
    {
        Vector3 nearestPoint = GetNearestPointOnGrid(ClickPoint);
        
        if(grid.TryGetValue(nearestPoint, out objectInGrid))
        {
            return grid.Remove(nearestPoint);
        }

        return false;
    }

    public bool IsPositionFree(Vector3 position)
    {
        return !grid.ContainsKey(position);
    } 

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for (float x = 0; x < 40; x+= size)
        {
            for (float z = 0; z < 40; z+= size)
            {
                Vector3 point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }*/
}
