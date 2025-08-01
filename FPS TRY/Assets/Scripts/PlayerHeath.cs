using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHeath : MonoBehaviour
{
    private float health;
    private float lerpTimer;

    [Header("Health Bar")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration;
    public float fadespeed;
    private float durationTimer;

    [Header("Game Over")]
    public GameObject gameOverScreen;
    public Image fadePanel;  // Assign in Inspector
    public float fadeDuration = 2f;

    private bool hasDied = false;

    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        fadePanel.color = new Color(0, 0, 0, 0);  // Fully transparent
        gameOverScreen.SetActive(false);         // Hide Game Over UI at start
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (overlay.color.a > 0)
        {
            if (health < 30)
                durationTimer += Time.deltaTime;

            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadespeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }

        if (health <= 0 && !hasDied)
        {
            hasDied = true;
            StartCoroutine(FadeToGameOver());
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0f;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete * percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete * percentComplete);
        }
    }

    IEnumerator FadeToGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float elapsed = 0f;
        Color startColor = fadePanel.color;
        Color targetColor = new Color(0, 0, 0, 1); // Full black

        // Smooth fade using Lerp
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadePanel.color = Color.Lerp(startColor, targetColor, elapsed / fadeDuration);
            yield return null;
        }

        // Force fully black just in case
        fadePanel.color = targetColor;

        // Activate Game Over screen
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Pause game AFTER screen shows up
        Time.timeScale = 0f;
    }
}
