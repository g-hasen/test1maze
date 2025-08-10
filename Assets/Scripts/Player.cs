using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public CharacterController cc;
    public float speed = 5f;
    public float rotate = 50f;
    public float jumpForce = 5f;

    Vector3 velocity;
    float gravity = -30f;
    bool isGrounded;
    bool isGameOver = false;

    public GameObject gameOver;

    private void Start()
    {
        cc = GetComponent<CharacterController>();

        if (gameOver != null)
            gameOver.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver) return;

        isGrounded = cc.isGrounded;

        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        transform.Rotate(0, Horizontal * rotate * Time.deltaTime, 0);

        Vector3 forward = transform.forward * Vertical * speed * Time.deltaTime;

        cc.Move(forward);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        Jump();
    }

    void Jump()
    {
        if (isGameOver) return;

        if ((Input.GetKeyDown(KeyCode.Space) && isGrounded))
        {
            velocity.y = jumpForce;
        }

        velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Danger"))
        {
            isGameOver = true;

            cc.enabled = false;
            this.enabled = false;

            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.GameOver();
            }
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.GameOver();
        }
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}