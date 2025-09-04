using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButtonUI : MonoBehaviour
{
    [Header("Configura��o da UI")]
    [Tooltip("O prefab de um bot�o que ser� usado para cada torre.")]
    public GameObject buildButtonPrefab;
    [Tooltip("O objeto (ex: painel com Grid Layout) que ir� conter os bot�es.")]
    public Transform buttonContainer;

    /// <summary>
    /// Limpa e cria os bot�es de constru��o com base nas torres dispon�veis.
    /// </summary>
    public void CreateBuildButtons(List<CharacterBase> availableTowers)
    {
        // Limpa bot�es antigos para n�o duplicar
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Cria um novo bot�o para cada torre da lista
        foreach (CharacterBase towerData in availableTowers)
        {
            if (towerData == null) continue;

            GameObject buttonGO = Instantiate(buildButtonPrefab, buttonContainer);

            Image buttonImage = buttonGO.GetComponent<Image>();
            Button button = buttonGO.GetComponent<Button>();

            // Define o �cone do bot�o
            if (buttonImage != null && towerData.characterIcon != null)
            {
                buttonImage.sprite = towerData.characterIcon;
            }

            // Define a a��o do clique do bot�o
            if (button != null)
            {
                button.onClick.AddListener(() => {
                    BuildManager.Instance.SelectTowerToBuild(towerData);
                });
            }
        }
    }
}