using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Novo Caminho de Upgrade", menuName = "ExoBeasts/Torres/Caminho de Upgrade")]
public class UpgradePath : ScriptableObject
{
    [Tooltip("A lista de upgrades para este caminho, em ordem do N�vel 1 ao N�vel 5.")]
    public List<Upgrade> upgradesInPath;
}