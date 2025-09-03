using UnityEngine;

public class PauseControl : MonoBehaviour
{
    [Header("Gerenciador de UI")]
    public UIManager uiManager; // Refer�ncia ao UIManager

    private bool isPaused = false;

    // Remove Start para evitar conflito com UIManager.Start()
    // O controle do mouse inicial ser� feito pelo BuildManager ou UIManager
    void Start() { /* Vazio, inicializa��o feita pelo UIManager */ }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (uiManager != null) uiManager.ShowPauseMenu(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPaused = true;
    }

    public void ResumeGame()
    {
        if (uiManager != null) uiManager.ShowPauseMenu(false);

        Time.timeScale = 1f;

        // Retorna o controle do cursor para o BuildManager ou para o estado Locked normal
        // Se estiver no modo de constru��o, o BuildManager vai gerenciar
        if (FindObjectOfType<BuildManager>() != null && FindObjectOfType<BuildManager>().isBuildingMode)
        {
            Cursor.lockState = CursorLockMode.None; // Permanece livre para constru��o
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Volta para o estado travado do jogo
            Cursor.visible = false;
        }

        isPaused = false;
    }
}