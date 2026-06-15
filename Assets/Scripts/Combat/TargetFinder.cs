using UnityEngine;

public static class TargetFinder
{
    public static Vector2 GetDirectionToNearestEnemy(Vector3 fromPosition)
    {
        EnemyAi[] enemies = Object.FindObjectsByType<EnemyAi>(FindObjectsSortMode.None);

        if(enemies.Length == 0)
        {
            return Vector2.zero;
        }

        EnemyAi nearest = null;

        float minDistance = float.MaxValue;

        foreach(EnemyAi enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(fromPosition, enemy.transform.position);

            if (distanceToEnemy < minDistance)
            {
                minDistance = distanceToEnemy;
                nearest = enemy;
            }
        }
        return ((Vector2)nearest.transform.position - (Vector2)fromPosition).normalized;
    }
}