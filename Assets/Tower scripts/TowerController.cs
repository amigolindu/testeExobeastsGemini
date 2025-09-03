using UnityEngine;
using System; // Necess�rio para usar 'Action' para criar Eventos

public class TowerController : MonoBehaviour
{
    [Header("Refer�ncias")]
    public CharacterBase towerData;

    // --- EVENTO ---
    /// <summary>
    /// Evento que � disparado toda vez que a torre ataca.
    /// Comportamentos especiais (como Corte Duplo) podem se inscrever neste evento.
    /// </summary>
    public event Action<Transform> OnAttack;

    // --- STATUS ATUAIS ---
    private float currentDamage;
    private float currentAttackSpeed;
    private float currentRange;
    private int[] currentPathLevels;

    void Start()
    {
        if (towerData != null)
        {
            currentDamage = towerData.damage;
            currentAttackSpeed = towerData.attackSpeed;
            currentRange = towerData.meleeRange;
            currentPathLevels = new int[towerData.upgradePaths.Count];
        }
        else
        {
            Debug.LogError("TowerData n�o foi atribu�do neste TowerController!", this.gameObject);
        }
    }

    /// <summary>
    /// M�TODO DE ATAQUE DE EXEMPLO. O script do seu amigo chamaria algo parecido.
    /// </summary>
    public void PerformAttack(Transform target)
    {
        // 1. L�gica de ataque base (criar proj�til, causar dano, etc.)
        Debug.Log("Ataque base no alvo: " + target.name + " com " + currentDamage + " de dano.");

        // 2. Dispara o evento, avisando a todos os "equipamentos" que um ataque ocorreu.
        OnAttack?.Invoke(target);
    }

    public void PurchaseUpgrade(int pathIndex)
    {
        if (!CanPurchaseUpgrade(pathIndex)) return;

        int nextTier = currentPathLevels[pathIndex];
        Upgrade upgradeToPurchase = towerData.upgradePaths[pathIndex].upgradesInPath[nextTier];

        if (!CurrencyManager.Instance.HasEnoughCurrency(upgradeToPurchase.geoditeCost, CurrencyType.Geodites) ||
            !CurrencyManager.Instance.HasEnoughCurrency(upgradeToPurchase.darkEtherCost, CurrencyType.DarkEther))
        {
            Debug.Log("Moeda insuficiente!");
            return;
        }

        CurrencyManager.Instance.SpendCurrency(upgradeToPurchase.geoditeCost, CurrencyType.Geodites);
        CurrencyManager.Instance.SpendCurrency(upgradeToPurchase.darkEtherCost, CurrencyType.DarkEther);

        ApplyUpgrade(upgradeToPurchase);
        currentPathLevels[pathIndex]++;
    }

    private void ApplyUpgrade(Upgrade upgrade)
    {
        // 1. Aplica b�nus de status
        foreach (var modifier in upgrade.modifiers)
        {
            // (Adicione a l�gica do switch/case aqui para cada StatType)
            if (modifier.statToModify == StatType.Damage)
            {
                currentDamage = CalculateNewValue(currentDamage, modifier);
            }
        }

        // 2. Adiciona o novo "equipamento" (comportamento) � torre, se houver um.
        if (upgrade.behaviorToUnlock != null)
        {
            var newBehavior = gameObject.AddComponent(upgrade.behaviorToUnlock.GetType()) as TowerBehavior;
            if (newBehavior != null)
            {
                newBehavior.Initialize(this);
                Debug.Log("Equipamento adicionado: " + newBehavior.GetType().Name);
            }
        }
    }

    private bool CanPurchaseUpgrade(int pathIndex)
    {
        if (pathIndex >= towerData.upgradePaths.Count ||
            currentPathLevels[pathIndex] >= towerData.upgradePaths[pathIndex].upgradesInPath.Count)
        {
            return false;
        }
        return true;
    }

    private float CalculateNewValue(float baseValue, StatModifier modifier)
    {
        if (modifier.modType == ModificationType.Additive)
            return baseValue + modifier.value;
        else // Multiplicative
            return baseValue * (1 + modifier.value);
    }
}