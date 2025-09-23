using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatusSO : ScriptableObject
{
    [Header("기본 능력치")]
    public float currentHealth;
    public float maxHealth;

    public float moveSpeed;
}
