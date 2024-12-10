using UnityEngine;
using UnityEngine.UI;

public class CountdownTimerWithFloorFade : MonoBehaviour
{
    public Text countdownText;
    public float countdownTime = 180f;
    public Renderer floorRenderer;
    public float fadeDuration = 2f;
    public float darkeningFactor = 0.5f;

    private float remainingTime;
    private bool isCountingDown = true;
    private Color originalColor;

    void Start()
    {
        remainingTime = countdownTime;
        UpdateCountdownText();

        if (floorRenderer != null)
        {
            originalColor = floorRenderer.material.color;
        }
    }

    void Update()
    {
        if (isCountingDown)
        {
            remainingTime -= Time.deltaTime;
            remainingTime = Mathf.Max(remainingTime, 0);
            UpdateCountdownText();

            if (remainingTime <= 0)
            {
                isCountingDown = false;
                TriggerEndEffects();
            }
        }
    }

    void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            countdownText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }

    void TriggerEndEffects()
    {
        if (countdownText != null)
        {
            countdownText.text = "DERROTALO";
        }

        if (floorRenderer != null)
        {
            StartCoroutine(FadeFloorToDarker());
        }
    }

    System.Collections.IEnumerator FadeFloorToDarker()
    {
        float elapsedTime = 0f;
        Color targetColor = originalColor * darkeningFactor;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            floorRenderer.material.color = Color.Lerp(originalColor, targetColor, t);
            yield return null;
        }

        floorRenderer.material.color = targetColor;
    }
}
