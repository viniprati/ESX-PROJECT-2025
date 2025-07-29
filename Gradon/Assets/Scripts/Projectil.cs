// Projectile.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [Header("Configura��es do Proj�til")]
    [SerializeField] private float speed = 20f;
    // [SerializeField] private float damage = 10f; // REMOVA OU COMENTE ESTA LINHA
    [SerializeField] private float lifetime = 3f;

    [Header("Identifica��o do Alvo")]
    [SerializeField] private string targetTag;

    [Header("Efeitos")]
    [SerializeField] private GameObject hitEffectPrefab;

    // --- Vari�veis Internas ---
    private Rigidbody2D rb;
    private float currentDamage; // NOVA VARI�VEL: Armazena o dano para este proj�til espec�fico

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // M�TODO 'LAUNCH' MODIFICADO
    // Agora ele aceita a dire��o e o valor do dano
    public void Launch(Vector2 direction, float damageFromAttacker)
    {
        // 1. Armazena o dano recebido do atirador
        this.currentDamage = damageFromAttacker;

        // 2. Define a velocidade na dire��o fornecida
        rb.linearVelocity = direction.normalized * speed;

        // 3. Gira o proj�til
        transform.right = direction.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            IDamageable damageableObject = other.GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                // USA A NOVA VARI�VEL 'currentDamage'
                damageableObject.TakeDamage(currentDamage);

                if (hitEffectPrefab != null)
                {
                    Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }
}