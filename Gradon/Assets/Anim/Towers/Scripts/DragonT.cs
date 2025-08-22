// RangedTower.cs (Integrado com Upgrades e Buffs)
using UnityEngine;

// Garante que a torre herda da classe correta que lida com buffs e upgrades
public class RangedTower : TowerWithBuffs
{
    [Header("Atributos Ranged (N�vel 1)")]
    // Estes s�o os valores iniciais da torre no N�vel 1
    [SerializeField] private int initialDamage = 10;
    [SerializeField] private float initialAttackRate = 1.0f;
    [SerializeField] private float initialAttackRange = 5.0f;

    [Header("Refer�ncias do Proj�til")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    // A vari�vel 'damage' agora representa o dano ATUAL (com upgrades e buffs)
    private int damage;

    // O m�todo Start define os valores BASE para o N�vel 1
    protected override void Start()
    {
        // Define os status BASE para o n�vel inicial.
        // Estes valores ser�o atualizados permanentemente pelos upgrades.
        baseDamage = initialDamage;
        baseAttackRate = initialAttackRate;
        baseAttackRange = initialAttackRange;

        // Chama o Start da classe base (TowerWithBuffs), que ir� aplicar esses status.
        base.Start();
    }

    // Este m�todo � chamado pela classe base para aplicar ou remover buffs de dano.
    // Ele trabalha com 'baseDamage' para calcular o dano com buff.
    protected override void HandleDamageBuff(float multiplier, bool isApplying)
    {
        // Se estiver aplicando, multiplica o dano base. Se n�o, restaura para o dano base.
        damage = isApplying ? Mathf.RoundToInt(baseDamage * multiplier) : baseDamage;
    }

    // A l�gica de ataque dispara um proj�til com o dano atual.
    protected override void Attack()
    {
        if (projectilePrefab == null || firePoint == null || currentTarget == null) return;

        // Cria o proj�til no ponto de disparo
        GameObject projGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Pega o script do proj�til para configur�-lo
        Projectile projectileScript = projGO.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            // Passa o alvo e o dano ATUAL (que j� inclui upgrades e buffs) para o proj�til.
            // Supondo que seu proj�til tenha um m�todo Seek ou Launch.
            // projectileScript.Seek(currentTarget, this.damage);
        }

        Debug.Log(gameObject.name + " atirou em " + currentTarget.name + " com " + this.damage + " de dano.");
    }
}