using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreateHospital : AICreateHQ
{
    void Start()
    {
        support = gameObject.GetComponent<AISupport>();

        buildingPrefab = support.Faction.BuildingPrefabs[4];
        buildingGhostPrefab = support.Faction.GhostBuildingPrefabs[4];
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
            Building b = hospitalObj.GetComponent<Building>();

            if (!b.IsFunctional && (b.CurHP < b.MaxHP)) //This hospital is not yet finished
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

        if (support.Hospitals.Count < 1 && support.Barracks.Count > 1) // If there are less than 1 hospital and there are some houses
            return 2;

        return 0;
    }
}
