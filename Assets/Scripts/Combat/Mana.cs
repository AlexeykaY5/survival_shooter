using UnityEngine;
using System;

public class Mana : MonoBehaviour, IResource
{
    [SerializeField] private int maxMana = 100;
    [SerializeField] private float regenPerSecond = 1f;

    private int currentMana;
    public int Current => currentMana;
    public int Max => maxMana;
    public event Action<int, int> OnChanged;
    private float regenAccumulator;

    private void Awake()
    {
        currentMana = maxMana;
        OnChanged?.Invoke(currentMana, maxMana);
    }

    private void Update()
    {
        if (currentMana >= maxMana) return;

        regenAccumulator += regenPerSecond * Time.deltaTime;

        if(regenAccumulator >= 1f)
        {
            int wholeAmount = Mathf.FloorToInt(regenAccumulator);
            regenAccumulator -= wholeAmount;
            Restore(wholeAmount);
        }
    }

    public bool TryUse(int amount)
    {
        if(currentMana < amount) 
        {
            return false;
        }
        else
        {
            currentMana -= amount;
            OnChanged?.Invoke(currentMana, maxMana);
            return true;
        }
    }

    public void Restore(int amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        OnChanged?.Invoke(currentMana, maxMana);
    }
}
