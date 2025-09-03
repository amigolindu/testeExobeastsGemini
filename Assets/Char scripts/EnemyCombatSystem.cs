using UnityEngine;

public class EnemyCombatSystem : MonoBehaviour
{
    [Header("Configura��es de Combate")]
    public float attackRange = 2f;
    // MODIFICADO: A linha abaixo foi removida, pois agora o cooldown vem do Scriptable Object.
    // public float attackCooldown = 1f;

    [Header("Refer�ncias")]
    public Transform attackPoint;
    public LayerMask playerLayer;

    // Componentes
    private EnemyController enemyController;
    private EnemyDataSO enemyData;
    private Transform player;

    // Estado
    private bool canAttack = true;
    private bool isAttacking = false;
    private float currentAttackCooldown;

    void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        if (enemyController != null)
        {
            enemyData = enemyController.enemyData;
        }
    }

    void Update()
    {
        if (enemyController == null || enemyController.IsDead) return;

        if (!canAttack)
        {
            currentAttackCooldown -= Time.deltaTime;
            if (currentAttackCooldown <= 0)
            {
                canAttack = true;
            }
        }
    }

    public void TryAttack()
    {
        if (canAttack && IsPlayerInAttackRange())
        {
            StartAttack();
        }
    }

    bool IsPlayerInAttackRange()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= attackRange;
    }

    void StartAttack()
    {
        isAttacking = true;
        canAttack = false;

        // MODIFICADO: O cooldown agora � calculado com base no 'attackSpeed' do EnemyDataSO.
        if (enemyData != null && enemyData.attackSpeed > 0)
        {
            // A f�rmula � 1 / ataques_por_segundo. Ex: 2 de attackSpeed = 0.5s de cooldown.
            currentAttackCooldown = 1f / enemyData.attackSpeed;
        }
        else
        {
            // Valor padr�o para evitar divis�o por zero caso o valor n�o esteja configurado.
            currentAttackCooldown = 1f;
        }

        // Inicia a anima��o de ataque (se tiver)
        // animator.SetTrigger("Attack");

        // Aplica o dano (ajuste o timing conforme sua anima��o)
        Invoke("ApplyDamage", 0.5f); // Ajuste este tempo para coincidir com a anima��o
    }

    void ApplyDamage()
    {
        if (!isAttacking) return;

        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        foreach (Collider playerCollider in hitPlayers)
        {
            PlayerHealthSystem playerHealth = playerCollider.GetComponent<PlayerHealthSystem>();
            if (playerHealth != null)
            {
                // Este c�lculo j� estava correto, usando o Scriptable Object.
                float finalDamage = enemyData.baseATQ + (enemyController.nivel * enemyData.atqPerLevel);

                playerHealth.TakeDamage(finalDamage);
                Debug.Log("Inimigo causou " + finalDamage + " de dano ao jogador");
            }
        }

        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}