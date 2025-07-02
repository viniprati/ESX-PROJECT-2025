// Tower.cs
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Atributos da Torre (Base)")]
    [Tooltip("O alcance de detec��o e ataque da torre.")]
    [SerializeField] protected float attackRange = 5f;

    [Tooltip("O custo para construir esta torre.")]
    [SerializeField] public int cost = 50;

    protected Transform currentTarget;
    private string enemyTag = "Enemy";

    // A l�gica agora � mais simples: encontrar um alvo e, se tiver um, atacar todo frame.
    protected virtual void Update()
    {
        FindTarget();

        if (currentTarget != null)
        {
            Attack();
        }
    }

    private void FindTarget()
    {
        // Se j� temos um alvo, verifica se ele ainda est� no alcance ou se foi destru�do
        if (currentTarget != null && Vector2.Distance(transform.position, currentTarget.position) > attackRange)
        {
            currentTarget = null;
        }

        // Se n�o temos alvo, procura um novo que esteja mais pr�ximo
        if (currentTarget == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));
            float closestDistance = Mathf.Infinity;
            Transform newTarget = null;

            foreach (Collider2D collider in colliders)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    newTarget = collider.transform;
                }
            }
            currentTarget = newTarget;
        }
    }

    // M�todo abstrato para o ataque, que agora ser� chamado continuamente.
    protected abstract void Attack();

    // Desenha o alcance no editor da Unity para facilitar o debug.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}