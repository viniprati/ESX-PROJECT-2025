// KirinT.cs (Buff Tower)
using UnityEngine;
using System.Collections.Generic;

public class KirinT : TowerBase
{
    [Header("Atributos de Buff")]
    [Tooltip("Multiplicador de dano (1.5 = +50% de dano)")]
    [SerializeField] private float damageMultiplier = 1.5f;
    [Tooltip("Multiplicador da taxa de ataque (1.5 = 50% mais r�pido)")]
    [SerializeField] private float rateMultiplier = 1.5f;
    [Tooltip("Por quanto tempo o buff dura ap�s ser aplicado")]
    [SerializeField] private float buffDuration = 3f;

    // Kirin n�o ataca, ent�o vamos sobrescrever o Update para n�o procurar inimigos
    protected override void Update()
    {
        // A l�gica de ataque/cooldown � usada para aplicar o buff periodicamente
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            Attack(); // O "Ataque" aqui � aplicar o buff
            attackCooldown = 1f / attackRate;
        }
    }

    // O ataque da Kirin � dar buff nas torres ao redor
    protected override void Attack()
    {
        // Encontra todas as torres no raio de alcance (buffRange)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (var col in colliders)
        {
            // Tenta pegar o script de outra torre (que n�o seja esta)
            TowerWithBuffs tower = col.GetComponent<TowerWithBuffs>();
            if (tower != null && tower != this)
            {
                // Aplica o buff na torre encontrada
                tower.ApplyBuff(damageMultiplier, rateMultiplier, buffDuration);
            }
        }
    }
}