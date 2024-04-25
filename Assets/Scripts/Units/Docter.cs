using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Docter : MonoBehaviour
{
    [SerializeField] private Unit inProgressHealing; // The building a unit is currently building
    public Unit InProgressHealing { get { return inProgressHealing; } set { inProgressHealing = value; } }
    
    private float timer = 0f; //Heal timer
    public float Timer { get { return timer; } set { timer = value; } }
    private float waitTime = 0.5f; //How fast it will be Heal, higher is longer
    public float WaitTime { get { return waitTime; } set { waitTime = value; } }
    
    private Unit unit;
    
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (unit.State == UnitState.Die)
            return;

        // switch (unit.State)
        // {
        //     case UnitState.MoveToHeal :
        //         MoveToHeal(inProgressHealing);
        //         break;
        //     case UnitState.HealProgress:
        //         HealProgress();
        //         break;
        // }
    }
    
    
    private void StartHealing(Unit healingObj)
    {
        DocterStartHealing(healingObj);
        unit.SetState(UnitState.HealProgress);
    }
    
    public void DocterStartHealing(Unit target)
    {
        inProgressHealing = target;        
        unit.SetState(UnitState.MoveToHeal);
        
    }
    
    private void MoveToHeal(Unit b)
    {
        if (b == null)
            return;

        unit.NavAgent.SetDestination(b.transform.position);
        unit.NavAgent.isStopped = true;
        
        HealProgress();
    }
    
    private void HealProgress()
    {
        if (inProgressHealing == null)
            return;

        StartHealing(inProgressHealing);
        unit.LookAt(inProgressHealing.transform.position);
        Unit b = inProgressHealing.GetComponent<Unit>();
        
        Timer += Time.deltaTime;
        if (Timer >= WaitTime)
        {
            Timer = 0;
            b.CurHP++;
            
            if (b.CurHP >= b.MaxHP) //finish
            {
                b.CurHP = b.MaxHP;

                inProgressHealing = null; //Clear this job off his mind
                unit.SetState(UnitState.Idle);
                return;
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (unit.State == UnitState.Die)
            return;

        if (unit != null)
        {
            if (other.gameObject == inProgressHealing)
            {
                unit.NavAgent.isStopped = true;
                unit.SetState(UnitState.HealProgress);
            }
        }
    }
}
