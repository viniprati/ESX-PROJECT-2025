// BuildManager.cs
using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    // Padr�o Singleton: permite que qualquer script acesse este manager facilmente.
    public static BuildManager instance;

    private PlayerController playerController;
    private List<GameObject> availableTowers;
    private int selectedTowerIndex = 0;
    private GameObject ghostTowerInstance;
    private LayerMask towerSlotLayer;

    void Awake()
    {
        // Configura��o do Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Pega as refer�ncias do PlayerController para poder usar suas vari�veis
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            this.availableTowers = playerController.availableTowers;
            this.towerSlotLayer = playerController.towerSlotLayer;
        }
    }

    void Update()
    {
        // Se estivermos no modo de constru��o, atualiza a posi��o do fantasma.
        if (ghostTowerInstance != null)
        {
            UpdateGhostTowerPosition();
        }
    }

    // Chamado pelo PlayerController para entrar no modo de constru��o
    public void EnterBuildMode()
    {
        if (availableTowers.Count == 0) return;
        selectedTowerIndex = 0;
        SpawnGhostTower();
    }

    // Chamado pelo PlayerController para sair/cancelar
    public void ExitBuildMode()
    {
        if (ghostTowerInstance != null)
        {
            Destroy(ghostTowerInstance);
        }
    }

    // Chamado pelo PlayerController para trocar a torre
    public void SelectNextTower()
    {
        selectedTowerIndex++;
        if (selectedTowerIndex >= availableTowers.Count) selectedTowerIndex = 0;
        UpdateGhostTower();
    }

    public void SelectPreviousTower()
    {
        selectedTowerIndex--;
        if (selectedTowerIndex < 0) selectedTowerIndex = availableTowers.Count - 1;
        UpdateGhostTower();
    }

    // O cora��o da l�gica de constru��o
    public void TryPlaceTower(Vector3 playerPosition)
    {
        if (ghostTowerInstance == null || !ghostTowerInstance.activeSelf)
        {
            Debug.Log("Local inv�lido para constru��o!");
            return;
        }

        GameObject towerToBuildPrefab = availableTowers[selectedTowerIndex];

        // Assumindo que a torre tem um script 'SamuraiT' ou similar com um custo
        // Vamos pegar o custo do prefab.
        int towerCost = 0; // Voc� precisa implementar a l�gica de custo nas suas torres
        // Exemplo: if (towerToBuildPrefab.GetComponent<SamuraiT>() != null) towerCost = towerToBuildPrefab.GetComponent<SamuraiT>().cost;

        if (playerController.currentMana >= towerCost)
        {
            // TEM MANA SUFICIENTE
            playerController.SpendMana(towerCost);

            // Encontra o slot e constr�i a torre REAL
            Collider2D slotCollider = Physics2D.OverlapCircle(playerPosition, 0.2f, towerSlotLayer);
            if (slotCollider != null)
            {
                // AQUI EST� A LINHA QUE CRIA A TORRE DE VERDADE
                Instantiate(towerToBuildPrefab, slotCollider.transform.position, Quaternion.identity);
                Debug.Log(towerToBuildPrefab.name + " constru�da com sucesso!");
                // Adicionar l�gica para marcar o slot como ocupado aqui.
            }
        }
        else
        {
            // N�O TEM MANA
            Debug.Log("Mana insuficiente para construir " + towerToBuildPrefab.name);
        }
    }

    // --- Fun��es Auxiliares para a pr�-visualiza��o ---

    private void UpdateGhostTower()
    {
        if (ghostTowerInstance != null) Destroy(ghostTowerInstance);
        SpawnGhostTower();
    }

    private void SpawnGhostTower()
    {
        ghostTowerInstance = Instantiate(availableTowers[selectedTowerIndex]);

        ghostTowerInstance.GetComponent<Collider2D>().enabled = false;
        if (ghostTowerInstance.GetComponent<SamuraiT>() != null) ghostTowerInstance.GetComponent<SamuraiT>().enabled = false;

        SpriteRenderer sr = ghostTowerInstance.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.color = new Color(1f, 1f, 1f, 0.5f);
    }

    private void UpdateGhostTowerPosition()
    {
        Collider2D slotCollider = Physics2D.OverlapCircle(playerController.transform.position, 0.2f, towerSlotLayer);

        if (slotCollider != null)
        {
            ghostTowerInstance.transform.position = slotCollider.transform.position;
            ghostTowerInstance.SetActive(true);
        }
        else
        {
            ghostTowerInstance.SetActive(false);
        }
    }
}