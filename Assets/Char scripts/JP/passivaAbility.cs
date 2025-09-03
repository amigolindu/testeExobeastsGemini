// PassiveAbility.cs
using UnityEngine;

// Um molde para habilidades que n�o s�o ativadas, mas aplicam um efeito.
public abstract class PassivaAbility : ScriptableObject
{
    public string abilityName;
    [TextArea] public string description;
    public Sprite icon;

    // M�todo chamado quando a passiva � equipada/inicia o jogo
    public abstract void OnEquip(GameObject owner);

    // M�todo chamado quando a passiva � desequipada/termina o jogo
    public abstract void OnUnequip(GameObject owner);
}