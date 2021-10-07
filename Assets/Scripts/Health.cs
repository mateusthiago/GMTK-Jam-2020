using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField]public int currHealth;
    [SerializeField] Color damageColor;
    Color originalColor;
    [SerializeField] float flashTime;
    SpriteRenderer sprite;
    [SerializeField] UnityEvent DeathEvent;
    public bool isPlayer;    

    void Start()
    {
        currHealth = maxHealth;

        sprite = GetComponentInChildren<SpriteRenderer>();
        originalColor = sprite.color;
        if (isPlayer)
        {
            UI_Controller.instance?.UpdateHealthBar((float)currHealth / (float)maxHealth);            
        }
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        if (isPlayer) UI_Controller.instance?.UpdateHealthBar((float)currHealth / (float)maxHealth);
        StartCoroutine(FlashDamage());
        if (currHealth <= 0) DeathEvent.Invoke();
    }

    private IEnumerator FlashDamage()
    {        
        sprite.color = damageColor;

        float timer = 0;
        float t = 0;

        while (t < 1)
        {
            if (!sprite) break;
            t = timer / flashTime;
            sprite.color = Color.Lerp(damageColor, originalColor, t);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
