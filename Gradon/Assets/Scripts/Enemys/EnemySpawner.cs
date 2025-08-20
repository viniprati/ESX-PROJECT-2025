// ArcadeEnemySpawner.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcadeEnemySpawner : MonoBehaviour
{
    [Header("Progress�o de Inimigos")]
    [Tooltip("Configure a lista de inimigos e quando eles devem come�ar a aparecer.")]
    [SerializeField] private List<EnemyProgression> enemyProgression;

    [Header("Pontos de Spawn")]
    [Tooltip("Se esta lista estiver vazia, o spawner tentar� usar todos os objetos filhos como pontos de spawn.")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Configura��o de Dificuldade")]
    [SerializeField] private float initialSpawnInterval = 2.5f;
    [SerializeField] private float minSpawnInterval = 0.4f;
    [SerializeField] private float timeToIncreaseDifficulty = 15f;
    [SerializeField] private float intervalReduction = 0.1f;
    [SerializeField] private float timeToIncreaseBurstCount = 45f;
    [SerializeField] private int maxEnemiesInBurst = 4;

    private float gameTime = 0f;
    private float currentSpawnInterval;
    private int enemiesPerBurst = 1;
    private List<GameObject> availableEnemies = new List<GameObject>();

    // Awake � chamado antes de Start, ideal para configurar refer�ncias.
    void Awake()
    {
        // --- L�GICA DE PREENCHIMENTO AUTOM�TICO ---

        // Se a lista de spawnPoints n�o foi preenchida manualmente no Inspector...
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.Log("A lista 'Spawn Points' est� vazia. Preenchendo automaticamente com os objetos filhos.");

            // Inicializa a lista para evitar erros.
            spawnPoints = new List<Transform>();

            // Itera sobre cada 'Transform' que � filho direto deste objeto.
            foreach (Transform child in transform)
            {
                // Adiciona o filho � lista.
                spawnPoints.Add(child);
            }
        }
    }

    void Start()
    {
        // A valida��o agora acontece depois da tentativa de preenchimento autom�tico.
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("Nenhum ponto de spawn foi encontrado (nem no Inspector, nem como filho)! Desativando o spawner.");
            this.enabled = false;
            return;
        }

        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        CheckForNewEnemyUnlocks();
        HandleDifficultyScaling();
    }

    private void CheckForNewEnemyUnlocks()
    {
        foreach (var progressionEntry in enemyProgression)
        {
            if (progressionEntry.enemyPrefab != null && gameTime >= progressionEntry.timeToStartSpawning && !availableEnemies.Contains(progressionEntry.enemyPrefab))
            {
                Debug.Log($"<color=cyan>NOVO INIMIGO DESBLOQUEADO: {progressionEntry.enemyPrefab.name}!</color>");
                availableEnemies.Add(progressionEntry.enemyPrefab);
            }
        }
    }

    private void HandleDifficultyScaling()
    {
        int difficultySteps = Mathf.FloorToInt(gameTime / timeToIncreaseDifficulty);
        currentSpawnInterval = initialSpawnInterval - (difficultySteps * intervalReduction);
        currentSpawnInterval = Mathf.Max(currentSpawnInterval, minSpawnInterval);

        int burstSteps = Mathf.FloorToInt(gameTime / timeToIncreaseBurstCount);
        enemiesPerBurst = Mathf.Min(1 + burstSteps, maxEnemiesInBurst);
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);
            if (availableEnemies.Count > 0 && spawnPoints.Count > 0)
            {
                for (int i = 0; i < enemiesPerBurst; i++)
                {
                    SpawnSingleEnemy();
                    if (enemiesPerBurst > 1) yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    private void SpawnSingleEnemy()
    {
        GameObject enemyToSpawn = availableEnemies[Random.Range(0, availableEnemies.Count)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}

// Lembre-se de que a classe EnemyProgression precisa estar definida
// ou neste arquivo (ap�s o final da classe ArcadeEnemySpawner) ou em seu pr�prio arquivo.
/*
[System.Serializable]
public class EnemyProgression
{
    public GameObject enemyPrefab;
    public float timeToStartSpawning;
}
*/