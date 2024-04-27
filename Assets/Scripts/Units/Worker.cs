using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [SerializeField]
    private ResourceSource curResourceSource;
    public ResourceSource CurResourceSource { get { return curResourceSource; } set { curResourceSource = value; } }

    [SerializeField]
    private float gatherRate = 0.5f;

    [SerializeField]
    private int gatherAmount = 1; // An amount unit can gather every "gatherRate" second(s)

    [SerializeField]
    private int amountCarry; //amount currently carried
    public int AmountCarry { get { return amountCarry; } set { amountCarry = value; } }
    
    [SerializeField]
    private int maxCarry = 25; //max amount to carry
    public int MaxCarry { get { return maxCarry; } set { maxCarry = value; } }

    [SerializeField]
    private ResourceType carryType;
    public ResourceType CarryType { get { return carryType; } set { carryType = value; } }
    
    private Collider Col;
    private float lastGatherTime;
    private Unit unit;
    
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
        Col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (unit.State)
        {
            case UnitState.MoveToResource:
                MoveToResourceUpdate();
                break;
            case UnitState.Gather:
                GatherUpdate();
                break;
            case UnitState.DeliverToHQ:
                DeliverToHQUpdate();
                break;
            case UnitState.StoreAtHQ:
                StoreAtHQUpdate();
                break;
            case UnitState.Invisible:
                Invisible();
                break;
            
        }
    }
    
    public void ToGatherResource(ResourceSource resource, Vector3 pos)
    {
        curResourceSource = resource;

        //if gather a new type of resource, reset amount to 0
        if (curResourceSource.RsrcType != carryType)
        {
            carryType = CurResourceSource.RsrcType;
            amountCarry = 0;
        }

        unit.SetState(UnitState.MoveToResource);

        unit.NavAgent.isStopped = false;
        unit.NavAgent.SetDestination(pos);
    }
    
    public void ToHide(ResourceSource resource, Vector3 pos)
    {
        curResourceSource = resource;
        
        unit.SetState(UnitState.MoveToResource);

        unit.NavAgent.isStopped = false;
        unit.NavAgent.SetDestination(pos);
    }
    
    private void MoveToResourceUpdate()
    {
        CheckForResource();
        
        if (unit == unit.IsScout)
            MoveToHideUpdate();
        else
        {
            if (Vector3.Distance(transform.position, unit.NavAgent.destination) <= 2f)
            {
                if (curResourceSource != null)
                {
                    unit.LookAt(curResourceSource.transform.position);
                    unit.NavAgent.isStopped = true;
                    unit.SetState(UnitState.Gather);
                }
            }
        }
    }
    
    private void MoveToHideUpdate()
    {
        if (Vector3.Distance(transform.position, unit.NavAgent.destination) <= 2f)
        {
            if (curResourceSource != null)
            {
                unit.LookAt(curResourceSource.transform.position);
                unit.NavAgent.isStopped = true;
                HidingUpdate();
            }
        }
    }
    
    private void HidingUpdate()
    {
        //Unit range = GetComponent<Unit>();
        Col.enabled = !Col.enabled;
        unit.SetState(UnitState.Invisible);
    }
    
    // private void UnitRangeUP(float scout, List<Unit> units)
    // {
    //     foreach (Unit u in units)
    //     {
    //         scout = 150;
    //         u.GetVisualRange(scout);
    //     }
    // }
    
    private void Invisible()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Col.enabled = !Col.enabled;
            
            unit.SetState(UnitState.Idle);
            curResourceSource = null; //Clear this job off his mind
        }
    }
    
    
    private void GatherUpdate()
    {
        if (Time.time - lastGatherTime > gatherRate)
        {
            lastGatherTime = Time.time;

            if (amountCarry < maxCarry)
            {
                if (curResourceSource != null)
                {
                    curResourceSource.GatherResource(gatherAmount);

                    carryType = curResourceSource.RsrcType;
                    amountCarry += gatherAmount;
                }
                else
                    CheckForResource();
            }
            else //amount is full, go back to deliver at HQ
                unit.SetState(UnitState.DeliverToHQ);
        }
    }

    private void DeliverToHQUpdate()
    {
        if (Time.time - unit.LastPathUpdateTime > unit.PathUpdateRate)
        {
            unit.LastPathUpdateTime = Time.time;

            unit.NavAgent.SetDestination(unit.Faction.GetHQSpawnPos());
            unit.NavAgent.isStopped = false;
        }
    }
    
    private void StoreAtHQUpdate()
    {
        unit.LookAt(unit.Faction.GetHQSpawnPos());

        if (amountCarry > 0)
        {
            // Deliver the resource to Faction
            unit.Faction.GainResource(carryType, amountCarry);
            amountCarry = 0;

            //Debug.Log("Delivered");
        }
        CheckForResource();
    }
    
    private void CheckForResource()
    {
        if (curResourceSource != null) //that resource still exists
            ToGatherResource(curResourceSource, curResourceSource.transform.position);
        else
        {
            //try to find a new resource
            curResourceSource = unit.Faction.GetClosestResource(transform.position, carryType);

            //CheckAgain, if found a new one, go to it
            if (curResourceSource != null)
                ToGatherResource(curResourceSource, curResourceSource.transform.position);
            else //can't find a new one
            {
                Debug.Log($"{unit.name} can't find a new tree");
                unit.SetState(UnitState.Idle);
            }
        }
    }

}
