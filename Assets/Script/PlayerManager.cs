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
        // �X�y�[�X�������ꂽ��W�����v������
        //�W�����v
        if (Input.GetKeyDown("space"))
        {
            Jump();
        }
    }

    void Jump()
    {
        if (jumpCount >= 2) return;     // �W�����v2��܂�
        //��ɗ͂�������
        rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        jumpCount++;
        audioSource.PlayOneShot(jumpSE);
        animator.SetBool("isJumping", true);
        animator.SetBool("isRunning", false);        // ���s���OFF

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
            //�A�C�e���擾
            audioSource.PlayOneShot(getItemSE);
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }

        if (collision.gameObject.tag == "Enemy")
        {
            EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();
            if (this.transform.position.y + 0.2f > enemy.transform.position.y)
            {
                // �ォ�瓥�񂾂�G���폜
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                Jump();
                audioSource.PlayOneShot(stampSE);
                enemy.DestroyEnemy();
            }
            else
            {
                //������Ԃ�������
                PlayerDeath();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // �X�e�[�W�ڐG��
        if (other.gameObject.CompareTag("Stage"))
        {
            Debug.Log("On Stage");
            jumpCount = 0;                              // �W�����v�񐔃��Z�b�g
            animator.SetBool("isJumping", false);       // �W�����v���OFF
            animator.SetBool("isRunning", true);        // ���s���ON
        }
    }

    void PlayerDeath()
    {
        isDead = true;
        rigidbody2D.velocity = new Vector2 (0, 0);
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        animator.SetBool("isJumping", false);           // �W�����v��ԉ���
        animator.Play("PlayerDeathAnimation");
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Destroy(boxCollider);
        gameManager.GameOver();
    }
}
