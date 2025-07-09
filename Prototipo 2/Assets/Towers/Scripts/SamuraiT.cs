// SamuraiT.cs (Vers�o com Trigger)
using UnityEngine;
using System.Collections.Generic; // Para usar List<>

public class SamuraiT : MonoBehaviour
{
    [Header("Atributos da Torre")]
    public float attackRate = 1f;  // Ataques por segundo
    public int damage = 10;

    // Lista para guardar todos os inimigos que est�o atualmente dentro do trigger.
    private List<EnemyController> enemiesInRange = new List<EnemyController>();
    private float attackCooldown = 0f;

    // --- N�o precisamos mais da vari�vel 'attackRange' aqui,
    // pois o raio do Collider 2D (que � o Trigger) definir� o alcance. ---

    void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }

        // Se o tempo de espera acabou E existem inimigos no alcance...
        if (attackCooldown <= 0f && enemiesInRange.Count > 0)
        {
            Attack();
            attackCooldown = 1f / attackRate;
        }
    }

    // --- M�TODOS DE TRIGGER ---

    // Este m�todo � chamado AUTOMATICAMENTE pela Unity quando um outro Collider 2D ENTRA no nosso Trigger.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto que entrou tem a tag "Enemy".
        if (other.CompareTag("Enemy"))
        {
            // Pega o script do inimigo.
            EnemyController enemy = other.GetComponent<EnemyController>();
            // Se encontrou o script e ele ainda n�o est� na nossa lista, adiciona.
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    // Este m�todo � chamado AUTOMATICAMENTE quando um outro Collider 2D SAI do nosso Trigger.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            // Se o inimigo que saiu est� na nossa lista, remove-o.
            if (enemy != null && enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    // --- L�GICA DE ATAQUE ---

    void Attack()
    {
        // Itera sobre todos os inimigos que est�o atualmente na lista.
        // Usamos um loop 'for' reverso para poder remover itens da lista sem causar erros.
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            // Se o inimigo foi destru�do por outra torre enquanto estava na lista,
            // ele se tornar� 'null', ent�o o removemos da lista.
            if (enemiesInRange[i] == null)
            {
                enemiesInRange.RemoveAt(i);
                continue; // Pula para a pr�xima itera��o
            }

            // Aplica o dano ao inimigo
            enemiesInRange[i].TakeDamage(damage);
            Debug.Log("Torre Samurai atingiu " + enemiesInRange[i].name);
        }
    }
}