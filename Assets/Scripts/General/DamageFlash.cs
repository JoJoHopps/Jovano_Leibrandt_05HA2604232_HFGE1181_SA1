using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image flashImage;
    [SerializeField] private float flashAlpha = 0.45f;
    [SerializeField] private float fadeSpeed = 2f;

    private float lastValue;
    private bool initialised;

    private void Start()
    {
        if (flashImage == null) flashImage = GetComponent<Image>();
        SetAlpha(0f);
    }

    private void Update()
    {
        if (healthSlider != null)
        {
            if (!initialised) { lastValue = healthSlider.value; initialised = true; }
            if (healthSlider.value < lastValue - 0.01f) SetAlpha(flashAlpha);
            lastValue = healthSlider.value;
        }

        if (flashImage != null && flashImage.color.a > 0f)
            SetAlpha(Mathf.MoveTowards(flashImage.color.a, 0f, fadeSpeed * Time.unscaledDeltaTime));
    }

    private void SetAlpha(float a)
    {
        if (flashImage == null) return;
        Color c = flashImage.color;
        c.a = a;
        flashImage.color = c;
    }
}