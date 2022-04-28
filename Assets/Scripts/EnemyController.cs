using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("�ƶ��ٶ�")]
    public float speed = 3f;
    [Header("�˺�ֵ")]
    public int damage = 2;
    [Header("�Ƿ�ֱ�ƶ�")]
    public bool vertical = false;
    [Header("ת������ʱ��")]
    public float changeTime = 3f;
    [Header("��������")]
    public ParticleSystem smokeEffect;
    [Header("������Ч")]
    public AudioClip fixClip;

    private float timer;
    private int direction = 1;
    private bool broken = true;

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = changeTime;
            direction = -direction;
        }
    }

    private void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y += speed * Time.deltaTime * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x += speed * Time.deltaTime * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        rigidbody2d.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.ChangeHealth(-damage);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        // ֹͣ������Ч
        smokeEffect.Stop();

        // ֹͣ������Ч
        audioSource.Stop();

        // ����������Ч
        audioSource.PlayOneShot(fixClip);
    }

}
