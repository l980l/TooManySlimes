using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int FullHP;
    [SerializeField] private int HP;

    [SerializeField] private float DamageDelay;     // 피격 후 무적 시간
    private float curDamageDelay;  // 마지막 피격 후 지난 시간

    private SpriteRenderer spriteRenderer;
    private float flashDuration; // 색상 변경 지속 시간
    private bool isFlashing;    // 현재 플래시 상태

    // HP UI
    [SerializeField] private RectTransform HPUI;
    [SerializeField] private Slider HPSlider;
    [SerializeField] private Text HPText;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        HP = FullHP;                    // 초기 체력 세팅 
        curDamageDelay = DamageDelay;   // 피격 맞을 수 있게 세팅
        isFlashing = false;             // 초기 플래시 상태

        SetHPUI();
    }

    private void Update()
    {
        curDamageDelay += Time.deltaTime;

        if (isFlashing)
        {
            flashDuration -= Time.deltaTime;
            if (flashDuration <= 0)
            {
                spriteRenderer.color = Color.white; // 원래 색상으로 복원
                isFlashing = false; // 플래시 상태 종료
            }
        }

        // 화면 상의 플레이어 위치로 이동시키기.
        HPUI.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void SetHPUI()
    {
        if (HP <= 0)
        {
            HP = 0;
        }

        HPText.text = HP.ToString();
        HPSlider.value = (float)HP / FullHP;
    }

    private void OnHit(int damage)
    {
        HP -= damage;

        if (HP <= 0)
            GameManager.Instance.PlayerDie();
        else
        {
            SoundManager.Instance.PlaySFX(SFX.playerHit, transform.position);
            FlashRed(); // 색상 변경 시작
        }
        
        SetHPUI();
    }

    private void FlashRed()
    {
        spriteRenderer.color = Color.red;
        flashDuration = 0.1f; // 색상 변경 지속 시간 설정
        isFlashing = true; // 플래시 상태 시작
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (curDamageDelay >= DamageDelay)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                curDamageDelay = 0;

                int damage = collision.gameObject.GetComponent<CommonEnemy>().GetAttackVal();
                OnHit(damage);
            }

            if (collision.gameObject.CompareTag("EnemyAttack"))
            {
                curDamageDelay = 0;

                int damage = collision.gameObject.GetComponent<RangerEnemyBullet>().GetAttackVal();
                OnHit(damage);
                collision.gameObject.SetActive(false);
            }
        }
    }
}
