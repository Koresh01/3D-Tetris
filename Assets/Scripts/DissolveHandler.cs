using System.Collections;
using UnityEngine;

public class DissolveHandler : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private float dissolveDuration = 1.5f;

    private Material material;
    private bool isVisible = false;

    private void Start()
    {
        material = objectRenderer.material;
    }

    public IEnumerator HideObject()
    {
        float startValue = material.GetFloat("_dissolve"); // Получаем текущее значение
        float time = 0;

        while (time < dissolveDuration)
        {
            time += Time.deltaTime;
            float dissolveValue = Mathf.Lerp(startValue, 0, time / dissolveDuration); // Используем актуальное значение
            material.SetFloat("_dissolve", dissolveValue);
            yield return null;
        }

        isVisible = true;
    }

    public IEnumerator ShowObject()
    {
        float startValue = material.GetFloat("_dissolve");
        float time = 0;

        while (time < dissolveDuration)
        {
            time += Time.deltaTime;
            float dissolveValue = Mathf.Lerp(startValue, 1, time / dissolveDuration);
            material.SetFloat("_dissolve", dissolveValue);
            yield return null;
        }

        isVisible = false;
    }
}
