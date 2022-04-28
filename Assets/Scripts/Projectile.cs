using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [Header("���е��ߵķ�����")]
    public int force = 300;
    [Header("���")]
    public float distance = 20f;

    private Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > distance)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("���У�" + collision.gameObject);
        EnemyController enemyController = collision.collider.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.Fix();
        }

        Destroy(gameObject);
    }

}
