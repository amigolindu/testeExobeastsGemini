using UnityEngine;

public class DoubleAttackBehavior : TowerBehavior
{
    private float chance = 0.25f; // 25% de chance de atacar novamente

    /// <summary>
    /// Sobrescrevemos o m�todo Initialize para nos "inscrevermos" no evento de ataque da torre.
    /// </summary>
    public override void Initialize(TowerController owner)
    {
        base.Initialize(owner);
        if (towerController != null)
        {
            // Diz para a torre: "Quando voc� atacar, me avise chamando meu m�todo HandleAttack."
            towerController.OnAttack += HandleAttack;
        }
    }

    /// <summary>
    /// Este m�todo � chamado toda vez que a torre ataca.
    /// </summary>
    private void HandleAttack(Transform target)
    {
        // Roda a chance de atacar novamente
        if (Random.value <= chance)
        {
            Debug.Log("CORTE DUPLO ATIVADO!");
            // Pede para a torre atacar o mesmo alvo novamente.
            // towerController.PerformAttack(target); // Descomente quando o m�todo de ataque existir
        }
    }

    /// <summary>
    /// M�todo chamado automaticamente quando o componente � destru�do.
    /// � MUITO importante se "desinscrever" do evento para evitar erros.
    /// </summary>
    private void OnDestroy()
    {
        if (towerController != null)
        {
            towerController.OnAttack -= HandleAttack;
        }
    }
}