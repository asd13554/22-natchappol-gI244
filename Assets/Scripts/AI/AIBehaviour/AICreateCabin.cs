using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreateCabin : AICreateHQ
{
    void Start()
    {
        support = gameObject.GetComponent<AISupport>();

        buildingPrefab = support.Faction.BuildingPrefabs[5];
        buildingGhostPrefab = support.Faction.GhostBuildingPrefabs[5];
    }
    private bool CheckIfAnyUnfinishedHouseAndBarrack()
    {
        foreach (GameObject houseObj in support.Houses)
        {
            Building h = houseObj.GetComponent<Building>();

            if (!h.IsFunctional && (h.CurHP < h.MaxHP)) //This house is not yet finished
                return true;
        }

        foreach (GameObject barrackObj in support.Barracks)
        {
            Building b = barrackObj.GetComponent<Building>();

            if (!b.IsFunctional && (b.CurHP < b.MaxHP)) //This barrack is not yet finished
                return true;
        }
        
        foreach (GameObject hospitalObj in support.Hospitals)
        {
            Building ho = hospitalObj.GetComponent<Building>();

            if (!ho.IsFunctional && (ho.CurHP < ho.MaxHP)) //This hospital is not yet finished
                return true;
        }
        
        foreach (GameObject cabinObj in support.Cabins)
        {
            Building c = cabinObj.GetComponent<Building>();

            if (!c.IsFunctional && (c.CurHP < c.MaxHP)) //This cabin is not yet finished
                return true;
        }
        return false;
    }

    
    public override float GetWeight()
    {
        Building b = buildingPrefab.GetComponent<Building>();

        if (!support.Faction.CheckBuildingCost(b)) //Don't have enough resource to build a barrack
            return 0;

        if (CheckIfAnyUnfinishedHouseAndBarrack()) //Check if there is any unfinished house or barrack
            return 0;

        if (support.Cabins.Count < 1 && support.Hospitals.Count > 0) // If there are less than 1 cabin and there are some houses
            return 2;

        return 0;
    }
}
