// TowerSlot.cs
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    // Vari�vel para rastrear se o slot tem uma torre.
    // 'public' para que o PlayerController possa ler o valor.
    // 'private set' para que somente este script possa alterar o valor (mais seguro).
    public bool isOccupied { get; private set; } = false;

    // Refer�ncia para a torre que est� neste slot.
    private GameObject currentTowerInstance;

    // M�todo p�blico que o PlayerController ir� chamar.
    // Ele recebe o "molde" (prefab) da torre a ser constru�da.
    public void PlaceTower(GameObject towerPrefab)
    {
        // Checagem de seguran�a: se o slot j� estiver ocupado, n�o faz nada.
        if (isOccupied)
        {
            Debug.LogWarning("Tentativa de construir em um slot j� ocupado. A��o ignorada.");
            return;
        }

        // Cria uma inst�ncia (uma c�pia) do prefab da torre.
        // Onde? Na posi��o deste slot (transform.position).
        // Com qual rota��o? Nenhuma (Quaternion.identity).
        currentTowerInstance = Instantiate(towerPrefab, transform.position, Quaternion.identity);

        // Marca o slot como ocupado para que outra torre n�o possa ser constru�da aqui.
        isOccupied = true;
    }

    // (Opcional) M�todo para liberar o slot se a torre for destru�da.
    // Voc� pode chamar isso se, no futuro, implementar uma forma de remover/vender torres.
    public void FreeUpSlot()
    {
        isOccupied = false;
        currentTowerInstance = null;
    }

    // Exemplo de como a torre poderia se auto-reportar como destru�da (para o futuro):
    // Se a torre tiver um script que a destr�i, ela poderia chamar:
    // FindObjectOfType<TowerSlot>().FreeUpSlot(); (n�o � a melhor forma, mas � um exemplo)
}