using UnityEngine;

using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    [Header("Flicker Settings")]
    [SerializeField] private float minTimeBetweenFlickers = 1f;
    [SerializeField] private float maxTimeBetweenFlickers = 4f;
    [SerializeField] private float minFlickerDuration = 0.05f;
    [SerializeField] private float maxFlickerDuration = 0.15f;

    [SerializeField][Range(0, 1)] private float minFlickerOpacity = 0.4f;
    [SerializeField][Range(0, 1)] private float maxFlickerOpacity = 0.9f;

    [SerializeField] private bool startFlickeringOnAwake = true;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Coroutine flickerCoroutine;
    private Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Start()
    {
        if (startFlickeringOnAwake)
        {
            StartFlickering();
        }
    }

    public void StartFlickering()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
        }
        flickerCoroutine = StartCoroutine(FlickerRoutine());
    }

    public void StopFlickering()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenFlickers, maxTimeBetweenFlickers);
            yield return new WaitForSeconds(waitTime);

            float flickerDuration = Random.Range(minFlickerDuration, maxFlickerDuration);
            float flickerOpacity = Random.Range(minFlickerOpacity, maxFlickerOpacity);

            Color flickerColor = originalColor;
            flickerColor.a = flickerOpacity;
            spriteRenderer.color = flickerColor;

            yield return new WaitForSeconds(flickerDuration);

            spriteRenderer.color = originalColor;
        }
    }

    void OnDisable()
    {
        StopFlickering();
    }

    void OnDestroy()
    {
        StopFlickering();
    }
}