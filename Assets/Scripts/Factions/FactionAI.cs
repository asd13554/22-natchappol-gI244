using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FactionAI : MonoBehaviour
{
    public float checkRate = 1.0f;

    private Faction faction;
    private AISupport support = null;
    
    [SerializeField] private Building curHQ;
    [SerializeField] private Building curBarrack;
    [SerializeField] private Building curHospital;
    [SerializeField] private Building curCabin;
    
    //[SerializeField] private GameObject unfinishedBuilding = null;

    [SerializeField] private Unit specificBuilder; //a builder for fixing any unfinished/broken building
    
    void Awake()
    {
        faction = GetComponent<Faction>();
    }

    
    // Start is called before the first frame update
    void Start()
    {
        support = gameObject.GetComponent<AISupport>();
        InvokeRepeating("Check", 0.0f, checkRate);
    }
    
    private void Check()
    {
        if (faction.AliveBuildings.Count == 0) // if all buildings are destroyed, return
            SceneManager.LoadScene(4);

        //Create Workers
        if (curHQ != null)
        {
            if (support.Workers.Count + curHQ.CheckNumInRecruitList(0) < 6) // if there are less than 5 units, keep recruiting Workers
            {
                // if we can recruit a new worker/builder, do so
                if (faction.CheckUnitCost(0))
                    curHQ.ToCreateUnit(0); //HQ recruits a primary worker/builder
            }
        }
        else
        {
            
            Debug.Log("End Game Here (FactionAI.cs 51)");
            
        }

        //Create main fighters
        if (curBarrack != null)
        {
            if ((support.Fighters.Count < 5))// if there are less than 5 fighters
            {
                if (faction.CheckUnitCost(1))
                    curBarrack.ToCreateUnit(0); // recruits main fighter
            }
        }
        
        //Create main docter
        if (curHospital != null)
        {
            if ((support.Dogters.Count < 2))// if there are less than 2 dogter
            {
                if (faction.CheckUnitCost(2))
                    curHospital.ToCreateUnit(0); // recruits dogter
            }
        }
        
        //Create main scout
        if (curCabin != null)
        {
            if ((support.Scouts.Count < 2))// if there are less than 2 scout
            {
                if (faction.CheckUnitCost(3))
                    curCabin.ToCreateUnit(0); // recruits scout
            }
        }
        
        UpdateImportantBuilding();
        WorkerFindResource(ResourceType.Wood,3);
        WorkerFindResource(ResourceType.Food,2);
        WorkerFindResource(ResourceType.Stone,3);
        WorkerFindResource(ResourceType.Gold,2);
    }

    //****************************************************************
    private void UpdateImportantBuilding()
    {
        foreach (Building b in faction.AliveBuildings)
        {
            if (!b.IsFunctional)
                continue;

            if (b.IsHQ)
                curHQ = b;

            if (b.IsBarrack)
                curBarrack = b;
            
            if (b.IsHospital)
                curHospital = b;
            
            if (b.IsCabin)
                curCabin = b;
        }
    }
    
    private void WorkerFindResource(ResourceType rType, int n)
    {
        foreach (GameObject workerObj in support.Workers)
        {
            Unit u = workerObj.GetComponent<Unit>();

            if (u.State == UnitState.Idle) //he's idle
            {
                ResourceSource r = faction.GetClosestResource(u.transform.position, rType);

                if (r == null)
                    continue;

                u.Worker.ToGatherResource(r, r.transform.position);
                n--;
            }
            else if (u.Worker.CurResourceSource != null) //he's has a job
            {
                if (u.Worker.CurResourceSource.RsrcType == rType) //he is already gathering this kind of resource
                    n--;
            }

            if (n == 0)
                break;
        }
    }
    
}
