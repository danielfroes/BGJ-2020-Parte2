using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    [SerializeField] private LayerMask lm;
    // Struct para mapear quais plantas estão em qual lugar, para salvar o progresso
    private struct plantInfo
    {
        private GameObject plant;
        private int x;
        private int z;
    }

    [SerializeField] GameObject plant = null;

    private Grid grid = null;
    //private GameObject[,] plantsPos = new GameObject[50, 50];

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        //Dar load nas posicoes das plantas
        // plantsPos = LoadPlantsPos
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hitInfo, 2100, lm))
            {
                if (hitInfo.transform.CompareTag("Ground"))
                {
                    grid.PutObjectOngrid(hitInfo.point, plant);
                }
            }
        }
    }

}
