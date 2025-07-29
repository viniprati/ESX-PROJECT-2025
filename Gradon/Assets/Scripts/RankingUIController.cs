// RankingUIController.cs
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Necess�rio para poder chamar o GameManager do outro script

public class RankingUIController : MonoBehaviour
{
    [Header("Refer�ncias Top 3")]
    [Tooltip("Texto para o 1� lugar. Ex: '1. NOME - SCORE'.")]
    [SerializeField] private TextMeshProUGUI top1_Text;
    [SerializeField] private TextMeshProUGUI top2_Text;
    [SerializeField] private TextMeshProUGUI top3_Text;

    [Header("Refer�ncias Outros (4� ao 10�)")]
    [Tooltip("Um �nico objeto TextMeshProUGUI que ser� usado como template.")]
    [SerializeField] private TextMeshProUGUI otherRanks_TemplateText; // MODIFICA��O: Agora � um TextMeshProUGUI direto
    [Tooltip("O 'pai' onde os textos do 4� ao 10� lugar ser�o clonados.")]
    [SerializeField] private Transform otherRanks_ContainerParent; // MODIFICA��O: Nome mais claro para o container

    void Start()
    {
        // Garante que o RankingManager exista
        if (RankingManager.instance == null)
        {
            Debug.LogError("RankingManager n�o encontrado na cena! Por favor, certifique-se de que ele exista na cena do Menu.");
            // Se o RankingManager n�o existir, n�o podemos exibir o ranking.
            // Voc� pode querer desativar esta UI ou mostrar uma mensagem.
            this.gameObject.SetActive(false); // Desativa este controlador
            return;
        }

        PopulateRanking();
    }

    private void PopulateRanking()
    {
        // Limpa o conte�do antigo do container de "outros ranks" para evitar duplica��o
        if (otherRanks_ContainerParent != null)
        {
            foreach (Transform child in otherRanks_ContainerParent)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            Debug.LogError("otherRanks_ContainerParent n�o est� atribu�do no Inspector!");
            return;
        }

        // Garante que o template de texto tamb�m esteja configurado
        if (otherRanks_TemplateText == null)
        {
            Debug.LogError("otherRanks_TemplateText n�o est� atribu�do no Inspector!");
            return;
        }

        // Pega a lista de scores ordenada
        List<ScoreEntry> ranking = RankingManager.instance.GetRanking();

        // Preenche o Top 3
        top1_Text.text = ranking.Count > 0 ? $"1. {ranking[0].playerName} - {ranking[0].score}" : "1. ...";
        top2_Text.text = ranking.Count > 1 ? $"2. {ranking[1].playerName} - {ranking[1].score}" : "2. ...";
        top3_Text.text = ranking.Count > 2 ? $"3. {ranking[2].playerName} - {ranking[2].score}" : "3. ...";

        // Preenche os outros (do 4� ao 10� lugar)
        if (ranking.Count > 3)
        {
            // Come�a a partir do 4� elemento (�ndice 3)
            for (int i = 3; i < ranking.Count; i++)
            {
                // --- CLONA O OBJETO TEMPLATE ---
                // Instancia uma c�pia do objeto 'otherRanks_TemplateText' como filho do container.
                GameObject rankEntryGO = Instantiate(otherRanks_TemplateText.gameObject, otherRanks_ContainerParent);

                // Pega o componente TextMeshProUGUI da c�pia clonada
                TextMeshProUGUI rankEntryText = rankEntryGO.GetComponent<TextMeshProUGUI>();

                // Formata o texto
                int rankNumber = i + 1;
                rankEntryText.text = $"{rankNumber}. {ranking[i].playerName} - {ranking[i].score}";
            }
        }
    }

    public void GoToGame()
    {
        SceneManager.LoadScene(1);
    }
}