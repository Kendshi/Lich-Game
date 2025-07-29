using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] List<Transform> positionPoints = new List<Transform>();

    private List<Unit> _units = new List<Unit>();
    private const int MAX_UNITS_IN_SQUAD = 9;

    void Start()
    {
        foreach (Unit unit in _units) 
        {
            Instantiate(unit);
        }
    }

    void Update()
    {
        
    }

    public void GetSpawnUnits(List<Unit> units)
    {
        if (units.Count > MAX_UNITS_IN_SQUAD)
        {
            Debug.LogError("Количество юнитов превышает максимальный лимит отряда");
            return;
        }

        _units = units;
    }
}
