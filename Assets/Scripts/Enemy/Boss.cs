using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // 2가지 패턴 돌아가면서 사용하기.
    // 1. 0~180도를 왕복하며 총 발사. 레이저 스킬 응용
    // 2. 일정 각도로 벌려서 여러발을 한번에 다다닥 발사. 기본 공격 응용
    // 일단 스킬은 보여주기 위해 2개를 순차적으로 사용.
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float ComboCoolTime = 3f;   // 콤보 사이의 쿨타임
    [SerializeField] private float Combo1Time = 3f;   // 콤보1 총 시간
    [SerializeField] private float Combo2Time = 3f;   // 콤보2 총 시간
    [SerializeField] private float Combo1Interval = 0.5f; // Combo1 발사 간격
    [SerializeField] private float Combo2Interval = 0.5f; // Combo2 발사 간격
    [SerializeField] private float BulletSpacing = 0.2f; // 총알 간격

    private float combo1Timer;             // Combo1 타이머
    private float combo2Timer;             // Combo2 타이머
    private float SinceLastComboBegin;   // 마지막 콤보 시작으로부터의 시간
    private float SinceLastComboEnd;    // 마지막 콤보 종료로부터의 시간
    private int ComboNum;           
    private bool isOnCombo;             // 콤보 중인지

    private void Awake()
    {
        SinceLastComboEnd = 0f; // 초기화
        ComboNum = 0;
        combo1Timer = 0f; // 초기화
        combo2Timer = 0f; // 초기화
        isOnCombo = false;
    }

    private void OnDisable()
    {
        GameManager.Instance.BossDie();
    }

    private void Update()
    {
        if(!isOnCombo)
        {
            SinceLastComboEnd += Time.deltaTime; // 경과 시간 증가

            if (SinceLastComboEnd >= ComboCoolTime)
            {
                isOnCombo = true;
                SinceLastComboBegin = 0f; // 콤보 시작 시간 초기화
            }
        }
        
        // 콤보 중
        else
        {
            SinceLastComboBegin += Time.deltaTime; // 경과 시간 증가
            
            if (ComboNum == 0)
                Combo1();
            else
                Combo2();
        }
    }

    private void FireBullet(int BulletCount, float Angle)
    {
        for (int i = 0; i < BulletCount; i++)
        {
            // x축으로 일정 간격을 두고 총알 생성
            float offset = (i - BulletCount / 2) * BulletSpacing; // 중심을 기준으로 간격 조정
            Vector3 spawnPosition = transform.position + new Vector3(offset, 0, 0);

            // 플레이어의 x 좌표에 따라 각도를 계산합니다.
            float x = transform.position.x + offset;

            // 레이저 오브젝트를 생성합니다.
            GameObject laser = PoolManager.Instance.MakeObj(PoolObjType.BossBullet, spawnPosition);

            // z축 회전량을 설정합니다.
            laser.transform.rotation = Quaternion.Euler(0, 0, Angle);
        }
     
        SoundManager.Instance.PlaySFX(SFX.bossShot, transform.position);
    }

    private void Combo1()
    {
        if (combo1Timer >= Combo1Interval)
        {
            float angle = Mathf.Lerp(-10, -170, SinceLastComboBegin / Combo1Time);

            FireBullet(1, angle); 
            combo1Timer = 0f; // 타이머 초기화
        }
        else
        {
            combo1Timer += Time.deltaTime; // 타이머 증가
        }

        if (SinceLastComboBegin >= Combo1Time)
        {
            isOnCombo = false;
            ComboNum = 1; // 다음 콤보로 전환
            SinceLastComboEnd = 0f; // 쿨타임 초기화
        }
    }

    private void Combo2()
    {
        if (combo2Timer >= Combo2Interval)
        {
            FireBullet(12, -90f);
            combo2Timer = 0f; // 타이머 초기화
        }
        else
        {
            combo2Timer += Time.deltaTime; // 타이머 증가
        }

        if (SinceLastComboBegin >= Combo2Time)
        {
            isOnCombo = false;
            ComboNum = 0;
            SinceLastComboEnd = 0f; // 쿨타임 초기화
        }
    }
}
