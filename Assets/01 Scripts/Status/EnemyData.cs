using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "GameData/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("기본 정보")]
    public string enemyName = "적 이름";
    public int tier = 1;

    [Header("능력치")]
    public float maxHealth = 30f;
    public float damage = 5f;
    public float moveSpeed = 2f;

    [TextArea]
    public string description = "적 설명";
}

public enum EnemyType
{
    Goblin,
    Orc,
    Troll,
    Skeleton,
    Zombie
}
