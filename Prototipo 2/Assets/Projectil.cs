// Projectile.cs
using UnityEngine;

public class Projectil : MonoBehaviour
{
    public Transform target;
    public float speed = 15f;
    public int damage = 5;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move o proj�til em dire��o ao alvo
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Se colidir com o alvo, causa dano e se destr�i
        if (other.transform == target)
        {
            Debug.Log("Proj�til atingiu " + target.name);
            // col.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}