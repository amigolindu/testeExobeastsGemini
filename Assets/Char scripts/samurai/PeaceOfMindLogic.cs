// PeaceOfMindLogic.cs
using UnityEngine;
using System.Collections;

// Este � um componente tempor�rio que existir� no jogador
// apenas enquanto a cura estiver ativa.
public class PeaceOfMindLogic : MonoBehaviour
{
    private PlayerHealthSystem healthSystem;

    // M�todo de inicializa��o que a "receita" vai chamar
    public void StartEffect(float totalHeal, float duration)
    {
        healthSystem = GetComponent<PlayerHealthSystem>();
        if (healthSystem != null)
        {
            StartCoroutine(HealCoroutine(totalHeal, duration));
        }
        else
        {
            // Se n�o encontrar o sistema de vida, se destr�i para n�o ficar "solto"
            Destroy(this);
        }
    }

    private IEnumerator HealCoroutine(float totalHeal, float duration)
    {
        float healPerSecond = totalHeal / duration;
        float timeLeft = duration;

        while (timeLeft > 0)
        {
            healthSystem.Heal(healPerSecond * Time.deltaTime);
            timeLeft -= Time.deltaTime;
            yield return null; // Espera o pr�ximo frame
        }

        // A tarefa acabou. Agora o ajudante se autodestr�i.
        Destroy(this);
    }
}