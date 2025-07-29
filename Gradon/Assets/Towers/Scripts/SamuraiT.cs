// SamuraiTower.cs
using UnityEngine;

public class SamuraiTower : TowerWithBuffs
{
    [Header("Atributos Samurai (�rea)")]
    [SerializeField] private int damage = 15;
    [SerializeField] private GameObject attackEffectPrefab; // Efeito visual do corte

    protected override void Start()
    {
        base.Start();
        originalDamage = damage; // Guarda o dano original
    }

    protected override void HandleDamageBuff(float multiplier, bool isApplying)
    {
        damage = isApplying ? Mathf.RoundToInt(originalDamage * multiplier) : originalDamage;
    }

    // A rota��o da base n�o faz muito sentido aqui, mas a l�gica de alvo ainda � �til.
    protected override void Attack()
    {
        // Opcional: Instancia um efeito visual para o ataque
        if (attackEffectPrefab != null)
        {
            Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
        }

        // Encontra todos os inimigos no alcance para dar dano em �rea
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, 1 << 6); // Layer "Enemy"

        foreach (var col in colliders)
        {
            // Pega o script base do inimigo para aplicar o dano
            EnemyBase enemy = col.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}