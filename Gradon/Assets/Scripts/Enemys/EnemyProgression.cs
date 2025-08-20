// Esta pequena classe/struct nos ajuda a organizar a progress�o dos inimigos no Inspector.
// [System.Serializable] faz com que ela apare�a no Inspector da Unity.
using UnityEngine;

[System.Serializable]
public class EnemyProgression
{
    public string description; // Apenas para organiza��o no Inspector
    public GameObject enemyPrefab;
    [Tooltip("Em quantos segundos de jogo este inimigo come�a a aparecer.")]
    public float timeToStartSpawning;
}