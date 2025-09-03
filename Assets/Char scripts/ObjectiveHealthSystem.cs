using UnityEngine;
using System; // Necess�rio para usar o 'Action'

public class ObjectiveHealthSystem : MonoBehaviour
{
    [Header("Configura��es de Vida")]
    public float maxHealth = 1000f;
    public float currentHealth;

    // Evento para notificar a UI quando a vida mudar (igual ao do jogador)
    public event Action OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        NotifyHealthChanged(); // Garante que a UI comece com o valor correto
    }

    public void TakeDamage(float damage)
    {
        // Se a vida j� for zero, n�o faz nada
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Garante que a vida n�o fique negativa

        Debug.Log("Objetivo sofreu " + damage + " de dano. Vida restante: " + currentHealth);
        NotifyHealthChanged(); // Avisa a UI para se atualizar

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.LogError("GAME OVER! O objetivo foi destru�do!");
        // --- L�GICA DE FIM DE JOGO ---
        // Aqui voc� pode adicionar a l�gica para parar o jogo,
        // mostrar uma tela de derrota, etc.
        // Por enquanto, vamos apenas parar o tempo:
        Time.timeScale = 0; // Pausa o jogo
    }

    private void NotifyHealthChanged()
    {
        // Dispara o evento, se algu�m estiver "ouvindo"
        OnHealthChanged?.Invoke();
    }
}