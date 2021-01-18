using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int health = 1000;
    public GameObject Win;
    public void TakeDamage(int damage)
    {

        health -= damage;

        if(health <= 200)
        {
            GetComponent<Animator>().SetBool("isAttack2", true);
        }

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        Win.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Fire")
        {
            Destroy(other.gameObject);
            TakeDamage(15);
        }
    }
}
