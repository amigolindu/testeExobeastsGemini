// Ability.cs
using UnityEngine;

// Este � o nosso "molde". N�o vamos us�-lo diretamente.
public abstract class Ability : ScriptableObject
{
    [Header("Informa��es da Habilidade")]
    public string abilityName = "Nova Habilidade";
    public Sprite icon;
    public float cooldown = 1f;

    // Este � o "bot�o" que o personagem vai apertar.
    // 'abstract' significa que o molde n�o diz o que o bot�o faz,
    // apenas que ele existe. Cada receita espec�fica vai ter que definir isso.
    //
    // Ele retorna 'true' se a habilidade deve entrar em cooldown.
    // Retorna 'false' se o cooldown deve ser ignorado (nosso caso de reset).
    public abstract bool Activate(GameObject quemUsou);
}