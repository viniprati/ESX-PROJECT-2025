// BuildManager.cs
using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    [Header("Configura��es de Constru��o")]
    [Tooltip("O custo em mana para construir QUALQUER torre.")]
    [SerializeField] private int towerCost = 50; // MODIFICA��O: Vari�vel para o custo

    // --- Refer�ncias e Estado ---
    private PlayerController playerController;
    private List<GameObject> availableTowers;
    private int selectedTowerIndex = 0;
    private GameObject ghostTowerInstance;
    private bool isBuilding = false;

    public bool IsInBuildMode { get { return isBuilding; } }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            this.availableTowers = playerController.availableTowers;
        }
    }

    void Update()
    {
        if (isBuilding && ghostTowerInstance != null)
        {
            UpdateGhostTowerPosition();
        }
    }

    // --- M�todos P�blicos chamados pelo PlayerController ---

    public void ToggleBuildMode()
    {
        if (!isBuilding)
        {
            EnterBuildMode();
        }
        else
        {
            TryPlaceTower();
        }
    }

    public void SelectNextTower()
    {
        if (!isBuilding) return;
        selectedTowerIndex++;
        if (selectedTowerIndex >= availableTowers.Count) selectedTowerIndex = 0;
        UpdateGhostTower();
    }

    public void SelectPreviousTower()
    {
        if (!isBuilding) return;
        selectedTowerIndex--;
        if (selectedTowerIndex < 0) selectedTowerIndex = availableTowers.Count - 1;
        UpdateGhostTower();
    }

    // --- L�gica Interna de Constru��o ---

    private void EnterBuildMode()
    {
        if (availableTowers == null || availableTowers.Count == 0)
        {
            Debug.LogError("A lista 'Available Towers' no PlayerController est� vazia!");
            return;
        }

        // MODIFICA��O: Usa a nova vari�vel de custo para a verifica��o
        if (playerController.currentMana < towerCost)
        {
            Debug.Log("Mana insuficiente para iniciar a constru��o!");
            return;
        }

        isBuilding = true;
        selectedTowerIndex = 0;
        SpawnGhostTower();
    }

    private void ExitBuildMode()
    {
        isBuilding = false;
        if (ghostTowerInstance != null)
        {
            Destroy(ghostTowerInstance);
        }
    }

    private void TryPlaceTower()
    {
        if (ghostTowerInstance == null) return;

        // MODIFICA��O: Usa a nova vari�vel de custo para a verifica��o
        if (playerController.currentMana >= towerCost)
        {
            playerController.SpendMana(towerCost);

            GameObject towerToBuildPrefab = availableTowers[selectedTowerIndex];
            Instantiate(towerToBuildPrefab, playerController.transform.position, Quaternion.identity);

            ExitBuildMode();
        }
        else
        {
            Debug.Log("Mana insuficiente!");
        }
    }
// --- Fun��es Auxiliares da Pr�-Visualiza��o ---

private void UpdateGhostTower()
    {
        if (ghostTowerInstance != null) Destroy(ghostTowerInstance);
        SpawnGhostTower();
    }

    private void SpawnGhostTower()
    {
        ghostTowerInstance = Instantiate(availableTowers[selectedTowerIndex]);
        ghostTowerInstance.name = "GHOST_TOWER";

        var collider = ghostTowerInstance.GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        var samuraiScript = ghostTowerInstance.GetComponent<SamuraiT>();
        if (samuraiScript != null) samuraiScript.enabled = false;

        var spriteRenderer = ghostTowerInstance.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
    }

    private void UpdateGhostTowerPosition()
    {
        if (playerController != null)
        {
            ghostTowerInstance.transform.position = playerController.transform.position;
        }
    }
}