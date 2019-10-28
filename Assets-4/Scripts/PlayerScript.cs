using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    private int count;

    private int lives;

    public float speed;

    public Text score;

    public Text winText;

    public Text livesText;

    public Text loseText;

    private int scoreValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        count = 0;
        lives = 3;
        winText.text = "";
        loseText.text = "";
        SetScoreValue();
        SetLivesText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            SetScoreValue();
        }
        if (collision.collider.tag == "Enemy")
        {
            lives = lives - 1;
            Destroy(collision.collider.gameObject);
            SetLivesText();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);

            }
        }
    }

    void SetScoreValue ()
    {
        score.text = "Score: " + scoreValue.ToString();

        if (scoreValue >= 4)
        {
            winText.text = "You win! Game created by Vivianna Hui!";
        }
    }
    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            loseText.text = "You Lose!";
        }

    }
}
