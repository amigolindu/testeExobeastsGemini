using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    [Header("Refer�ncias")]
    public EnemyDataSO enemyData;

    [Header("Status Atual")]
    public float currentHealth;
    public bool isDead;

    private EnemyController enemyController;

    void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public void InitializeHealth(int level)
    {
        if (enemyData == null)
        {
            Debug.LogError("EnemyData n�o atribu�do em " + gameObject.name);
            return;
        }
        currentHealth = enemyData.GetHealth(level);
        isDead = false;
    }

    // MUDAN�A AQUI: de 'void' para 'bool'
    public bool TakeDamage(float damage)
    {
        if (isDead) return false;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            return true; // AVISA: Sim, o dano matou o inimigo.
        }

        return false; // AVISA: N�o, o inimigo sobreviveu.
    }

    private void Die()
    {
        isDead = true;

        // Adiciona as recompensas ao jogador
        if (CurrencyManager.Instance != null && enemyData != null)
        {
            int geoditesAmount = enemyData.geoditasOnDeath;
            if (geoditesAmount > 0)
            {
                CurrencyManager.Instance.AddCurrency(geoditesAmount, CurrencyType.Geodites);
            }

            if (Random.value <= enemyData.etherDropChance)
            {
                CurrencyManager.Instance.AddCurrency(1, CurrencyType.DarkEther);
            }
        }

        if (enemyController != null)
        {
            enemyController.HandleDeath();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}