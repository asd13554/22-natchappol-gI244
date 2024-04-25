using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AISupport : MonoBehaviour
{
    [SerializeField] private List<GameObject> fighters = new List<GameObject>(); //fighter
    public List<GameObject> Fighters { get { return fighters; } }

    [SerializeField] private List<GameObject> builders = new List<GameObject>(); //builder
    public List<GameObject> Builders { get { return builders; } }

    [SerializeField] private List<GameObject> workers = new List<GameObject>(); //worker
    public List<GameObject> Workers { get { return workers; } }
    
    [SerializeField] private List<GameObject> dogters = new List<GameObject>(); //dogter
    public List<GameObject> Dogters { get { return dogters; } }
    
    [SerializeField] private List<GameObject> scouts = new List<GameObject>(); //scout
    public List<GameObject> Scouts { get { return scouts; } }

    [SerializeField] private Faction faction;
    public Faction Faction { get { return faction; } }

    [SerializeField] private List<GameObject> hq = new List<GameObject>();
    public List<GameObject> HQ { get { return hq; } }
    
    [SerializeField] private List<GameObject> houses = new List<GameObject>();
    public List<GameObject> Houses { get { return houses; } }
    
    [SerializeField] private List<GameObject> barracks = new List<GameObject>();
    public List<GameObject> Barracks { get { return barracks; } }
    
    [SerializeField] private List<GameObject> hospitals = new List<GameObject>();
    public List<GameObject> Hospitals { get { return hospitals; } }
    
    [SerializeField] private List<GameObject> cabins = new List<GameObject>();
    public List<GameObject> Cabins { get { return cabins; } }

    // Start is called before the first frame update
    void Awake()
    {
        faction = GetComponent<Faction>();
    }
    public void Refresh()
    {
        fighters.Clear();
        workers.Clear();
        builders.Clear();
        dogters.Clear();
        scouts.Clear();

        foreach (Unit u in faction.AliveUnits)
        {
            if (u.gameObject == null)
                continue;
            
            if (u.IsBuilder) //if it is a builder
                builders.Add(u.gameObject);
            
            if (u.IsWorker) //if it is a worker
                workers.Add(u.gameObject);
            
            if (u.IsDocter) //if it is a dogter
                dogters.Add(u.gameObject);
            
            if (u.IsScout) //if it is a scout
                scouts.Add(u.gameObject);
            
            if (!u.IsBuilder && !u.IsWorker) //if it is a fighter
                fighters.Add(u.gameObject);
            
        }
        
        hq.Clear();
        houses.Clear();
        barracks.Clear();
        hospitals.Clear();
        cabins.Clear();

        foreach (Building b in faction.AliveBuildings)
        {
            if (b == null)
                continue;
            
            if (b.IsHQ)
                hq.Add(b.gameObject);
            
            if (b.IsHousing)
                houses.Add(b.gameObject);
            
            if (b.IsBarrack)
                barracks.Add(b.gameObject);

            if (b.IsHospital)
                hospitals.Add(b.gameObject);
            
            if (b.IsCabin)
                cabins.Add(b.gameObject);
        }
    }
    
}
