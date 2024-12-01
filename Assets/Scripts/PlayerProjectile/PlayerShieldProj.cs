using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldProj : MonoBehaviour
{
    public PlayerProjectileData projectileData;
    [SerializeField] private float FireForce;

    private List<Transform> enemyTransforms = new List<Transform>(); // 적의 Transform을 저장할 리스트

    private Rigidbody2D RB;
    private ParticleSystem PS;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        PS = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        RB.AddForce(Vector2.up * FireForce, ForceMode2D.Impulse);
        PS.Play();
    }

    private void OnDisable()
    {
        foreach (Transform enemyTransform in enemyTransforms)
        {
            if (enemyTransform != null)
            {
                enemyTransform.gameObject.SetActive(false);
            }
        }

        enemyTransforms.Clear();
        PS.Clear();
    }

    private void FixedUpdate()
    {
        // 리스트에 있는 모든 적의 위치를 업데이트
        foreach (Transform enemyTransform in enemyTransforms)
        {
            if (enemyTransform != null)
            {
                enemyTransform.position = transform.position; 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletBoarder"))
        {
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<DownMovement>().SetStopUse(true);

            // 적의 Transform을 리스트에 추가
            enemyTransforms.Add(collision.transform);
        }
    }
}
