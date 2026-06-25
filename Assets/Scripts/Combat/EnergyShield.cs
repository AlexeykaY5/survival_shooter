using System;
using UnityEngine;

public class EnergyShield : MonoBehaviour, IResource
{
    [SerializeField] private int maxEnergyShield = 100;
    [SerializeField] private float regenPerSecond = 2f;
    [SerializeField] private float regenDelay = 2f;

    private int currentEnergyShield;
    public int Current => currentEnergyShield;
    public int Max => maxEnergyShield;
    public event Action<int, int> OnChanged;
    private float regenAccumulator;
    private float lastDamagedTime;

    private void Awake()
    {
        currentEnergyShield = maxEnergyShield;
        OnChanged?.Invoke(currentEnergyShield, maxEnergyShield);
    }

    public int Absorb(int damage)
    {
        lastDamagedTime = Time.time;

        regenAccumulator = 0f;

        if(damage >= currentEnergyShield)
        {
            int leftover = damage - currentEnergyShield;
            currentEnergyShield = 0;
            OnChanged?.Invoke(currentEnergyShield, maxEnergyShield);
            return leftover;
        }
        else
        {
            currentEnergyShield -= damage;
            OnChanged?.Invoke(currentEnergyShield, maxEnergyShield);
            return 0;
        }
    }

    private void Update()
    {
        if (Time.time < lastDamagedTime + regenDelay) return;

        if (currentEnergyShield >= maxEnergyShield) return;

        regenAccumulator += regenPerSecond * Time.deltaTime;

        if (regenAccumulator >= 1f)
        {
            int wholeAmount = Mathf.FloorToInt(regenAccumulator);
            regenAccumulator -= wholeAmount;
            Restore(wholeAmount);
        }
    }

    public void Restore(int amount)
    {
        currentEnergyShield = Mathf.Min(currentEnergyShield + amount, maxEnergyShield);
        OnChanged?.Invoke(currentEnergyShield, maxEnergyShield);
    }
}