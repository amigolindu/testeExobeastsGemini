using UnityEngine;
using TMPro;

// Enum para definir os tipos de moeda de forma clara e segura.
public enum CurrencyType
{
    Geodites,
    DarkEther
}

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [Header("Refer�ncias da UI")]
    public TextMeshProUGUI geoditesText;
    public TextMeshProUGUI darkEtherText;

    [Header("Valores Iniciais")]
    [SerializeField] private int initialGeodites = 500;
    [SerializeField] private int initialDarkEther = 0;

    // Propriedades p�blicas para acessar os valores, mas com set privado
    public int CurrentGeodites { get; private set; }
    public int CurrentDarkEther { get; private set; }

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

    private void Start()
    {
        CurrentGeodites = initialGeodites;
        CurrentDarkEther = initialDarkEther;
        UpdateUI();
    }

    // M�todo universal para adicionar qualquer tipo de moeda
    public void AddCurrency(int amount, CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Geodites:
                CurrentGeodites += amount;
                break;
            case CurrencyType.DarkEther:
                CurrentDarkEther += amount;
                break;
        }
        UpdateUI();
    }

    // M�todo universal para verificar se h� saldo suficiente
    public bool HasEnoughCurrency(int amount, CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Geodites:
                return CurrentGeodites >= amount;
            case CurrencyType.DarkEther:
                return CurrentDarkEther >= amount;
            default:
                return false;
        }
    }

    // M�todo universal para gastar moeda
    public void SpendCurrency(int amount, CurrencyType type)
    {
        if (HasEnoughCurrency(amount, type))
        {
            switch (type)
            {
                case CurrencyType.Geodites:
                    CurrentGeodites -= amount;
                    break;
                case CurrencyType.DarkEther:
                    CurrentDarkEther -= amount;
                    break;
            }
            UpdateUI();
        }
    }

    // Atualiza ambos os textos da UI
    private void UpdateUI()
    {
        if (geoditesText != null)
        {
            geoditesText.text = $"Geoditas: {CurrentGeodites}";
        }

        if (darkEtherText != null)
        {
            darkEtherText.text = $"�ter Negro: {CurrentDarkEther}";
        }
    }
}