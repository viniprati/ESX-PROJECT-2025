// NormalEnemy.cs
using UnityEngine;

public class NormalEnemy : EnemyBase
{
    // A �nica coisa que ele precisa fazer � definir como se mover.
    protected override Vector2 HandleMovement()
    {
        // Apenas retorna a dire��o para o jogador. A classe base cuidar� da velocidade.
        if (playerTransform != null)
        {
            return (playerTransform.position - transform.position).normalized;
        }
        return Vector2.zero;
    }
}