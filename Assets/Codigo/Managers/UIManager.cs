using UnityEngine;
using System.Collections.Generic; // Adicionado para usar List<>

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Pain�is de UI")]
    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject buildPanel;

    [Header("Refer�ncias da UI de Constru��o")]
    [Tooltip("Arraste o objeto da cena que cont�m o script BuildButtonUI.")]
    public BuildButtonUI buildButtonUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ShowHUD();
    }

    // --- NOVO M�TODO ---
    // Pede ao BuildButtonUI para criar os bot�es com base na lista de torres.
    public void UpdateBuildUI(List<CharacterBase> availableTowers)
    {
        if (buildButtonUI != null)
        {
            buildButtonUI.CreateBuildButtons(availableTowers);
        }
        else
        {
            Debug.LogWarning("A refer�ncia para o BuildButtonUI n�o foi definida no UIManager!");
        }
    }

    // O restante do seu script original continua aqui sem altera��es...
    public void ShowHUD()
    {
        if (hudPanel != null) hudPanel.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (buildPanel != null) buildPanel.SetActive(false);
    }

    public void ShowPauseMenu(bool show)
    {
        if (pausePanel != null) pausePanel.SetActive(show);

        if (show)
        {
            if (hudPanel != null) hudPanel.SetActive(false);
        }
        else
        {
            if (FindObjectOfType<BuildManager>() != null && FindObjectOfType<BuildManager>().isBuildingMode)
            {
                ShowBuildUI(true);
            }
            else
            {
                ShowHUD();
            }
        }
    }

    public void ShowBuildUI(bool show)
    {
        if (buildPanel != null) buildPanel.SetActive(show);

        if (show)
        {
            if (hudPanel != null) hudPanel.SetActive(false);
        }
        else
        {
            ShowHUD();
        }
    }
}