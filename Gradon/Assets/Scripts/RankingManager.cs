// RankingManager.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Necess�rio para usar OrderByDescending

public class RankingManager : MonoBehaviour
{
    public static RankingManager instance;

    private ScoreList rankingData;
    private const string RankingKey = "GameRanking"; // A chave para salvar nos PlayerPrefs

    void Awake()
    {
        // Padr�o Singleton com DontDestroyOnLoad
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadRanking(); // Carrega os dados salvos ao iniciar
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadRanking()
    {
        // Verifica se j� existem dados salvos
        if (PlayerPrefs.HasKey(RankingKey))
        {
            string json = PlayerPrefs.GetString(RankingKey);
            rankingData = JsonUtility.FromJson<ScoreList>(json);
        }
        else
        {
            // Se n�o, cria uma nova lista vazia
            rankingData = new ScoreList();
        }
    }

    private void SaveRanking()
    {
        // Ordena a lista do maior para o menor score
        rankingData.scores = rankingData.scores.OrderByDescending(entry => entry.score).ToList();

        // Limita o ranking aos 10 melhores
        if (rankingData.scores.Count > 10)
        {
            rankingData.scores = rankingData.scores.GetRange(0, 10);
        }

        // Converte a lista para JSON e salva
        string json = JsonUtility.ToJson(rankingData);
        PlayerPrefs.SetString(RankingKey, json);
        PlayerPrefs.Save();
    }

    // M�todo p�blico para adicionar uma nova pontua��o
    public void AddScore(string playerName, int score)
    {
        ScoreEntry newEntry = new ScoreEntry { playerName = playerName, score = score };
        rankingData.scores.Add(newEntry);
        SaveRanking(); // Salva e reordena a lista
    }

    // M�todo p�blico para obter a lista de scores
    public List<ScoreEntry> GetRanking()
    {
        return rankingData.scores;
    }

    // (Opcional) M�todo para limpar o ranking para testes
    public void ClearRanking()
    {
        rankingData.scores.Clear();
        PlayerPrefs.DeleteKey(RankingKey);
        Debug.Log("Ranking foi limpo!");
    }
}