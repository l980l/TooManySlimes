using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    // 플레이어 위치에 따라 다른 각도로 레이저를 날리는 스킬. 
    // 레이저는 벽에 튕기며, Z축 회전량을 방향으로 사용함.
    // 홀수 강화: 탄 수 증가
    // 짝수 강화: 쿨타임 20프로 감소 
    [SerializeField] private float laserCoolTime;
    [SerializeField] private float laserSpacing;   // 레이저 간의 간격
    [SerializeField] private float minXAngle; // 플레이어 X좌표가 minX일때 각도
    [SerializeField] private float maxXAngle; // 플레이어 X좌표가 maxX일때 각도
    private float minX = -2f;
    private float maxX = 2f;

    // 스킬데이터와 투사체 데이터는 계수를 세팅하기 위해 들고 있는 것.
    [SerializeField] private SkillData skillData;
    public PlayerProjectileData projectileData;

    // yield instruction은 Start에 미리 만들어서 GC 줄이기.
    private WaitForSeconds[] waitInstructions;

    private void Start()
    {
        InitializeWaitInstructions();
        StartCoroutine(FireLaserRoutine());
        SetSkillData();
    }
    private void InitializeWaitInstructions()
    {
        // 쿨타임에 따른 대기 시간 배열 초기화
        waitInstructions = new WaitForSeconds[5];
        for (int i = 0; i < waitInstructions.Length; i++)
        {
            waitInstructions[i] = new WaitForSeconds(laserCoolTime * (1 - i * 0.2f));
        }
    }

    private void SetSkillData()
    {
        skillData.Damage = projectileData.Damage;
        skillData.baseCount = 1;
        skillData.baseCoolTime = laserCoolTime;
    }

    private IEnumerator FireLaserRoutine()
    {
        while (true) // 무한 루프를 통해 지속적으로 발사
        {
            FireLaser();

            // 나중에 강화레벨은 따로 함수로 갱신하게 만들 수 있음. 지금은 일단 빠르게 만들기
            int CoolTimeLV = (EnhancementManager.Instance.SkillLV[1]) / 2;

            int coolTimeLevel = EnhancementManager.Instance.SkillLV[1] / 2;
            yield return waitInstructions[coolTimeLevel];
        }
    }

    private void FireLaser()
    {
        // 홀수 강화: 탄 수 증가
        int LaserCount = (EnhancementManager.Instance.SkillLV[1] + 3) / 2;

        for (int i = 0; i < LaserCount; i++)
        {
            // x축으로 일정 간격을 두고 총알 생성
            float offset = (i - LaserCount / 2) * laserSpacing; // 중심을 기준으로 간격 조정
            Vector3 spawnPosition = transform.position + new Vector3(offset, 0, 0);

            // 플레이어의 x 좌표에 따라 각도를 계산합니다.
            float x = transform.position.x + offset;
            float angle = CalculateAngle(x);

            // 레이저 오브젝트를 생성합니다.
            GameObject laser = PoolManager.Instance.MakeObj(PoolObjType.PlayerLaser, spawnPosition);

            // z축 회전량을 설정합니다.
            laser.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        SoundManager.Instance.PlaySFX(SFX.reflect, transform.position);
    }

    private float CalculateAngle(float x)
    {
        float normalizedValue = (x - minX) / (maxX - minX);
        return Mathf.Lerp(minXAngle, maxXAngle, normalizedValue);
    }
}
