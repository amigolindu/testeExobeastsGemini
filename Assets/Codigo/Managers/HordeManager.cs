using UnityEngine;
using System.Collections; // Adicionado para usar Coroutines
using System.Collections.Generic;

public class HordeManager : MonoBehaviour
{
    [Header("Configura��es da Horda")]
    public int enemiesPerHordeMin = 5;
    public int enemiesPerHordeMax = 10;
    public int victoryHorde = 5;

    [Header("Dados dos Inimigos")]
    public EnemyDataSO[] enemyTypes;

    [Header("Configura��o das Rotas")]
    public List<SpawnPath> spawnPaths;
    private int lastPathIndex = -1;

    [Header("Status da Horda")]
    public int currentHorde = 0;
    public int enemyLevel = 1;

    private List<GameObject> aliveEnemies = new List<GameObject>();
    private bool waveIsActive = false;

    // --- ADI��O AQUI ---
    // Vari�vel para guardar a refer�ncia do jogador
    private Transform playerTransform;

    void Start()
    {
        if (EnemyPoolManager.Instance == null)
        {
            Debug.LogError("EnemyPoolManager n�o encontrado na cena!");
            return;
        }

        // --- MUDAN�A AQUI ---
        // Em vez de iniciar a horda diretamente, chamamos uma coroutine
        // que primeiro encontra o jogador.
        StartCoroutine(FindPlayerAndBeginHorde());
    }

    // --- NOVO M�TODO ---
    // Esta rotina garante que vamos procurar o Player DEPOIS que ele foi criado.
    private IEnumerator FindPlayerAndBeginHorde()
    {
        // Espera um �nico frame. Isso d� tempo para o GameSetupManager rodar seu m�todo Start().
        yield return null;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("HordeManager n�o conseguiu encontrar o Player! A IA do inimigo pode falhar.");
        }

        // Agora que temos a refer�ncia (ou falhamos em encontr�-la), come�amos a primeira horda.
        StartNextHorde();
    }

    void Update()
    {
        if (waveIsActive)
        {
            CheckForRemainingEnemies();
            if (aliveEnemies.Count == 0)
            {
                waveIsActive = false;
                Debug.Log("Horda " + currentHorde + " conclu�da!");
                if (currentHorde >= victoryHorde)
                {
                    Debug.Log("Parab�ns! Voc� venceu o jogo!");
                }
                else
                {
                    Invoke("StartNextHorde", 5f);
                }
            }
        }
    }

    void SpawnEnemies()
    {
        if (spawnPaths == null || spawnPaths.Count == 0)
        {
            Debug.LogError("Nenhuma rota (SpawnPath) configurada!");
            return;
        }
        if (enemyTypes.Length == 0)
        {
            Debug.LogError("Faltam tipos de inimigos configurados!");
            return;
        }

        int enemiesToSpawn = Random.Range(enemiesPerHordeMin, enemiesPerHordeMax + 1);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int pathIndex = GetRandomPathIndex();
            SpawnPath selectedPath = spawnPaths[pathIndex];

            if (selectedPath.spawnPoint == null || selectedPath.patrolPoints == null || selectedPath.patrolPoints.Count == 0)
            {
                Debug.LogWarning("A rota '" + selectedPath.pathName + "' n�o est� configurada corretamente. Pulando este spawn.");
                continue;
            }

            int enemyTypeIndex = Random.Range(0, enemyTypes.Length);
            EnemyDataSO enemyData = enemyTypes[enemyTypeIndex];

            // MODIFICADO: Usamos a refer�ncia correta do PoolManager
            GameObject newEnemy = EnemyPoolManager.Instance.GetPooledEnemy();
            newEnemy.transform.position = selectedPath.spawnPoint.position;
            newEnemy.transform.rotation = selectedPath.spawnPoint.rotation;

            EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                // --- MUDAN�A PRINCIPAL AQUI ---
                // Agora, passamos a refer�ncia do Player para o inimigo no momento da inicializa��o.
                enemyController.InitializeEnemy(playerTransform, selectedPath.patrolPoints, enemyData, enemyLevel);
            }

            // O HealthSystem n�o precisa ser configurado aqui se o EnemyController j� faz isso.
            // Removido para evitar redund�ncia.

            aliveEnemies.Add(newEnemy);
        }
    }

    // O resto do seu script continua igual
    void CheckForRemainingEnemies()
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] == null || !aliveEnemies[i].activeInHierarchy)
            {
                aliveEnemies.RemoveAt(i);
            }
        }
    }

    void StartNextHorde()
    {
        currentHorde++;
        Debug.Log("Iniciando Horda " + currentHorde);
        enemyLevel = currentHorde;
        SpawnEnemies();
        waveIsActive = true;
    }

    int GetRandomPathIndex()
    {
        if (spawnPaths.Count <= 1) return 0;
        int newIndex;
        do
        {
            newIndex = Random.Range(0, spawnPaths.Count);
        } while (newIndex == lastPathIndex);
        lastPathIndex = newIndex;
        return newIndex;
    }
}