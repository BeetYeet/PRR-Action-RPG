using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarHandler : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float currentHealth;

    [SerializeField, Range(0f, 1f)]
    private float currentDamageAmount;

    private float timeSinceDamage;

    [SerializeField, Range(0f, 1f)]
    private float currentHealAmount;

    private float timeSinceHeal;

    [SerializeField]
    private Image healthImage;

    [SerializeField]
    private Image healImage;

    [SerializeField]
    private Image damageImage;

    public float maxUnchangedTime = 3f;
    public float unchangedDecayTime = .5f;

    [SerializeField]
    private Transform root = null;

    private void Awake()
    {
        ResetValues();
    }

    public void Destroy()
    {
        Destroy(root.gameObject);
    }

    public void ResetValues()
    {
        currentDamageAmount = 0f;
        currentHealAmount = 0f;
    }

    private void Update()
    {
        if (currentHealAmount > 0f)
        {
            timeSinceHeal += Time.deltaTime;
            if (timeSinceHeal > maxUnchangedTime)
                currentHealAmount = Mathf.Clamp01(currentHealAmount - Time.deltaTime / unchangedDecayTime);
        }

        if (currentDamageAmount > 0f)
        {
            timeSinceDamage += Time.deltaTime;
            if (timeSinceDamage > maxUnchangedTime)
                currentDamageAmount = Mathf.Clamp01(currentDamageAmount - Time.deltaTime / unchangedDecayTime);
        }
        UpdateHealth();
    }

    private void OnValidate()
    {
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        UpdateHealth(currentHealth);
    }

    public void UpdateHealth(float newHealth)
    {
        newHealth = Mathf.Clamp01(newHealth);

        if (newHealth < currentHealth)
        {
            // Took damage
            currentDamageAmount += currentHealth - newHealth;
            timeSinceDamage = 0f;
            currentHealth = newHealth;
        }
        if (newHealth > currentHealth)
        {
            // Healed
            currentHealAmount += newHealth - currentHealth;
            timeSinceHeal = 0f;
            currentHealth = newHealth;
        }

        UpdateHealthbars(GetHealthbars());
    }

    private void UpdateHealthbars(Vector3 healthbars)
    {
        healthImage.fillAmount = healthbars.x;
        healImage.fillAmount = healthbars.y;
        damageImage.fillAmount = healthbars.z;
    }

    private Vector3 GetHealthbars()
    {
        Vector3 output = new Vector3();
        float sum = currentHealth - currentHealAmount;
        sum = Mathf.Clamp01(sum);
        output.x = sum;

        sum += currentHealAmount;
        sum = Mathf.Clamp01(sum);
        output.y = sum;

        sum += currentDamageAmount;
        sum = Mathf.Clamp01(sum);
        output.z = sum;
        return output;
    }
}