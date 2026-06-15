using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{

    [SerializeField] private Image fillImage;
    [SerializeField] private Health source;

    private void Awake()
    {
        source.OnHealthChange += UpdateBar;
    }

    private void UpdateBar(int current, int max)
    {
        fillImage.fillAmount = (float)current / max;
    }

    private void OnDestroy()
    {
        if (source != null) source.OnHealthChange -= UpdateBar;
    }
}