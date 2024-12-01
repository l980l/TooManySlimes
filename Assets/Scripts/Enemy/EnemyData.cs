using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Normal,
    Ranger,
    Boss,
    Max
}

[CreateAssetMenu(menuName = "ScriptableObject/EnemyData")] 
public class EnemyData : ScriptableObject
{
    public EnemyType Type;  // 타입
    public int FullHP;      // 최대체력
    public int AtkVal;         // 공격력
}
