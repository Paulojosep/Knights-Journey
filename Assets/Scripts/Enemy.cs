using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2;
    private int direction = 1; // -1 -> esquerda e 1 -> direita
    public int enemyHP = 20; // Vida do inimigo
    public int ammo = 3; //Balas
    private bool canShoot = true;
    public int coolDownTime = 15;
    public GameObject enemyFire; //fogo do inimigo
    public Transform PlayerPosition; // Posição do Player
    public float fireSpeed = 10;
    public Transform firePositionRight;
    public Transform firePositionLeft;
    private Rigidbody2D rigidbody;
    private SpriteRenderer sprite;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(moveSpeed*direction,rigidbody.velocity.y);

        float distance = Vector2.Distance(transform.position, PlayerPosition.position);

        if(distance < 3 && transform.position.x > PlayerPosition.position.x)
        {
            direction = -1;
        }else if(distance < 4 && transform.position.x < PlayerPosition.position.x)
        {
            direction = 1;
        }

        if(direction == 1)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }

        if(distance < 3 && canShoot)
        {
            animator.SetTrigger("shoot");
        }  
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Limiter")
        {
            direction *= -1;
            sprite.flipX = !sprite.flipX;
        }
        if (other.gameObject.tag == "Fire")
        {
            Destroy(other.gameObject);
            TakeDamage(5);
        }
    }

    void Shoot()
    {
        GameObject fire;
        if(direction == 1)
        {
            fire = Instantiate(enemyFire, firePositionRight.position, Quaternion.identity);
        }
        else
        {
            fire = Instantiate(enemyFire, firePositionLeft.position, Quaternion.identity);
            fire.GetComponent<SpriteRenderer>().flipX = true;
        }
        
        fire.GetComponent<Rigidbody2D>().velocity = new Vector2(fireSpeed * direction, 0);
        ammo--;
        if(ammo <= 0)
        {
            canShoot = false;
            Invoke("CoolDownShoot", coolDownTime);
        }
    }

    void CoolDownShoot()
    {
        canShoot = true;
    }

    public void TakeDamage(int damage)
    {
        enemyHP -= damage;
        animator.SetTrigger("hurt");
        if(enemyHP <= 0)
        {
            animator.SetBool("isDead", true);
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");

        Destroy(gameObject);
        
        
        this.enabled = false;
    }
}
