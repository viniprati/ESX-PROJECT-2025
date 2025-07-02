using UnityEngine;
// Se for usar UI, adicione esta linha
// using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance; // Singleton

    [Header("Configura��o das Torres")]
    public GameObject[] towerPrefabs; // Array para os 3 prefabs das torres
    public int[] towerCosts; // Array para os custos de cada torre (na mesma ordem dos prefabs)

    [Header("Estado do Jogo")]
    public int currentGold = 100; // Ouro inicial

    private int selectedTowerIndex = 0;
    private bool isInBuildMode = false;

    // Refer�ncia para a UI (opcional por enquanto)
    // public GameObject buildUI;
    // public Text selectedTowerText;


    void Awake()
    {
        // Configura��o do Singleton
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void EnterBuildMode()
    {
        isInBuildMode = true;
        Debug.Log("Modo de constru��o ATIVADO. Torre selecionada: " + towerPrefabs[selectedTowerIndex].name);
        // Aqui voc� ativaria a sua UI
        // buildUI.SetActive(true);
        // UpdateUI();
    }

    public void ExitBuildMode()
    {
        isInBuildMode = false;
        Debug.Log("Modo de constru��o DESATIVADO.");
        // Aqui voc� desativaria a sua UI
        // buildUI.SetActive(false);
    }

    public void SelectNextTower()
    {
        if (!isInBuildMode) return;

        selectedTowerIndex++;
        if (selectedTowerIndex >= towerPrefabs.Length)
        {
            selectedTowerIndex = 0; // Volta para a primeira
        }
        Debug.Log("Torre selecionada: " + towerPrefabs[selectedTowerIndex].name);
        // UpdateUI();
    }

    public void SelectPreviousTower()
    {
        if (!isInBuildMode) return;

        selectedTowerIndex--;
        if (selectedTowerIndex < 0)
        {
            selectedTowerIndex = towerPrefabs.Length - 1; // Vai para a �ltima
        }
        Debug.Log("Torre selecionada: " + towerPrefabs[selectedTowerIndex].name);
        // UpdateUI();
    }

    public void TryPlaceTower(Vector3 position)
    {
        int cost = towerCosts[selectedTowerIndex];

        if (currentGold >= cost)
        {
            // Tem ouro suficiente
            currentGold -= cost;
            Instantiate(towerPrefabs[selectedTowerIndex], position, Quaternion.identity);
            Debug.Log("Torre constru�da! Ouro restante: " + currentGold);
        }
        else
        {
            // N�o tem ouro
            Debug.Log("Ouro insuficiente! Precisa de " + cost);
        }
    }

    // Fun��o para atualizar a UI (vamos deixar para depois, mas a l�gica ficaria aqui)
    // void UpdateUI() {
    //     selectedTowerText.text = $"Torre: {towerPrefabs[selectedTowerIndex].name}\nCusto: {towerCosts[selectedTowerIndex]}";
    // }
}