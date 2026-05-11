using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] int value = 25;

    int currentHealth;

    private void Start()
    {
        int currentWave = FindFirstObjectByType<UIScript>().GetWave();
        if (currentWave > 15)
        {
            maxHealth += (currentWave - 10) * (currentWave - 10);
        }
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        FindFirstObjectByType<UIScript>().AddMoney(value);
        Destroy(gameObject);
    }
}