using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    // Array para os 8 slots: [0] = Comandante, [1-7] = Torres
    public CharacterBase[] equipeSelecionada = new CharacterBase[8];

    private void Awake()
    {
        // L�gica do Singleton: Garante que exista apenas um GameDataManager no jogo.
        if (Instance == null)
        {
            Instance = this;
            // Impede que este objeto seja destru�do ao carregar uma nova cena.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se um j� existir, destr�i este para evitar duplicatas.
            Destroy(gameObject);
        }
    }

    // Limpa a sele��o para garantir que uma nova partida comece do zero.
    public void LimparSelecao()
    {
        for (int i = 0; i < equipeSelecionada.Length; i++)
        {
            equipeSelecionada[i] = null;
        }
    }
}