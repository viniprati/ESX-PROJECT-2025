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
    [Tooltip("Crie objetos vazios e arraste-os para c�.")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Configura��o de Dificuldade")]
    [Tooltip("Tempo inicial entre spawns de inimigos.")]
    [SerializeField] private float initialSpawnInterval = 2.5f;
    [Tooltip("O tempo m�nimo entre spawns. O jogo n�o ficar� mais r�pido que isso.")]
    [SerializeField] private float minSpawnInterval = 0.4f;

    [Tooltip("A cada quantos segundos a dificuldade aumenta (spawn fica mais r�pido).")]
    [SerializeField] private float timeToIncreaseDifficulty = 15f;
    [Tooltip("Quanto o tempo de spawn diminui a cada aumento de dificuldade.")]
    [SerializeField] private float intervalReduction = 0.1f;

    [Tooltip("A cada quantos segundos o spawner tenta adicionar mais um inimigo por vez.")]
    [SerializeField] private float timeToIncreaseBurstCount = 45f;
    [SerializeField] private int maxEnemiesInBurst = 4;


    // --- Vari�veis Internas ---
    private float gameTime = 0f;
    private float currentSpawnInterval;
    private float difficultyTimer = 0f;
    private float burstTimer = 0f;
    private int enemiesPerBurst = 1;

    // Lista de inimigos que j� foram "desbloqueados" pelo tempo
    private List<GameObject> availableEnemies = new List<GameObject>();

    void Start()
    {
        // Valida��o inicial
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("A lista 'Spawn Points' est� vazia! Desativando o spawner.");
            this.enabled = false;
            return;
        }

        currentSpawnInterval = initialSpawnInterval;

        // Inicia a rotina principal de spawn que roda para sempre
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        // Mant�m o controle do tempo de jogo
        gameTime += Time.deltaTime;

        // Verifica se novos inimigos devem ser desbloqueados
        CheckForNewEnemyUnlocks();

        // Aumenta a dificuldade com o tempo
        HandleDifficultyScaling();
    }

    private void CheckForNewEnemyUnlocks()
    {
        // Itera pela lista de progress�o para ver se algum inimigo novo pode ser adicionado
        foreach (var progressionEntry in enemyProgression)
        {
            // Se o tempo de jogo ultrapassou o tempo de spawn e o inimigo ainda n�o est� na lista dispon�vel...
            if (gameTime >= progressionEntry.timeToStartSpawning && !availableEnemies.Contains(progressionEntry.enemyPrefab))
            {
                Debug.Log($"<color=cyan>NOVO INIMIGO DESBLOQUEADO: {progressionEntry.enemyPrefab.name}!</color>");
                availableEnemies.Add(progressionEntry.enemyPrefab);
            }
        }
    }

    private void HandleDifficultyScaling()
    {
        // --- Escala de Velocidade de Spawn ---
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= timeToIncreaseDifficulty)
        {
            difficultyTimer = 0f; // Reseta o timer
            if (currentSpawnInterval > minSpawnInterval)
            {
                currentSpawnInterval -= intervalReduction;
                // Garante que n�o fique abaixo do m�nimo
                currentSpawnInterval = Mathf.Max(currentSpawnInterval, minSpawnInterval);
                Debug.Log($"Dificuldade aumentada! Novo intervalo de spawn: {currentSpawnInterval:F2}s");
            }
        }

        // --- Escala de Quantidade por Vez (Burst) ---
        burstTimer += Time.deltaTime;
        if (burstTimer >= timeToIncreaseBurstCount)
        {
            burstTimer = 0f;
            if (enemiesPerBurst < maxEnemiesInBurst)
            {
                enemiesPerBurst++;
                Debug.Log($"<color=orange>SURTO DE INIMIGOS! Agora spawnando {enemiesPerBurst} por vez!</color>");
            }
        }
    }


    private IEnumerator SpawnRoutine()
    {
        // Este loop � infinito, t�pico de um modo arcade
        while (true)
        {
            // Espera o tempo definido antes de spawnar
            yield return new WaitForSeconds(currentSpawnInterval);

            // S� tenta spawnar se houver inimigos dispon�veis e pontos de spawn
            if (availableEnemies.Count > 0 && spawnPoints.Count > 0)
            {
                // Spawna um "burst" (rajada) de inimigos
                for (int i = 0; i < enemiesPerBurst; i++)
                {
                    SpawnSingleEnemy();
                    // Pequeno delay entre os inimigos da mesma rajada para n�o aparecerem empilhados
                    if (enemiesPerBurst > 1) yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    private void SpawnSingleEnemy()
    {
        // Escolhe um tipo de inimigo aleat�rio dentre os dispon�veis
        GameObject enemyToSpawn = availableEnemies[Random.Range(0, availableEnemies.Count)];

        // Escolhe um ponto de spawn aleat�rio
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Instancia o inimigo
        Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}