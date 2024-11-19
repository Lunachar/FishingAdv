using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSkillSystem
{
    private Dictionary<string, float> _skills;

    public void Initialize()
    {
        _skills = new Dictionary<string, float>
        {
            { "Knots", 0 },
            { "BaitMixing", 0 },
            { "CastingAccuracy", 0 }
        };
    }

    public void UpgradeSkill(string skillName, float amount)
    {
        if (_skills.ContainsKey(skillName))
        {
            _skills[skillName] += amount;
        }
    }

    public float GetSkillLevel(string skillName)
    {
        return _skills.ContainsKey(skillName) ? _skills[skillName] : 0;
    }
}
