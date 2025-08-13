using UnityEngine;
using UnityEngine.UI;

public class TotemHealth : MonoBehaviour
{
    [Header("Configura��es da Base")]
    [Tooltip("A vida m�xima do totem.")]
    [SerializeField] private float maxHealth = 1000f;

    [Tooltip("Refer�ncia opcional para uma barra de vida na UI.")]
    [SerializeField] private Slider healthBar;

    public float CurrentHealth { get; private set; }

    private bool isDestroyed = false;

    void Start()
    {
        CurrentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed) return;

        CurrentHealth -= damage;

        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        UpdateHealthBar();
        Debug.Log($"Base sofreu {damage} de dano! Vida restante: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = CurrentHealth;
        }
    }

    // --- M�TODO Die ---
    private void Die()
    {
        isDestroyed = true;

        Debug.Log("<color=red>GAME OVER! A base foi destru�da.</color>");


        gameObject.SetActive(false);
    }
}