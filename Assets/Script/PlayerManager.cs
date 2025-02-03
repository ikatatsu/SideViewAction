using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] LayerMask   blockLayer;
    [SerializeField] private float jumpPower = 5.0f;
    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT
    }

    public bool isDead = false;

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    Rigidbody2D rigidbody2D;
    float speed;
    Animator animator;
    //SE
    [SerializeField] AudioClip getItemSE;
    [SerializeField] AudioClip jumpSE;
    [SerializeField] AudioClip stampSE;
    AudioSource audioSource;

    private int jumpCount = 0;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDead)
        {
            return;
        }
        // スペースが押されたらジャンプさせる
        //ジャンプ
        if (Input.GetKeyDown("space"))
        {
            Jump();
        }
    }

    void Jump()
    {
        if (jumpCount >= 2) return;     // ジャンプ2回まで
        //上に力を加える
        rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        jumpCount++;
        audioSource.PlayOneShot(jumpSE);
        animator.SetBool("isJumping", true);
        animator.SetBool("isRunning", false);        // 走行状態OFF

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }

        if (collision.gameObject.tag == "Trap")
        {
            Debug.Log("Game Over");
            PlayerDeath();
            gameManager.GameOver();
        }

        if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("Clear");
            gameManager.GameClear();
        }

        if (collision.gameObject.tag == "Item")
        {
            //アイテム取得
            audioSource.PlayOneShot(getItemSE);
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }

        if (collision.gameObject.tag == "Enemy")
        {
            EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();
            if (this.transform.position.y + 0.2f > enemy.transform.position.y)
            {
                // 上から踏んだら敵を削除
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                Jump();
                audioSource.PlayOneShot(stampSE);
                enemy.DestroyEnemy();
            }
            else
            {
                //横からぶつかったら
                PlayerDeath();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // ステージ接触時
        if (other.gameObject.CompareTag("Stage"))
        {
            Debug.Log("On Stage");
            jumpCount = 0;                              // ジャンプ回数リセット
            animator.SetBool("isJumping", false);       // ジャンプ状態OFF
            animator.SetBool("isRunning", true);        // 走行状態ON
        }
    }

    void PlayerDeath()
    {
        isDead = true;
        rigidbody2D.velocity = new Vector2 (0, 0);
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        animator.SetBool("isJumping", false);           // ジャンプ状態解除
        animator.Play("PlayerDeathAnimation");
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Destroy(boxCollider);
        gameManager.GameOver();
    }
}
