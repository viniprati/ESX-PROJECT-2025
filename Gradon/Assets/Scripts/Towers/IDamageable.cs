// IDamageable.cs
public interface IDamageable
{
    // Qualquer script que implementar esta interface DEVE ter um m�todo TakeDamage.
    void TakeDamage(float damage);
}