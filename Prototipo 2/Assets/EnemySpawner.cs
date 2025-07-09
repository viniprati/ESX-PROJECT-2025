// EnemySpawner.cs (M�ltiplos Pontos de Spawn + 15 Ondas)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs dos Inimigos")]
    [SerializeField] private GameObject ghoulPrefab;
    [SerializeField] private GameObject oniPrefab;

    [Header("Pontos de Spawn")]
    [Tooltip("Crie objetos vazios na cena para serem os pontos de spawn e arraste-os para esta lista.")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Configura��es das Ondas")]
    [Tooltip("O n�mero total de ondas no jogo.")]
    [SerializeField] private int totalWaves = 15;

    [Tooltip("N�mero inicial de inimigos na primeira onda.")]
    [SerializeField] private int initialEnemiesPerWave = 5;

    [Tooltip("Quantos inimigos a mais s�o adicionados a cada nova onda.")]
    [SerializeField] private int enemiesIncreasePerWave = 3;

    [Tooltip("O tempo, em segundos, de pausa entre as ondas.")]
    [SerializeField] private float timeBetweenWaves = 10f;

    [Tooltip("O intervalo de tempo inicial entre cada inimigo gerado dentro de uma onda.")]
    [SerializeField] private float initialSpawnInterval = 1.5f;

    [Tooltip("Quanto o intervalo de spawn diminui a cada onda.")]
    [SerializeField] private float spawnIntervalDecrease = 0.07f;

    [Tooltip("O intervalo de spawn m�nimo que podemos atingir.")]
    [SerializeField] private float minSpawnInterval = 0.3f;

    [Header("Chances de Spawn")]
    [Range(0, 100)]
    [SerializeField] private float oniSpawnChance = 20f;

    // --- Vari�veis Internas ---
    private int currentWaveNumber = 1;
    private int enemiesToSpawn;
    private float currentSpawnInterval;

    void Start()
    {
        // Checagem de seguran�a para os pontos de spawn
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("A lista 'Spawn Points' est� vazia! O spawner n�o sabe onde gerar os inimigos.");
            this.enabled = false;
            return;
        }

        // Define os valores iniciais
        enemiesToSpawn = initialEnemiesPerWave;
        currentSpawnInterval = initialSpawnInterval;

        // Inicia a coroutine principal que gerencia as ondas
        StartCoroutine(WaveSpawnerRoutine());
    }

    private IEnumerator WaveSpawnerRoutine()
    {
        // Loop que roda para cada onda, de 1 at� totalWaves.
        while (currentWaveNumber <= totalWaves)
        {
            Debug.Log($"<color=cyan>--- INICIANDO ONDA {currentWaveNumber} / {totalWaves} ---</color>");

            yield return StartCoroutine(SpawnWave());

            if (currentWaveNumber < totalWaves)
            {
                Debug.Log($"Onda {currentWaveNumber} finalizada. Pr�xima onda em {timeBetweenWaves} segundos.");
                yield return new WaitForSeconds(timeBetweenWaves);
                PrepareNextWave();
            }
            else
            {
                break; // Sai do loop ap�s a �ltima onda ser gerada.
            }
        }

        // Ap�s o fim de todas as ondas, come�a a verificar a condi��o de vit�ria.
        StartCoroutine(CheckForVictoryRoutine());
    }

    private IEnumerator SpawnWave()
    {
        // Gera o n�mero de inimigos definido para esta onda
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    private void PrepareNextWave()
    {
        currentWaveNumber++;
        enemiesToSpawn += enemiesIncreasePerWave;

        if (currentSpawnInterval > minSpawnInterval)
        {
            currentSpawnInterval -= spawnIntervalDecrease;
        }
    }

    private void SpawnRandomEnemy()
    {
        // Escolhe um ponto de spawn aleat�rio da lista
        Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Escolhe qual inimigo gerar
        float randomValue = Random.Range(0f, 100f);
        GameObject prefabToSpawn = (randomValue <= oniSpawnChance) ? oniPrefab : ghoulPrefab;

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, chosenSpawnPoint.position, chosenSpawnPoint.rotation);
        }
    }

    private IEnumerator CheckForVictoryRoutine()
    {
        Debug.Log("<color=orange>Ondas finalizadas! Derrote os inimigos restantes para vencer!</color>");

        while (true)
        {
            // Procura por qualquer objeto com a tag "Enemy".
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                DeclareVictory();
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void DeclareVictory()
    {
        Debug.Log("<color=green>VIT�RIA! Voc� sobreviveu a todas as ondas!</color>");
        // Adicione sua l�gica de tela de vit�ria aqui.
    }
}