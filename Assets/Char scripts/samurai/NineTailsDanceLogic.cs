// NineTailsDanceLogic.cs
using UnityEngine;
using System.Collections;

// Componente tempor�rio que gerencia o estado de f�ria da ultimate.
public class NineTailsDanceLogic : MonoBehaviour
{
    // Refer�ncias para os sistemas que vamos manipular
    private PlayerCombatManager combatManager;
    private PlayerShooting shootingSystem;
    private MeleeCombatSystem meleeSystem;

    // M�todo de inicializa��o chamado pela "receita"
    public void StartEffect(float duration)
    {
        // Pega as refer�ncias dos componentes no jogador
        combatManager = GetComponent<PlayerCombatManager>();
        shootingSystem = GetComponent<PlayerShooting>();
        meleeSystem = GetComponent<MeleeCombatSystem>();

        // Garante que todas as refer�ncias foram encontradas antes de iniciar
        if (combatManager != null && shootingSystem != null && meleeSystem != null)
        {
            StartCoroutine(UltimateCoroutine(duration));
        }
        else
        {
            Debug.LogError("N�o foi poss�vel encontrar todos os componentes necess�rios para a Ultimate. Abortando.");
            Destroy(this); // Se autodestr�i se algo estiver faltando
        }
    }

    private IEnumerator UltimateCoroutine(float duration)
    {
        // --- IN�CIO DA ULTIMATE ---
        Debug.Log("ULTIMATE ATIVADA: Dan�a das Nove Caudas!");

        // 1. Desativa o gerenciador de combate para assumirmos o controle manual
        combatManager.enabled = false;

        // 2. Troca para o modo de combate corpo a corpo
        shootingSystem.enabled = false;
        meleeSystem.enabled = true;

        // 3. Concede imunidade (se voc� tiver esse sistema)
        // GetComponent<StatusController>().isImmune = true;

        // 4. Espera a dura��o da ultimate acabar
        yield return new WaitForSeconds(duration);

        // --- FIM DA ULTIMATE ---
        Debug.Log("Ultimate finalizada.");

        // 5. Reverte a imunidade
        // GetComponent<StatusController>().isImmune = false;

        // 6. Reativa o gerenciador de combate para ele reassumir o controle
        // O gerenciador vai automaticamente colocar o modo de combate correto
        // baseado no CharacterBase do personagem.
        combatManager.enabled = true;

        // 7. O ajudante cumpriu sua miss�o. Ele se autodestr�i.
        Destroy(this);
    }
}