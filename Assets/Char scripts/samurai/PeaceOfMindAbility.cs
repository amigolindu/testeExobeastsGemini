// PeaceOfMindAbility.cs
using UnityEngine;

[CreateAssetMenu(fileName = "Paz de Esp�rito", menuName = "ExoBeasts/Habilidades/Paz de Esp�rito")]
public class PeaceOfMindAbility : Ability
{
    [Header("Ingredientes da Cura")]
    public float totalHeal = 80f;
    public float duration = 3f;

    public override bool Activate(GameObject quemUsou)
    {
        // 1. Cria o "ajudante" da l�gica e o adiciona ao personagem
        PeaceOfMindLogic ajudante = quemUsou.AddComponent<PeaceOfMindLogic>();

        // 2. Passa os "ingredientes" para o ajudante e manda ele come�ar a trabalhar
        ajudante.StartEffect(totalHeal, duration);

        // A habilidade ainda entra em cooldown normalmente.
        return true;
    }
}