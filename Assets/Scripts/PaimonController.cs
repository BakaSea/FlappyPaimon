using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaimonController : MonoBehaviour {

    private static float upSpeed = 4.3f;
    private float maxHeight;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource clickAudio;
    private AudioSource deadAudio;
    private AudioSource hintAudio;
    private GameStatus prevGameStatus;
    private bool upStatus;
    private Sprite downSprite;
    private Sprite upSprite;
    private Sprite deadSprite;

    // Start is called before the first frame update
    void Start() {
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        maxHeight = screenPos.y;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody.gravityScale = 0.0f;
        prevGameStatus = GameController.status;
        upStatus = false;
        Sprite[] spriteList = Resources.LoadAll<Sprite>("PaimonSprite");
        downSprite = spriteList[0];
        upSprite = spriteList[1];
        deadSprite = spriteList[2];
        AudioSource[] audioList = GetComponents<AudioSource>();
        clickAudio = audioList[1];
        deadAudio = audioList[2];
        hintAudio = audioList[3];
    }

    private void changeUpStatus() {
        upStatus = true;
        spriteRenderer.sprite = upSprite;
        transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    private void changeDownStatus() {
        upStatus = false;
        spriteRenderer.sprite = downSprite;
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0.0f, 0.0f, -10.0f));
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (GameController.status == prevGameStatus) {
                switch (GameController.status) {
                    case GameStatus.START: {
                            GameController.run();
                            animator.enabled = false;
                            rigidBody.gravityScale = 1.0f;
                            clickAudio.Play();
                            if (transform.position.y < maxHeight) {
                                rigidBody.velocity += Vector2.up * upSpeed;
                                changeUpStatus();
                            }
                            break;
                        }
                    case GameStatus.OVER: {
                            GameController.start();
                            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                            rigidBody.gravityScale = 0.0f;
                            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                            animator.enabled = true;
                            break;
                        }
                    case GameStatus.RUN: {
                            clickAudio.Play();
                            if (transform.position.y < maxHeight) {
                                rigidBody.velocity += Vector2.up * upSpeed;
                                changeUpStatus();
                            }
                            break;
                        }
                    case GameStatus.WINOVER: {
                            GameController.start();
                            break;
                        }
                }
            }
        } else {
            if (GameController.status == GameStatus.RUN) {
                if (upStatus && rigidBody.velocity.y < 0) {
                    changeDownStatus();
                }
            } else if (GameController.status == GameStatus.OVER && prevGameStatus == GameStatus.RUN) {
                spriteRenderer.sprite = deadSprite;
                rigidBody.constraints = RigidbodyConstraints2D.None;
            } else if (GameController.status == GameStatus.WIN && prevGameStatus == GameStatus.RUN) {
                transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                rigidBody.gravityScale = 0.0f;
                rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                animator.enabled = true;
            }
        }
        prevGameStatus = GameController.status;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (GameController.status == GameStatus.RUN) {
            if (collision.gameObject.tag == "GenStone") {
                GameController.score += 10;
                Debug.LogFormat("Goal point! Now {0} points.", GameController.score);
                hintAudio.Play();
                Destroy(collision.gameObject);
            } else if (collision.gameObject.tag == "Pipe") {
                GameController.score++;
                Debug.LogFormat("Goal point! Now {0} points.", GameController.score);
            }
            if (GameController.score >= 100) {
                Debug.Log("Win!");
                GameController.win();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (GameController.status == GameStatus.RUN) {
            if (collision.gameObject.name != "Sky") {
                Debug.Log("Game Over!");
                deadAudio.Play();
                GameController.over();
            }
        }
    }

}
