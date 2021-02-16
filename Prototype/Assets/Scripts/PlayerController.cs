using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 5;
    private float moveInput;
    public float jumpHeight = 7;
    private bool isJumping = false;

    Rigidbody2D rb2d;

    Vector2 startPosition = new Vector2(-11.0f, -3.7f);

    bool completeFirstStage = false;
    bool startSecondStage = false;
    bool completeSecondStage = false;
    bool death = false;
    bool gameComplete = false;

    Scene scene;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        scene = SceneManager.GetActiveScene();
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(moveInput * movementSpeed, rb2d.velocity.y);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb2d.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            rb2d.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb2d.velocity = Vector2.up * jumpHeight;
            isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.X) && startSecondStage)
        {
            StartCoroutine(Obstacles.ShowObstacles());
        }

        if (completeFirstStage)
        {
            Obstacles.RemoveObstacles();
            startSecondStage = true;
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("LevelThree"))
        {
            gameComplete = true;
        }

    }

    void ResetPlayer()
    {
        this.transform.position = startPosition;
        death = false;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(150, 15, 200, 50), "The goal is to reach the goal!");
        GUI.Label(new Rect(150, 35, 200, 50), "Press X to quick see obstacles");

        if (GUI.Button(new Rect(10, 10, 100, 100), "Reset Player!"))
        {
            ResetPlayer();
        }

        if (completeFirstStage)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 50), "STAGE ONE COMPLETE!"))
            {
                ResetPlayer();
                Time.timeScale = 1.0f;
                completeFirstStage = false;
            }
        }

        if (completeSecondStage && !gameComplete)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 50), "LEVEL COMPLETE!"))
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        if (death)
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 50), "YOU DIED!"))
            {
                ResetPlayer();
                Time.timeScale = 1.0f;
                completeFirstStage = false;
            }

        if (completeSecondStage && gameComplete)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 50), "GAME COMPLETE"))
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("Obstacles"))
        {
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            death = true;
        }

        if (collision.gameObject.CompareTag("Goal"))
        {
            completeFirstStage = true;
            Time.timeScale = 0f;

            if (collision.gameObject.CompareTag("Goal") && startSecondStage)
            {
                completeFirstStage = false;
                completeSecondStage = true;
                Time.timeScale = 0f;
            }
        }
    }
}
