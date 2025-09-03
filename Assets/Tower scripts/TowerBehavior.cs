using UnityEngine;

/// <summary>
/// Classe base para todos os comportamentos especiais que uma torre pode ganhar via upgrades.
/// 'abstract' significa que este script n�o pode ser usado diretamente, apenas herdado.
/// </summary>
public abstract class TowerBehavior : MonoBehaviour
{
    // Uma refer�ncia protegida para a torre que possui este comportamento.
    protected TowerController towerController;

    /// <summary>
    /// M�todo de inicializa��o chamado pelo TowerController quando este comportamento � adicionado.
    /// </summary>
    /// <param name="owner">A inst�ncia do TowerController que est� recebendo este comportamento.</param>
    public virtual void Initialize(TowerController owner)
    {
        this.towerController = owner;
    }
}