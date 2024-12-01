using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAttack : MonoBehaviour
{
    // 홀수 강화: 탄 수 증가
    // 짝수 강화: 쿨타임 20프로 감소 
    [SerializeField] private GameObject normalBulletPrefab;
    [SerializeField] private float normalBulletCoolTime;
    [SerializeField] private float bulletSpacing;   // 총알 간의 간격

    // 스킬데이터와 투사체 데이터는 계수를 세팅하기 위해 들고 있는 것.
    [SerializeField] private SkillData skillData;   
    public PlayerProjectileData projectileData;

    // yield instruction은 Start에 미리 만들어서 GC 줄이기.
    private WaitForSeconds[] waitInstructions;

    private void Start()
    {
        InitializeWaitInstructions();
        StartCoroutine(FireNormalBulletRoutine());
        SetSkillData();
    }

    private void InitializeWaitInstructions()
    {
        // 쿨타임에 따른 대기 시간 배열 초기화
        waitInstructions = new WaitForSeconds[5];
        for (int i = 0; i < waitInstructions.Length; i++)
        {
            waitInstructions[i] = new WaitForSeconds(normalBulletCoolTime * (1 - i * 0.2f));
        }
    }

    private void SetSkillData()
    {
        skillData.Damage = projectileData.Damage;
        skillData.baseCount = 1;
        skillData.baseCoolTime = normalBulletCoolTime;
    }

    private IEnumerator FireNormalBulletRoutine()
    {
        while (true) // 무한 루프를 통해 지속적으로 발사
        {
            FireNormalBullet();

            // 나중에 강화레벨은 따로 함수로 갱신하게 만들 수 있음. 지금은 일단 빠르게 만들기
            int CoolTimeLV = (EnhancementManager.Instance.SkillLV[0]) / 2;

            // 쿨타임 레벨이 배열의 범위를 초과하지 않도록 조정
            if (CoolTimeLV >= 0 && CoolTimeLV < waitInstructions.Length)
            {
                yield return waitInstructions[CoolTimeLV]; // 쿨타임만큼 대기
            }
        }
    }

    private void FireNormalBullet()
    {
        // 홀수 강화: 탄 수 증가
        int BulletCount = (EnhancementManager.Instance.SkillLV[0] + 3) / 2;

        for (int i = 0; i < BulletCount; i++)
        {
            // x축으로 일정 간격을 두고 총알 생성
            float offset = (i - BulletCount / 2) * bulletSpacing; // 중심을 기준으로 간격 조정
            Vector3 spawnPosition = transform.position + new Vector3(offset, 0, 0);
            // AddForce는 Awake에서 함.
            PoolManager.Instance.MakeObj(PoolObjType.PlayerBullet, spawnPosition);
        }
     
        SoundManager.Instance.PlaySFX(SFX.bullet, transform.position);
    }
}
