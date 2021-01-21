using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth;
    private float curHealth;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth <= 0)
        {
            if (gameObject.tag == "Enemy")
            {
                GameController.EnemyCount--;
                curHealth = maxHealth;
                gameObject.SetActive(false);
            }

            if (gameObject.tag == "Player")
            {
                curHealth = maxHealth;
                UIController.instance.EndGame();
            }
        }
    }

    public void UpdateHealth(float amt)
    {
        curHealth += amt;
    }

    public void ResetHealth()
    {
        curHealth = maxHealth;
    }
}
