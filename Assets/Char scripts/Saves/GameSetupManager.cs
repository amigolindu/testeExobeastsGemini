using UnityEngine;

public class GameSetupManager : MonoBehaviour
{
    [Header("Configura��o de Spawn")]
    public Transform spawnPoint; // Onde o comandante vai aparecer

    void Start()
    {
        // Garante que a l�gica rode no in�cio da partida
        SetupGame();
    }

    private void SetupGame()
    {
        // 1. VERIFICA SE O GAME DATA MANAGER EXISTE
        if (GameDataManager.Instance == null)
        {
            Debug.LogError("GameDataManager n�o encontrado! A cena foi iniciada diretamente?");
            return;
        }

        // 2. CRIA O COMANDANTE
        CharacterBase commanderData = GameDataManager.Instance.equipeSelecionada[0];
        if (commanderData != null && commanderData.commanderPrefab != null && spawnPoint != null)
        {
            Instantiate(commanderData.commanderPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        // 3. AQUI EST� A CONEX�O QUE VOC� PERGUNTOU
        // Ele encontra o BuildManager na cena e chama o m�todo para entregar a lista de torres.
        if (BuildManager.Instance != null)
        {
            // Pega a lista do GameDataManager e a "salva" dentro do BuildManager para esta partida.
            BuildManager.Instance.SetAvailableTowers(GameDataManager.Instance.equipeSelecionada);
        }
        else
        {
            Debug.LogError("BuildManager n�o foi encontrado na cena para configurar as torres!");
        }
    }
}