using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessorMachine : MonoBehaviour
{

    //processa um tipo específico de item
    [SerializeField] private List<GarbageItemType> itemsToProcess = null;
    [SerializeField] private float deathRate = 0.5f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private GridMap grid = null;

    private Animator animator = null;

    private void Start()
    {
        grid = FindObjectOfType<GridMap>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("GarbagePile"))
        {
            GarbagePile gp = other.gameObject.GetComponent<GarbagePile>();

            int failCounter = 0;

            //verifica quantos items não podem ser processador por ela
            foreach (GarbageTypeComponent gtc in gp.items)
            {
                if (!itemsToProcess.Contains(gtc.type))
                {
                    failCounter++;
                }
            }

            // Animation
            animator.SetTrigger("AtivarPistao");

            gp.Process();
            //Destroy(collision.gameObject);

            TakeDamage(failCounter * deathRate);

           // Debug.Log($"Número de itens incorretos: {failCounter}");
            // caso os items que chegaram não sejam do tipo que ela processa, a sua durabilidade diminui
        }
    }

    private void TakeDamage(float dmg)
    {        
        health = Mathf.Clamp(health - dmg, 0, maxHealth);

        if(health == 0)
        {
            GameObject _g;
            grid.RemoveObject(transform.position, out _g);
            if (grid != null)
            {
                Destroy(_g);
            }
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void Heal()
    {
        health = maxHealth;
    }




}
