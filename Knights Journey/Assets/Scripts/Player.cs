using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement Variables")]
    public float moveSpeed = 5;
    public float jumpForce = 300;
    private int direction = 1;
    public Transform groundCheck; //Verificação do chao
    public float groundCheckRadius = 0.2f; // tamanho do grounCheck
    public bool grounded; // Verificar se foi enconstado ou nao
    public LayerMask whatIsGround;

    private Rigidbody2D rig;
    private Animator animator;
    private SpriteRenderer sprite;

    [Header("Fire Variables")]
    public GameObject firePrefab;
    public Transform firePositionRight;
    public Transform firePositionLeft;
    public float fireSpeed;

    [Header("Attack Variables")]
    public Transform attackCheck;
    public float radiusAttack;
    public LayerMask layerEnemy;
    float timeNextAttack;
    private int damage = 5;

    [Header("HP Variable")]
    public float playerHP = 100;
    public Slider hpBar;
    public GameObject gameOver;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); //Horizontal

        rig.velocity = new Vector2(h*moveSpeed, rig.velocity.y);
        animator.SetFloat("speed", Mathf.Abs(h));

        if (h > 0)
        {
            Flip(false);
        }  
        else if (h < 0)
        {
            Flip(true);
        }

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if(grounded && Input.GetButtonDown("Jump"))
        {
            rig.AddForce(Vector2.up * jumpForce);
        }

        animator.SetBool("grounded", grounded);

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("shoot");
            
        }

        if(timeNextAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Z) && rig.velocity == new Vector2(0, 0))
            {
                animator.SetTrigger("attack");
                timeNextAttack = 0.2f;
                //Attack();
            }
        }
        else
        {
            timeNextAttack -= Time.deltaTime;
        }

        if (rig.position.y <= -300f)
        {
            TakeDamage(damage);
            if(playerHP <= 0)
            {
                Die();
            }
        }

        hpBar.value = playerHP; // Barra de vida

    }

    // Metodo que faz o player virar para o lado
    void Flip(bool f) 
    {
        sprite.flipX = f;
        if (!f)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }


        attackCheck.localPosition = new Vector2(-attackCheck.localPosition.x, attackCheck.localPosition.y);
    }

    void Shoot()
    {
        GameObject fire;
        if (direction == 1)
        {
            fire = Instantiate(firePrefab, firePositionRight.position, Quaternion.identity);
        }
        else
        {
            fire = Instantiate(firePrefab, firePositionLeft.position, Quaternion.identity);
            fire.GetComponent<SpriteRenderer>().flipX = true;
        }

        fire.GetComponent<Rigidbody2D>().velocity = new Vector2(fireSpeed * direction, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "EnemyFire")
        {
            Destroy(other.gameObject);
            TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("hurt");
        playerHP -= damage;
        if(playerHP <= 0)
        {
            animator.SetBool("die", true);
        }
    }

    void Attack()
    {
        Collider2D[] enemiesAttack = Physics2D.OverlapCircleAll(attackCheck.position, radiusAttack, layerEnemy);
        foreach(Collider2D enemy in enemiesAttack)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
            Debug.Log(enemy.name);
        }
    }

    void Die()
    {
        Debug.Log("Player Die");
        Destroy(gameObject);
        EndGame();
        gameOver.SetActive(true);
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, radiusAttack);
    }

    public void EndGame()
    {
        Debug.Log("GAME OVER");
    }
}
