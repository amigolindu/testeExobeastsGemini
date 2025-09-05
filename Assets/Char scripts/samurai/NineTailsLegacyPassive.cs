// NineTailsLegacyPassive.cs
using UnityEngine;

[CreateAssetMenu(fileName = "Legado das Nove Caudas", menuName = "ExoBeasts/Passivas/Legado das Nove Caudas")]
public class NineTailsLegacyPassive : PassivaAbility
{
    [Header("Ingredientes da Passiva")]
    public float attackSpeedBonus = 0.15f; // 15%

    public override void OnEquip(GameObject owner)
    {
       // Debug.Log("Passiva equipada: " + abilityName);

       /*  Encontra todas as torres na cena
        //TowerController[] allTowers = FindObjectsOfType<TowerController>();

        foreach (var tower in allTowers)
        {
            // Aplica o b�nus
            // (Isso assume que sua torre tem uma vari�vel 'attackSpeedModifier' ou algo assim)
            tower.ApplyAttackSpeedBonus(attackSpeedBonus);
        }*/
    }

    public override void OnUnequip(GameObject owner)
    {
        Debug.Log("Passiva desequipada: " + abilityName);

        /*TowerController[] allTowers = FindObjectsOfType<TowerController>();
        foreach (var tower in allTowers)
        {
            // Remove o b�nus (importante para n�o acumular)
            tower.RemoveAttackSpeedBonus(attackSpeedBonus);
        }*/
    }
}