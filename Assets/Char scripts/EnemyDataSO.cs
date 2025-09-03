using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Status B�sicos")]
    public float baseHP = 100f;
    public float baseATQ = 10f;
    public float moveSpeed = 3f;
    public float attackSpeed = 1f;

    [Header("Escala por N�vel")]
    public float hpPerLevel = 10f;
    public float atqPerLevel = 2f;
    public float speedPerLevel = 0.5f;

    [Header("Recompensas")]
    public int geoditasOnDeath = 1;
    [Range(0f, 1f)]
    public float etherDropChance = 0.1f;

    // M�todos para calcular status baseado no n�vel
    public float GetHealth(int level) => baseHP + (level * hpPerLevel);
    public float GetDamage(int level) => baseATQ + (level * atqPerLevel);
    public float GetMoveSpeed(int level) => moveSpeed + (level * speedPerLevel);
}