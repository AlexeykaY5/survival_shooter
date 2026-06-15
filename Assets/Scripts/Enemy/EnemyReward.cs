using UnityEngine;

public class EnemyReward : MonoBehaviour
{

    [SerializeField] private int xpAmount = 10;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.OnDied += HandleDeath;
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDied -= HandleDeath;
        }
    }

    private void HandleDeath()
    {
        if (Player.Instance == null) return;
        
        if(Player.Instance.TryGetComponent<PlayerExperience>(out var xp))
        {
            xp.AddXP(xpAmount);
        }
    }
}
