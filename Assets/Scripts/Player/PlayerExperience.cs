using System;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private int xpPerLevel = 100;

    private int currentXP;
    private int currentLevel = 1;
    public int CurrentXP => currentXP;
    public int CurrentLevel => currentLevel;

    public event Action<int> OnXPChanged;
    public event Action<int> OnLevelUp;

    public void AddXP(int amount) 
    { 
        currentXP += amount;
        OnXPChanged?.Invoke(currentXP);

        while(currentXP >= xpPerLevel)
        {
            currentXP -= xpPerLevel;
            currentLevel++;
            xpPerLevel *= 2;
            OnLevelUp?.Invoke(currentLevel);
        }

        Debug.Log($"XP: {currentXP}/{xpPerLevel}, Level: {currentLevel}");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
