using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�ƶ��ٶ�")]
    public float speed = 5f;
    [Header("����ֵ����")]
    public int maxHealth = 5;
    [Header("�����޵�ʱ��")]
    public float timeInvincible = 2f;
    [Header("���е���")]
    public GameObject projectile;
    [Header("������Ч")]
    public AudioClip hitClip;
    [Header("������Ч")]
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

        // ���ö����ܶ����Ǿ�ֹ
        animator.SetFloat("Speed", move.magnitude);
        // ���ö�������
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
            // �����λ������ҳ��򷢳�1��λ�����ߣ�������Ӧ��ײ��NPCͼ��
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.5f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            // ������ߵ���ײ����
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

    // �ı�����ֵ
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
        Debug.Log("����ֵ�仯��" + amount + "����ǰ��" + currentHealth + "/" + maxHealth);
    }

    // ������е���
    private void Launch()
    {
        PlaySound(throwClip);
        GameObject projectileObject = Instantiate(projectile, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        projectileObject.GetComponent<Projectile>().Launch(lookDirection);
        animator.SetTrigger("Launch");
    }

    // ������Ч
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

}
