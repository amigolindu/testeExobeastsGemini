using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance; // Adicionado para fácil acesso (Singleton)

    [Header("Câmeras")]
    public CinemachineCamera buildCamera;

    // --- MUDANÇA: A lista de torres agora é dinâmica ---
    private List<CharacterBase> availableTowers = new List<CharacterBase>();
    private CharacterBase selectedTowerData; // Guarda o ScriptableObject da torre selecionada

    [Header("Visual do Ghost")]
    private GameObject currentBuildGhost;
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;

    public bool isBuildingMode = false;

    private const int PriorityBuild = 20;
    private const int PriorityInactive = 0;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        buildCamera.Priority.Value = PriorityInactive;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --- NOVO MÉTODO: Chamado pelo GameSetupManager para carregar as torres ---
    public void SetAvailableTowers(CharacterBase[] selectedTeam)
    {
        availableTowers.Clear();
        for (int i = 1; i < selectedTeam.Length; i++) // Começa em 1 para pular o comandante
        {
            if (selectedTeam[i] != null)
            {
                availableTowers.Add(selectedTeam[i]);
            }
        }
        Debug.Log(availableTowers.Count + " torres disponíveis para construção.");

        // Avisa o UIManager para criar os botões com esta lista
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateBuildUI(availableTowers);
        }
    }

    // --- NOVO MÉTODO: A UI chamará este para selecionar uma torre ---
    public void SelectTowerToBuild(CharacterBase towerData)
    {
        ClearSelection(); // Limpa o ghost anterior
        selectedTowerData = towerData;
        Debug.Log("Torre selecionada para construir: " + towerData.name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuildingMode = !isBuildingMode;
            ToggleBuildMode(isBuildingMode);
        }

        if (isBuildingMode)
        {
            HandleBuildGhost();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }
        }
    }

    void ToggleBuildMode(bool state)
    {
        buildCamera.Priority.Value = state ? PriorityBuild : PriorityInactive;
        if (!state)
        {
            ClearSelection();
        }
        UIManager.Instance.ShowBuildUI(state);
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }

    void HandleBuildGhost()
    {
        if (selectedTowerData == null)
        {
            if (currentBuildGhost != null) Destroy(currentBuildGhost);
            return;
        }

        GameObject selectedPrefab = selectedTowerData.commanderPrefab;
        if (selectedPrefab == null) return;

        if (currentBuildGhost == null)
        {
            currentBuildGhost = Instantiate(selectedPrefab);
            Collider ghostCollider = currentBuildGhost.GetComponent<Collider>();
            if (ghostCollider != null) ghostCollider.enabled = false;
        }

        // Sua lógica de Raycast, posicionamento e cor do material.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool isOverValidSurface = Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Local");

        currentBuildGhost.transform.position = hit.point;

        int buildingCost = selectedTowerData.cost;
        bool hasEnoughCurrency = CurrencyManager.Instance.HasEnoughCurrency(buildingCost, CurrencyType.Geodites);

        var ghostRenderer = currentBuildGhost.GetComponentInChildren<MeshRenderer>();
        if (ghostRenderer != null)
        {
            ghostRenderer.material = (isOverValidSurface && hasEnoughCurrency) ? validPlacementMaterial : invalidPlacementMaterial;
        }
    }

    void PlaceBuilding()
    {
        if (selectedTowerData == null || currentBuildGhost == null) return;

        GameObject selectedPrefab = selectedTowerData.commanderPrefab;
        int buildingCost = selectedTowerData.cost;

        var ghostRenderer = currentBuildGhost.GetComponentInChildren<MeshRenderer>();
        if (ghostRenderer != null && ghostRenderer.material.name.Contains(validPlacementMaterial.name))
        {
            if (CurrencyManager.Instance.HasEnoughCurrency(buildingCost, CurrencyType.Geodites))
            {
                Instantiate(selectedPrefab, currentBuildGhost.transform.position, Quaternion.identity);
                CurrencyManager.Instance.SpendCurrency(buildingCost, CurrencyType.Geodites);
                ClearSelection();
            }
            else
            {
                Debug.Log("Não há Geoditas suficientes!");
            }
        }
    }

    void ClearSelection()
    {
        if (currentBuildGhost != null)
        {
            Destroy(currentBuildGhost);
        }
        currentBuildGhost = null;
        selectedTowerData = null;
    }
}