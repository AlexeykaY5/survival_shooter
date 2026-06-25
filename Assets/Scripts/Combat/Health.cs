using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IResource
{
    [SerializeField] private int maxHealth = 100;

    public event Action OnDied;
    private int currentHealth;
    public int Current => currentHealth;
    public int Max => maxHealth;
    public event Action<int, int> OnChanged;


    private void Awake()
    {
        currentHealth = maxHealth;
        OnChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (TryGetComponent<EnergyShield>(out var energyShield))
        {
            damage = energyShield.Absorb(damage);
        }

        if (damage > 0)
        {
            currentHealth -= damage;
            OnChanged?.Invoke(currentHealth, maxHealth);
        }else if(damage <= 0)
        {
            return;
        }


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if(GetComponent<Player>())
        {
            Debug.Log("Игрок погиб!");
            // Здесь можно добавить логику для перезапуска уровня или отображения экрана Game Over
        }
        else
        {
            OnDied?.Invoke();
            Destroy(gameObject);
        }
    }
}
