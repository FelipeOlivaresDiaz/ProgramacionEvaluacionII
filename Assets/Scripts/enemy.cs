using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float walkSpeeed = 3f;

    [HideInInspector]
    public bool mustPatrol;
    private bool mustFlip;

    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask wallLayer;
    public LayerMask enemyLayer;
    public LayerMask groundLayer;
    public Collider2D bodyCollider;
    bool canMove;

    public delegate void FNotift();
    public static event FNotift OnEnemyDeath;

    private void OnDisable()
    {
        Gamemanager.OnGameplay -= GameManager_OnGameplay;
        Gamemanager.OnRoundOver -= GameManager_OnRoundOver;
        Gamemanager.OnGameOver -= GameManager_OnGameOver;
    }

    private void OnEnable()
    {
        Gamemanager.OnGameplay += GameManager_OnGameplay;
        Gamemanager.OnRoundOver += GameManager_OnRoundOver;
        Gamemanager.OnGameOver += GameManager_OnGameOver;
    }
    public void GameManager_OnWait()
    {
        canMove = false;
    }
    public void GameManager_OnGameplay()
    {
        canMove = true;
    }

    public void GameManager_OnRoundOver()
    {
        canMove = false;
    }

    public void GameManager_OnGameOver()
    {
        canMove = false;
    }

    public void Awake()
    {
        Gamemanager.enemyAmount++;
    }

    private void Start()
    {
        mustPatrol = true;
    }

    private void Update()
    {
        if (mustPatrol && canMove == true)
        {
            Patrol();
        }
    }
    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            mustFlip = !Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
        }
    }

    private void Patrol()
    {
        if(mustFlip || bodyCollider.IsTouchingLayers(enemyLayer) || bodyCollider.IsTouchingLayers(wallLayer))
        {
            Flip();
        }
        rb.velocity = new Vector2(walkSpeeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeeed *= -1;
        mustPatrol = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            OnEnemyDeath.Invoke();
            Destroy(gameObject);
        }
    }
}
