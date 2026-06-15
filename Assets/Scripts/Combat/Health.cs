using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;

    public event Action<int, int> OnHealthChange; 
    public event Action OnDied;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChange?.Invoke(currentHealth, maxHealth);

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
