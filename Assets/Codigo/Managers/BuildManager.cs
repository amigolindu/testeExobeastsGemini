using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class BuildManager : MonoBehaviour
{
    [Header("C�meras")]
    public CinemachineCamera buildCamera;

    [Header("Lista de Constru��es")]
    public List<GameObject> buildablePrefabs;
    private int selectedPrefabIndex = -1;

    [Header("Visual do Ghost")]
    private GameObject currentBuildGhost;
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;

    public bool isBuildingMode = false;

    private const int PriorityBuild = 20;
    private const int PriorityInactive = 0;


    void Start()
    {
        buildCamera.Priority.Value = PriorityInactive;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        if (state)
        {
            buildCamera.Priority.Value = PriorityBuild;
        }
        else
        {
            buildCamera.Priority.Value = PriorityInactive;
            ClearSelection();
        }

        UIManager.Instance.ShowBuildUI(state);
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }

    void HandleBuildGhost()
    {
        if (selectedPrefabIndex == -1)
        {
            if (currentBuildGhost != null) Destroy(currentBuildGhost);
            return;
        }

        GameObject selectedPrefab = buildablePrefabs[selectedPrefabIndex];

        if (currentBuildGhost == null)
        {
            currentBuildGhost = Instantiate(selectedPrefab);
            // Desativar o collider para evitar que ele bloqueie o raycast
            Collider ghostCollider = currentBuildGhost.GetComponent<Collider>();
            if (ghostCollider != null) ghostCollider.enabled = false;

            // Desativar o script de GridPlacement para evitar que ele execute a l�gica no ghost
            GridPlacement ghostGrid = currentBuildGhost.GetComponent<GridPlacement>();
            if (ghostGrid != null) ghostGrid.enabled = false;
        }

        MeshRenderer ghostRenderer = currentBuildGhost.GetComponent<MeshRenderer>();
        if (ghostRenderer == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool isOverValidSurface = false; // Vari�vel para controlar se o raycast acertou uma superf�cie v�lida

        // VERIFICA��O PRINCIPAL: Checa se o raycast acertou algo
        if (Physics.Raycast(ray, out hit))
        {
            // Se acertou um objeto com a tag "Local"
            if (hit.transform.CompareTag("Local"))
            {
                isOverValidSurface = true;

                // Posiciona o ghost no local exato do hit
                GridPlacement gridPlacer = selectedPrefab.GetComponent<GridPlacement>();
                Vector3 placementPosition = hit.point;

                // Se o prefab tiver um componente GridPlacement, ajuste a posi��o
                if (gridPlacer != null)
                {
                    // Este passo foi movido para o GridPlacement em si, garantindo que
                    // ele use a lgica de grid corretamente.
                    // O script GridPlacement original precisa de uma pequena correo para isso.
                    // Vou te dar essa correo tambm.
                    placementPosition = hit.point;
                }

                // Ajusta a altura do ghost
                float objectHeight = 0;
                if (selectedPrefab.GetComponent<GridPlacement>() != null)
                {
                    objectHeight = selectedPrefab.GetComponent<GridPlacement>().GetObjectHeight();
                }
                else
                {
                    objectHeight = selectedPrefab.GetComponent<MeshRenderer>().bounds.size.y;
                }

                currentBuildGhost.transform.position = new Vector3(
                    placementPosition.x,
                    hit.point.y + (objectHeight / 2f),
                    placementPosition.z
                );

            }
            else
            {
                // Se o raycast acertou outra coisa que n�o seja "Local"
                isOverValidSurface = false;
                // Move o ghost para a posi��o de hit, mas a cor ficar� vermelha
                currentBuildGhost.transform.position = hit.point;
            }
        }
        else
        {
            // Se o raycast n�o acertou nada (est� no c�u, por exemplo)
            isOverValidSurface = false;
        }

        // L�gica para mudar a cor do ghost
        int buildingCost = selectedPrefab.GetComponent<GridPlacement>().cost;
        bool hasEnoughCurrency = CurrencyManager.Instance.HasEnoughCurrency(buildingCost, CurrencyType.Geodites);

        // O ghost s� ser� verde se estiver sobre uma superf�cie v�lida E o jogador tiver dinheiro
        if (isOverValidSurface && hasEnoughCurrency)
        {
            ghostRenderer.material = validPlacementMaterial;
        }
        else
        {
            ghostRenderer.material = invalidPlacementMaterial;
        }
    }

    void PlaceBuilding()
    {
        if (selectedPrefabIndex == -1 || currentBuildGhost == null) return;

        GameObject selectedPrefab = buildablePrefabs[selectedPrefabIndex];
        int buildingCost = selectedPrefab.GetComponent<GridPlacement>().cost;

        // Obt�m a posi��o atual do ghost
        Vector3 ghostPosition = currentBuildGhost.transform.position;

        // Verifica se a cor do ghost � v�lida, indicando que a coloca��o � permitida
        // Esta � a verifica��o crucial. Se o material for verde, a coloca��o � v�lida.
        if (currentBuildGhost.GetComponent<MeshRenderer>().material.name.Contains(validPlacementMaterial.name))
        {
            if (CurrencyManager.Instance.HasEnoughCurrency(buildingCost, CurrencyType.Geodites))
            {
                // Instancia o objeto real na posi��o do ghost
                Instantiate(selectedPrefab, ghostPosition, Quaternion.identity);

                // Gasta a moeda
                CurrencyManager.Instance.SpendCurrency(buildingCost, CurrencyType.Geodites);

                // Limpa a seleo, removendo o ghost
                ClearSelection();
            }
            else
            {
                Debug.Log("N�o h� Geoditas suficientes!");
            }
        }
    }

    public void SelectBuilding(int prefabIndex)
    {
        ClearSelection();

        if (prefabIndex >= 0 && prefabIndex < buildablePrefabs.Count)
        {
            selectedPrefabIndex = prefabIndex;
        }
    }

    void ClearSelection()
    {
        if (currentBuildGhost != null)
        {
            Destroy(currentBuildGhost);
        }
        currentBuildGhost = null;
        selectedPrefabIndex = -1;
    }
}