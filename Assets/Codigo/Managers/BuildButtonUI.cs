using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButtonUI : MonoBehaviour
{
    [Header("Configuração da UI")]
    [Tooltip("O prefab de um botão que será usado para cada torre.")]
    public GameObject buildButtonPrefab;
    [Tooltip("O objeto (ex: painel com Grid Layout) que irá conter os botões.")]
    public Transform buttonContainer;

    /// <summary>
    /// Limpa e cria os botões de construção com base nas torres disponíveis.
    /// </summary>
    public void CreateBuildButtons(List<CharacterBase> availableTowers)
    {
        // Limpa botões antigos para não duplicar
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Cria um novo botão para cada torre da lista
        foreach (CharacterBase towerData in availableTowers)
        {
            if (towerData == null) continue;

            GameObject buttonGO = Instantiate(buildButtonPrefab, buttonContainer);

            Image buttonImage = buttonGO.GetComponent<Image>();
            Button button = buttonGO.GetComponent<Button>();

            // Define o ícone do botão
            if (buttonImage != null && towerData.characterIcon != null)
            {
                buttonImage.sprite = towerData.characterIcon;
            }

            // Define a ação do clique do botão
            if (button != null)
            {
                button.onClick.AddListener(() => {
                    BuildManager.Instance.SelectTowerToBuild(towerData);
                });
            }
        }
    }
}