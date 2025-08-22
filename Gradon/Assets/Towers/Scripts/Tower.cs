// TowerBase.cs
using UnityEngine;

// A palavra-chave 'abstract' significa que este script serve como uma base
// para outros scripts de torre (como SamuraiT, DragonT), mas n�o pode ser usado sozinho.
public abstract class TowerBase : MonoBehaviour
{
    [Header("Atributos Gerais da Torre")]
    [SerializeField] protected float attackRange = 5f;
    [SerializeField] protected float attackRate = 1f; // Ataques por segundo
    [SerializeField] public int cost = 50;
    [SerializeField] public Sprite towerIcon;

    [Header("Refer�ncias (Setup no Prefab)")]
    [Tooltip("A parte da torre que deve girar para encarar o inimigo.")]
    [SerializeField] protected Transform partToRotate;
    [Tooltip("O objeto SpriteRenderer que visualmente representa o alcance da torre.")]
    [SerializeField] protected SpriteRenderer rangeIndicator;

    [Header("Configura��es Visuais")]
    [Tooltip("A opacidade do indicador de alcance (0 = invis�vel, 1 = opaco).")]
    [Range(0f, 1f)]
    [SerializeField] private float rangeIndicatorOpacity = 0.15f;

    // --- Vari�veis Internas ---
    protected Transform currentTarget;
    protected float attackCooldown = 0f;
    private static readonly int EnemyLayer = 1 << 6; // Assume que a layer "Enemy" � a 6�. Ajuste se necess�rio.

    // --- Vari�veis para o Sistema de Buffs ---
    protected float originalAttackRate; // Guarda a cad�ncia de tiro original da torre

    // O m�todo 'virtual' permite que classes filhas (como SamuraiT) possam adicionar mais l�gica ao Start se precisarem.
    protected virtual void Start()
    {
        // Garante que o indicador de alcance esteja configurado corretamente
        if (rangeIndicator != null)
        {
            rangeIndicator.gameObject.SetActive(true);
            float diameter = attackRange * 2f;
            rangeIndicator.transform.localScale = new Vector3(diameter, diameter, 1f);
            Color currentColor = rangeIndicator.color;
            rangeIndicator.color = new Color(currentColor.r, currentColor.g, currentColor.b, rangeIndicatorOpacity);
        }

        attackCooldown = 0f;

        // Guarda a cad�ncia de tiro original no in�cio, antes de qualquer buff ser aplicado.
        originalAttackRate = attackRate;
    }

    protected virtual void Update()
    {
        // Se o alvo foi destru�do ou saiu do alcance, procura por um novo.
        if (currentTarget == null || Vector3.Distance(transform.position, currentTarget.position) > attackRange)
        {
            FindTarget();
        }

        // Se, ap�s a busca, um alvo foi encontrado...
        if (currentTarget != null)
        {
            HandleRotation();

            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f)
            {
                Attack();
                attackCooldown = 1f / attackRate;
            }
        }
    }

    private void FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, EnemyLayer);
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

    private void HandleRotation()
    {
        if (partToRotate == null) return;

        Vector2 direction = currentTarget.position - partToRotate.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        partToRotate.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Este m�todo � 'abstract', o que FOR�A as classes filhas (SamuraiT, DragonT) a criarem sua pr�pria vers�o de "Attack".
    protected abstract void Attack();

    // --- M�TODOS P�BLICOS PARA O SISTEMA DE BUFFS ---
    // A torre KirinT ir� chamar estas fun��es.

    public virtual void ApplyBuff(float rateMultiplier)
    {
        // Multiplica a cad�ncia de tiro ORIGINAL pelo multiplicador do buff.
        attackRate = originalAttackRate * rateMultiplier;
        Debug.Log(gameObject.name + " recebeu buff! Nova cad�ncia: " + attackRate);
    }

    public virtual void RemoveBuff()
    {
        // Restaura a cad�ncia de tiro para o valor original que guardamos no Start.
        attackRate = originalAttackRate;
        Debug.Log(gameObject.name + " perdeu o buff.");
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}