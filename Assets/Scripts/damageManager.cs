using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Infantry,
    Archer,
    Special
}


public class damageManager : MonoBehaviour
{
    public List<float> damageValues;
    public List<float> healthValues;

    void Start()
    {
        damageValues = new List<float>
        {
            1.0f, // Infantry
            0.8f, // Archer
            1.5f, // Special
        };

        healthValues = new List<float>
        {
            4f, // Infantry
            3f, // Archer
            8f, // Special
        };
    }

    public float GetDamageForUnit(UnitType unitType)
    {
        int index = (int)unitType;

        if (index >= 0 && index < damageValues.Count)
        {
            return damageValues[index];
        }
        else
        {
            Debug.LogWarning("Invalid unit type, returning 0 damage");
            return 0f;
        }
    }

    public float GetHealthForUnit(UnitType unitType)
    {
        int index = (int)unitType;

        if (index >= 0 && index < healthValues.Count)
        {
            return healthValues[index];
        }
        else
        {
            Debug.LogWarning("Invalid unit type, returning 0 damage");
            return 0f;
        }
    }
}
