using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShield : MonoBehaviour
{
    // 몬스터와 충돌시 투명한 투사체를 날림. 그것과 부딪히면 뒤로 쭉 밀려남. 보스 제외. 보스는 안 밀려나고, 데미지만 입음. 
    // 몬스터가 붙는 자석 미사일 느낌
    // 홀수 강화: 쉴드 충전 수 증가
    // 짝수 강화: 쿨타임 20프로 감소 
    [SerializeField] private int FullShieldCount; // 최대 방패 수
    private int shieldCount; // 현재 방패 수

    private SpriteRenderer spriteRenderer;

    // 스킬데이터와 투사체 데이터는 계수를 세팅하기 위해 들고 있는 것.
    [SerializeField] private SkillData skillData;
    public PlayerProjectileData projectileData;

    // UI 관련 변수
    [SerializeField] private RectTransform ShieldCountUI;
    [SerializeField] private float ShieldCoolTime; // 초기 쿨타임
    [SerializeField] private Text ShieldCountText; // 방패 수 표시 텍스트

    // yield instruction은 Start에 미리 만들어서 GC 줄이기.
    private WaitForSeconds[] waitInstructions;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shieldCount = 0; // 초기 방패 수 설정
        SetShieldVisibility(false);

        // 쿨타임 배열 초기화
        InitializeCoolTimeWaits();
        SetSkillData();
    }

    private void InitializeCoolTimeWaits()
    {
        waitInstructions = new WaitForSeconds[5];
        for (int i = 0; i < waitInstructions.Length; i++)
        {
            waitInstructions[i] = new WaitForSeconds(ShieldCoolTime * (1 - 0.2f * i));
        }
    }

    private void SetSkillData()
    {
        skillData.Damage = projectileData.Damage;
        skillData.baseCount = 1;
        skillData.baseCoolTime = ShieldCoolTime;
    }

    private void Start()
    {
        SetFullShieldCount();
        StartCoroutine(AddShieldCoroutine());
    }

    private void Update()
    {
        // 화면 상의 방패 위치로 이동시키기.
        ShieldCountUI.position = Camera.main.WorldToScreenPoint(transform.position);
     
        // 최대 쉴드 수 갱신 및 UI 업데이트. 업글할 때, 델리게이트로 호출되게 최적화 가능
        SetFullShieldCount();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            if (shieldCount > 0) // 쉴드가 남아있는지 확인
            {
                // PoolManager에서 PlayerShieldProj를 가져와서 사용
                PoolManager.Instance.MakeObj(PoolObjType.PlayerShieldProj, collision.gameObject.transform.position);

                // 쉴드 수 감소
                shieldCount--;

                // 쉴드 수가 0인 경우 SpriteRenderer 색상 변경
                if (shieldCount == 0)
                {
                    SetShieldVisibility(false); // 쉴드 비가시화
                }
            }
        }
    }

    private IEnumerator AddShieldCoroutine()
    {
        while (true)
        {
            // 쉴드 수가 최대인 경우 대기
            while (shieldCount >= FullShieldCount)
            {
                yield return null; // 쉴드가 최대일 때는 다음 프레임까지 대기
            }

            // 쿨타임 레벨을 가져오기
            int CoolTimeLV = EnhancementManager.Instance.SkillLV[2] / 2;

            // 쿨타임 대기
            if (CoolTimeLV < waitInstructions.Length)
            {
                yield return waitInstructions[CoolTimeLV];
            }
            else
            {
                yield return waitInstructions[waitInstructions.Length - 1];
            }

            // 방패 추가
            AddShield();
            SetShieldVisibility(true); // 쉴드 가시화
        }
    }

    private void SetShieldVisibility(bool isVisible)
    {
        Color color = spriteRenderer.color;
        color.a = isVisible ? 1f : 0f; // 가시성에 따라 알파값 설정
        spriteRenderer.color = color; // 색상 업데이트
    }


    private void SetFullShieldCount()
    {
        FullShieldCount = (EnhancementManager.Instance.SkillLV[2] + 3) / 2;
        SetShieldCountUI();
    }

    private void AddShield()
    {
        if (shieldCount < FullShieldCount)
        {
            shieldCount++; // 방패 수 증가
        }
    }

    private void SetShieldCountUI()
    {
        ShieldCountText.text = shieldCount.ToString() + " / " + FullShieldCount.ToString(); // 방패 수 텍스트 업데이트
    }
}
