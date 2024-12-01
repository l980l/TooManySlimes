using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Gun,
    bounceGun,
    MagneticShield,
    Max
}

[CreateAssetMenu(menuName = "ScriptableObject/SkillData")] 
public class SkillData : ScriptableObject
{
    public SkillType skillType;
    public int skillID;
    public string skillName;

    [TextArea]
    public string skillDescription;
    public Sprite Icon;

    public float Damage;
    public int baseCount;
    public float baseCoolTime;
    public int[] counts;
    public float[] coolTime;
}
