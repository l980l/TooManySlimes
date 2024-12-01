using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEnemy : MonoBehaviour    // 모든 몬스터가 사용할 스크립트
{
    [SerializeField] private EnemyData enemyData;

    private SpriteRenderer spriteRenderer;

    private float HP;   // 현재 체력. 최대 체력은 EnemyData에 있음.

    private float flashDuration; // 색상 변경 지속 시간
    private bool isFlashing; // 현재 플래시 상태

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        spriteRenderer.color = Color.white;
    }

    private void Init()
    {
        HP = enemyData.FullHP;  // 초기 체력 세팅 
        isFlashing = false; // 초기 플래시 상태
    }

    public int GetAttackVal()
    {
        return enemyData.AtkVal;
    }

    private void Update()
    {
        if (isFlashing)
        {
            flashDuration -= Time.deltaTime;
            if (flashDuration <= 0)
            {
                spriteRenderer.color = Color.white; // 원래 색상으로 복원
                isFlashing = false; // 플래시 상태 종료
            }
        }
    }

    private void OnHit(int damage)
    {
        HP -= damage;

        if (HP <= 0)
            gameObject.SetActive(false);
        else
            FlashRed(); // 색상 변경 시작
    }

    private void FlashRed()
    {
        spriteRenderer.color = Color.red;
        flashDuration = 0.1f; // 색상 변경 지속 시간 설정
        isFlashing = true; // 플래시 상태 시작
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            PlayerBullet bullet = collision.gameObject.GetComponent<PlayerBullet>();
            OnHit(bullet.projectileData.Damage);
        }
        if (collision.gameObject.CompareTag("PlayerLaser"))
        {
            PlayerLaserProj laser = collision.gameObject.GetComponent<PlayerLaserProj>();
            OnHit(laser.projectileData.Damage);
        }
        if (collision.gameObject.CompareTag("PlayerShieldProj"))
        {
            PlayerShieldProj shield = collision.gameObject.GetComponent<PlayerShieldProj>();
            OnHit(shield.projectileData.Damage);
            SoundManager.Instance.PlaySFX(SFX.magneticHit, transform.position);
        }
    }
}
