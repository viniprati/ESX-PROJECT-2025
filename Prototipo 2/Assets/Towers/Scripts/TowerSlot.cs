// TowerSlot.cs (Nova Vers�o)
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    // A �nica vari�vel que precisamos: um booleano para saber se o slot est� livre.
    // 'private set' significa que s� este script pode mudar o valor de 'isOccupied',
    // mas outros scripts podem ler (verificar) se ele � 'true' ou 'false'.
    public bool isOccupied { get; private set; }

    // Fun��o para construir uma torre.
    // Recebe o "molde" (prefab) da torre que deve ser constru�da.
    public void PlaceTower(GameObject towerPrefab)
    {
        // Checagem de seguran�a: se o slot j� estiver ocupado, n�o faz nada.
        if (isOccupied)
        {
            Debug.Log("Tentativa de construir em um slot j� ocupado.");
            return;
        }

        // AQUI A M�GICA ACONTECE:
        // Cria uma c�pia (uma inst�ncia) do prefab da torre.
        // Onde? Na posi��o deste slot (transform.position).
        // Com qual rota��o? Nenhuma (Quaternion.identity).
        Instantiate(towerPrefab, transform.position, Quaternion.identity);

        // Marca o slot como ocupado para que outra torre n�o possa ser constru�da aqui.
        isOccupied = true;
    }

    // (Opcional) Fun��o para liberar o slot.
    // Voc� pode chamar isso se, no futuro, implementar uma forma de remover/vender torres.
    public void FreeUpSlot()
    {
        isOccupied = false;
    }
}