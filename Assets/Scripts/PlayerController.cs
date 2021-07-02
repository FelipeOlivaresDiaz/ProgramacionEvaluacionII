using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void notifyenterdoor(int color, string id);
    public static event notifyenterdoor ondoorenter;

    public delegate void notifyexitdoor(int color, string id);
    public static event notifyexitdoor ondoorexit;

    public delegate void bonus();
    public static event bonus bonuslink;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
    Collider2D col;
    public float moveSpeed = 3f;
    public float jumpForce = 10f;
    public bool eKEnable = false;
    bool canMove;
    public delegate void FNotify();
    public static event FNotify OnPlayerDeath;
    public GameObject[] puertas;
    List<string> Detective = new List<string>();
    public int idPuertas;
    bool inside;
    string MemeDoor;
    int DoorColor;
    string[] ching;
    int chong;
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

    bool CanEnterDoor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Door>())
        {
            MemeDoor = collision.GetComponent<Door>().lol;
            CanEnterDoor = true;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Door>())
        {
            DoorColor = collision.GetComponent<Door>().color;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Door>())
        {
            CanEnterDoor = false;
        }

    }
    public void GameManager_OnGameplay()
    {
        canMove = true;
        anim.speed = 1;
    }

    public void GameManager_OnRoundOver()
    {
        canMove = false;
    }

    public void GameManager_OnGameOver()
    {
        canMove = false;
        anim.speed = 0;
    }
    public Color[] xd = new Color[3];

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        Vector2 targetVelocity = Vector2.right * x * moveSpeed;
        targetVelocity.y = rb.velocity.y;

        rb.velocity  = targetVelocity; 
        sprite.flipX = x < 0;
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void Jump()
    {
        if (Mathf.Abs(rb.velocity.y) > 0f) return;

        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        
    }
    // Update is called once per frame
    void Update()
    {
        if(canMove == true && inside == false) 
        {
            MovePlayer();
            if (Input.GetButtonDown("Jump"))
                Jump();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && inside == true && eKEnable == false)
        {
            int numintem = 0;
            inside = false;
            ondoorexit?.Invoke(DoorColor, MemeDoor);
            rb.constraints = RigidbodyConstraints2D.None;
            sprite.enabled = !sprite.enabled;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            Color actualcolor = puertas[idPuertas].gameObject.GetComponent<Door>().GetComponent<SpriteRenderer>().color;
            foreach (var item in xd)
            {
                if (actualcolor == item)
                {
                    if (numintem == 2) { numintem = 0; } else { numintem++; }
                    puertas[idPuertas].gameObject.GetComponent<Door>().GetComponent<SpriteRenderer>().color = xd[numintem];
                }
                numintem++;
            }

            /*puertas[idPuertas].transform.GetChild(1).gameObject.SetActive(true);
            int i = 0;
            foreach (var item in puertas)
            {
                if (actualcolor == item.gameObject.GetComponent<Door>().GetComponent<SpriteRenderer>().color)
                {
                    puertas[i].transform.GetChild(1).gameObject.SetActive(true);
                    eKEnable = true;
                }
                i++;
            }
        */
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && inside == false && CanEnterDoor)
        {
            inside = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            sprite.enabled = !sprite.enabled;
            ondoorenter?.Invoke(DoorColor, MemeDoor);
            Detective.Add(MemeDoor);
            ching = Detective.ToArray();
            chong = Detective.Count;

            if (chong - 7 >= 0)
            {
                if (ching[chong - 8] == "O" && ching[chong - 7] == "L" && ching[chong - 6] == "I" && ching[chong - 5] == "V" && ching[chong - 4] == "A" && ching[chong - 3] == "R" && ching[chong - 2] == "E" && MemeDoor == "S")
                {
                    //OnJackpot?.Invoke();
                }
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<enemy>())
        {
            Gamemanager.Instance.gameState = GameState.GameOver;
            OnPlayerDeath.Invoke();
        }
    }
}
