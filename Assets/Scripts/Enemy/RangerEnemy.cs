using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerEnemy : MonoBehaviour
{
    [SerializeField] private GameObject rangerBulletPrefab;
    [SerializeField] private float rangerBulletCoolTime;
    private float timeSinceLastBullet;

    private void OnEnable()
    {
        timeSinceLastBullet = 0f; // 초기화
    }

    private void Update()
    {
        timeSinceLastBullet += Time.deltaTime; // 경과 시간 증가

        if (timeSinceLastBullet >= rangerBulletCoolTime)
        {
            FireNormalBullet();
            timeSinceLastBullet = 0f; // 쿨타임 초기화
        }
    }

    private void FireNormalBullet()
    {
        // AddForce는 Awake에서 함.
        PoolManager.Instance.MakeObj(PoolObjType.rangerBullet, transform.position);
    }
}
