using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    Animator anim;
    private Rigidbody2D rd2d;

    private int count;

    private int lives;

    public float speed;
    public float jumpForce;

    public Text score;

    public Text winText;

    public Text livesText;

    public Text loseText;

    public Text countText;

    public Text countDownText;

    public Text restartText;

    private int scoreValue = 0;

    public bool isGrounded;
    public bool isSpeedUp;
    public bool restart;
    private bool facingRight = true;

    private float baseSpeed;
    public float powerUpTimer;
    public float startPowerUpTimer;
    public float startTime = 70.0F;
    

    bool isLevel2;

    public Transform startMarker;
    public AudioClip musicClip;

    private AudioSource musicSource;

    public AudioClip coinPickUp;

    public AudioClip jumpSound;

    // Start is called before the first frame update

    void Awake()
    {
        Time.timeScale = 1f; 
    }

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        anim = GetComponent<Animator>();

        lives = 3;
        winText.text = "";
        loseText.text = "";
        SetCountText();

        baseSpeed = 10;
        startPowerUpTimer = powerUpTimer;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("escape"))
        {
            Debug.Log("quit");
            Application.Quit();
        }
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        SetCountText();

        
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement <0)
        {
            Flip();
        }

    }

    void PowerUpCheck()
    {
        if (!isSpeedUp)
        {
            speed = baseSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Powerup"))
        {
            if (other.GetComponent<PowerUpsScript>().type == "SpeedUp")
            {
                isSpeedUp = true;
                startPowerUpTimer = powerUpTimer;
                speed += 15;
                Debug.Log("Add Speed");
            }
            other.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            musicSource.PlayOneShot(coinPickUp);
            Destroy(collision.collider.gameObject);
            
        }
        if (collision.collider.tag == "Enemy")
        {
            Debug.Log("Hit");
            lives = lives - 1;
            Destroy(collision.collider.gameObject);
            collision.gameObject.SetActive(false);
            SetLivesText();
            SetCountText();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isGrounded = true;

            if(Input.GetKey(KeyCode.W))
            {
                anim.SetInteger("State", 3);
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                musicSource.PlayOneShot(jumpSound);

            }
            isGrounded = false;

            if (Input.GetKeyUp (KeyCode.W))
            {
                anim.SetInteger("State", 0);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown (KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp (KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            anim.SetInteger("State", 0);
        }

        startPowerUpTimer -= Time.deltaTime;
        startTime -= Time.deltaTime;
        countDownText.text = startTime.ToString("F0");

        if (startPowerUpTimer <= 0)
        {
            isSpeedUp = false;
            
        }
       
        if (startTime <=0)
        {
            restartText.text = "Time's up! Game Over! Press R to Restart!";
            restart = true;
        }

        if (restart)
        {
           Time.timeScale = 0f;
           
              if (Input.GetKeyDown(KeyCode.R))
              {
                Destroy(this.gameObject);
                restart = false;
                SceneManager.LoadScene("SampleScene");
              }
            
            PowerUpCheck();
        }
    }

    void SetCountText ()
    {
        score.text = "Score: " + scoreValue.ToString();

        if (scoreValue >= 4 && !isLevel2)
        {
            Debug.Log("Stage2");
            transform.position = new Vector2(startMarker.position.x, startMarker.position.y);
            lives = 3;
            scoreValue = 0;
            isLevel2 = true;

        }

        if (scoreValue >= 8 && isLevel2 == true)
        {
            winText.text = "You win! Game created by Vivianna Hui!";
            musicSource.PlayOneShot(musicClip);
        }
    }
    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            loseText.text = "You Lose!";
            Destroy(gameObject);
            restart = true;
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
