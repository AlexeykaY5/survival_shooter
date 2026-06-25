using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    public enum ResourceType
    {
        Health,
        Mana,
        EnergyShield
    }

    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject sourceObject;
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private float smoothSpeed = 5f;

    private float targetFillAmount = 1f;
    private IResource source;

    private void Awake()
    {
        switch (resourceType)
        {
            case ResourceType.Health:
                source = sourceObject.GetComponent<Health>();
                break;
            case ResourceType.Mana:
                source = sourceObject.GetComponent<Mana>();
                break;
            case ResourceType.EnergyShield:
                source = sourceObject.GetComponent<EnergyShield>();
                break;
        }

        if (source == null)
        {
            Debug.Log($"{name}: source не реализует IResource", this);
            enabled = false;
            return;
        }
        source.OnChanged += UpdateBar;
    }

    private void Update()
    {
        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            targetFillAmount,
            smoothSpeed * Time.deltaTime);
    }

    private void UpdateBar(int current, int max)
    {
        targetFillAmount = (float)current / max;
    }

    private void OnDestroy()
    {
        if (source != null) source.OnChanged -= UpdateBar;
    }
}