using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动速度")]
    public float speed = 5f;
    [Header("生命值上限")]
    public int maxHealth = 5;
    [Header("受伤无敌时间")]
    public float timeInvincible = 2f;
    [Header("飞行道具")]
    public GameObject projectile;
    [Header("受伤音效")]
    public AudioClip hitClip;
    [Header("发射音效")]
    public AudioClip throwClip;

    public int Health { get { return currentHealth; } }
    private int currentHealth;

    private bool isInvincible = false;
    private float invincibleTimer = 0f;

    private float horizontal;
    private float vertical;

    private Vector2 lookDirection = new(0, 1);

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private AudioSource footstepSource;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        footstepSource = GetComponents<AudioSource>()[0];
        audioSource = GetComponents<AudioSource>()[1];

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 move = new(horizontal, vertical);

        if (move.magnitude > 0 && !footstepSource.isPlaying)
        {
            footstepSource.UnPause();
        }
        if (Mathf.Approximately(move.magnitude, 0f) && footstepSource.isPlaying)
        {
            footstepSource.Pause();
        }

        // 设置动画跑动还是静止
        animator.SetFloat("Speed", move.magnitude);
        // 设置动画方向
        if (!Mathf.Approximately(move.x, 0f) || !Mathf.Approximately(move.y, 0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            // 从玩家位置向玩家朝向发出1单位的射线，该射线应碰撞到NPC图层
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.5f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            // 如果射线的碰撞存在
            if (hit.collider != null)
            {
                NonPlayerCharacter nonPlayerCharacter = hit.collider.gameObject.GetComponent<NonPlayerCharacter>();
                if (nonPlayerCharacter != null)
                {
                    nonPlayerCharacter.DisplayDialog();
                }
            }
        }

    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x += horizontal * speed * Time.deltaTime;
        position.y += vertical * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    // 改变生命值
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            else
            {
                animator.SetTrigger("Hit");
                isInvincible = true;
                invincibleTimer = timeInvincible;

                PlaySound(hitClip);
            }
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.Instance.SetValue(currentHealth / (float)maxHealth);
        Debug.Log("生命值变化：" + amount + "，当前：" + currentHealth + "/" + maxHealth);
    }

    // 发射飞行道具
    private void Launch()
    {
        PlaySound(throwClip);
        GameObject projectileObject = Instantiate(projectile, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        projectileObject.GetComponent<Projectile>().Launch(lookDirection);
        animator.SetTrigger("Launch");
    }

    // 播放音效
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

}
