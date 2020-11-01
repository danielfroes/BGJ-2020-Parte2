using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayCycleController : MonoBehaviour
{
    [Range(0, 20)]
    [Tooltip("Tempo de um ciclo de dia em minutos")]
    public float CycleTime = 2;

    public UnityEvent OnCycleEnd = new UnityEvent();

    [SerializeField] private GarbageGenerator GarbageGenerator = null;

    private float _initTime = 0f;
    private bool _isRunning = false;

    [ContextMenu("Start Cycle")]
    public void ResetInitTime()
    {
        _initTime = Time.time;
        _isRunning = true;
        GarbageGenerator.enabled = true;
    }

    private void Start()
    {
        ResetInitTime();
    }

    // Update is called once per frame
    void Update()
    {
        if( CheckTime() && _isRunning)
        {
            GarbageGenerator.enabled = false;
            _isRunning = false;
            OnCycleEnd?.Invoke();
            //ResetInitTime();
        }
    }

    private bool CheckTime()
    {
        return (Time.time - _initTime) >= (CycleTime * 60f);
    }
}
