using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    private Animator playerAnimation;

    private Vector3 respawnPoint;
    public GameObject fallDetector;
    public TextMeshProUGUI scoreText;
    public HealthBar healthBar;

   

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        //scoreText = FindObjectOfType<TextMeshProUGUI>();
        respawnPoint = transform.position;
        scoreText.text = "Score: " + Scoring.totalScore;
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);



        direction = Input.GetAxis("Horizontal");

        if (direction > 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(0.2031774f, 0.2031774f);
            respawnPoint = transform.position;
        }
        else if (direction < 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(-0.2031774f, 0.2031774f);
        }
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }
        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
        }

        playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimation.SetBool("OnGround", isTouchingGround);

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if(collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        else if(collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            respawnPoint = transform.position;
        }
        else if(collision.tag == "PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            respawnPoint = transform.position;
        }
        else if(collision.tag == "FirstLevel")
        {
            SceneManager.LoadScene(0);
            respawnPoint = transform.position;
        }

        else if(collision.tag == "Crystal")
        {
            Scoring.totalScore += 1;
            scoreText.text = "Score: " + Scoring.totalScore;
            collision.gameObject.SetActive(false);
            speed += 0.5f;
        }
        else if (collision.tag == "Crystal 2")
        {
            Scoring.totalScore += 5;
            scoreText.text = "Score: " + Scoring.totalScore;
            collision.gameObject.SetActive(false);
            speed += 0.7f;
        }
        else if (collision.tag == "Crystal 3")
        {
            Scoring.totalScore += 10;
            scoreText.text = "Score: " + Scoring.totalScore;
            collision.gameObject.SetActive(false);
            speed += 0.9f;
        }

        else if ((collision.tag == "Player"))
        {
            Health.totalHealth = 0f;
            respawnPoint = transform.position;

        }



    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Spike")
        {
            healthBar.Damage(0.002f);
        }

        else if(collision.tag == "Enemy")
        {
            healthBar.Damage(0.005f);
        }

        else if ((collision.tag == "Player"))
        {
            Health.totalHealth = 0f;
            respawnPoint = transform.position;

        }

    }



}

