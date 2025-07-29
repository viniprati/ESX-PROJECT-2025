// TowerWithBuffs.cs
using System.Collections;
using UnityEngine;

public abstract class TowerWithBuffs : TowerBase
{
    // Vari�veis para guardar os status originais
    protected float originalAttackRate;
    protected int originalDamage; // Supondo que o dano seja um int
    private Coroutine currentBuffCoroutine;

    protected override void Start()
    {
        base.Start(); // Executa a l�gica da classe base
        // Guarda os valores originais para poder restaur�-los
        originalAttackRate = attackRate;
        // Voc� precisar� fazer isso para o dano tamb�m em cada torre espec�fica.
    }

    public void ApplyBuff(float damageMultiplier, float rateMultiplier, float duration)
    {
        // Se j� existe um buff, reseta antes de aplicar o novo
        if (currentBuffCoroutine != null)
        {
            StopCoroutine(currentBuffCoroutine);
            RemoveBuff();
        }

        // Inicia o processo de buff
        currentBuffCoroutine = StartCoroutine(BuffSequence(damageMultiplier, rateMultiplier, duration));
    }

    private IEnumerator BuffSequence(float damageMultiplier, float rateMultiplier, float duration)
    {
        // Aplica o buff
        attackRate *= rateMultiplier;
        // O dano � espec�fico para cada torre, ent�o elas precisam lidar com isso
        HandleDamageBuff(damageMultiplier, true);

        Debug.Log(gameObject.name + " BUFFED!");

        // Espera a dura��o do buff
        yield return new WaitForSeconds(duration);

        // Remove o buff
        RemoveBuff();
        Debug.Log(gameObject.name + " Buff ended.");
        currentBuffCoroutine = null;
    }

    private void RemoveBuff()
    {
        attackRate = originalAttackRate;
        HandleDamageBuff(1, false); // Restaura o dano
    }

    // Cada torre de dano precisa implementar como seu dano � alterado
    protected abstract void HandleDamageBuff(float multiplier, bool isApplying);
}