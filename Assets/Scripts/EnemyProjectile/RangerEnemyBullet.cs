using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerEnemyBullet : MonoBehaviour
{
    public EnemyProjectileData projectileData;
    [SerializeField] private float FireForce;

    protected Rigidbody2D RB;

    protected void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    virtual protected void OnEnable()
    {
        RB.AddForce(Vector2.down * FireForce, ForceMode2D.Impulse);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DownBoarder"))
        {
            gameObject.SetActive(false);
        }
    }

    public int GetAttackVal()  
    {
        return projectileData.Damage;
    }
}
