// EnemySeparation.cs (Vers�o 2.0 - Mais Robusta)
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemySeparation : MonoBehaviour
{
    [Header("Configura��o da Separa��o")]
    [Tooltip("O raio no qual este inimigo detecta outros para se afastar.")]
    [SerializeField] private float separationRadius = 1.5f;

    [Tooltip("A for�a m�xima de repuls�o quando dois inimigos est�o quase se tocando.")]
    [SerializeField] private float maxSeparationForce = 10f;

    [Tooltip("A Layer em que os outros inimigos est�o.")]
    [SerializeField] private LayerMask enemyLayer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ApplySeparationForce();
    }

    private void ApplySeparationForce()
    {
        // Encontra todos os colliders de inimigos pr�ximos dentro do raio de separa��o.
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationRadius, enemyLayer);

        // Itera sobre cada vizinho encontrado.
        foreach (var neighbor in nearbyEnemies)
        {
            // Garante que n�o est� se comparando consigo mesmo.
            if (neighbor.gameObject == this.gameObject)
            {
                continue;
            }

            // Calcula a dist�ncia e a dire��o para longe do vizinho.
            Vector2 awayFromNeighbor = transform.position - neighbor.transform.position;
            float distance = awayFromNeighbor.magnitude;

            // Se a dist�ncia for zero (caso raro), ignora para evitar divis�o por zero.
            if (distance == 0) continue;

            // --- L�gica de Atenua��o ---
            // A for�a de repuls�o � inversamente proporcional � dist�ncia.
            // Quanto mais perto o vizinho, mais forte � o "empurr�o".
            float repulsionStrength = 1.0f - (distance / separationRadius);

            // Calcula a for�a final a ser aplicada.
            Vector2 force = awayFromNeighbor.normalized * repulsionStrength * maxSeparationForce;

            // Aplica a for�a ao Rigidbody.
            rb.AddForce(force);
        }
    }

    // Desenha o raio de separa��o no editor para facilitar o debug.
    private void OnDrawGizmosSelected()
    {
        if (!enabled) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}