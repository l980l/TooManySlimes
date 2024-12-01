using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private GameObject bossPrefabs;  // 보스 몬스터

    [SerializeField] private float[] startX;    // 생성 X 좌표
    [SerializeField] private float SpawnRate;   // 랜덤 생성 확률
    [SerializeField] private float MonsterPerItemRatio;   // 아이템 대비 몬스터 생성 비율

    [SerializeField] private float SpawnDelayUnit;   // 겹치지 않게 한 칸씩 나올 수 있는 스폰 딜레이 단위
    [SerializeField] private float curSpawnDelay;
    private bool stopSpawn;

    #region Singleton
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    private void Update()
    {
        if (!DownManager.Instance.Stop && !stopSpawn)
        {
            curSpawnDelay += Time.deltaTime;

            if (curSpawnDelay >= SpawnDelayUnit)
            {
                SpawnEnemyAndItem();
                curSpawnDelay = 0;
            }
        }
    }

    private void SpawnEnemyAndItem()
    {
        if (Random.value <= SpawnRate)
        {
            int[] availablePositions = new int[startX.Length];
            for (int i = 0; i < startX.Length; i++)
            {
                availablePositions[i] = i;
            }

            int totalCount = Random.Range(1, 6); // 1~5

            ShuffleArray(availablePositions);

            for (int i = 0; i < totalCount; i++)
            {
                if (i >= availablePositions.Length) break;

                float StartY = 7f;
                int positionIndex = availablePositions[i];
                Vector3 spawnPosition = new Vector3(startX[positionIndex], StartY, 0);

                if (Random.value <= MonsterPerItemRatio)
                {
                    int enemyIndex = Random.Range(0, (int)PoolObjType.MonsterEnd);
                    PoolManager.Instance.MakeObj((PoolObjType)enemyIndex, spawnPosition); // 오브젝트 풀에서 적 생성
                }
                else
                {
                    int itemIndex = Random.Range((int)PoolObjType.MonsterEnd + 1, (int)PoolObjType.ItemEnd);
                    PoolManager.Instance.MakeObj((PoolObjType)itemIndex, spawnPosition); // 오브젝트 풀에서 아이템 생성
                }
            }
        }
    }

    private void ShuffleArray(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    public void SpawnFloor()
    {
        float offset = 0.3f;
        Vector3 spawnPosition1 = new Vector3(startX[1]- offset, 7f, 0);
        Vector3 spawnPosition2 = new Vector3(startX[3]+ offset, 7f, 0);

        PoolManager.Instance.MakeObj(PoolObjType.floor, spawnPosition1); 
        PoolManager.Instance.MakeObj(PoolObjType.floor, spawnPosition2);
    }

    public void SpawnBoss()
    {
        Invoke("InstantiateBoss", 5f);  // 5초 후에 생성
    }

    private void InstantiateBoss()
    {
        Vector3 spawnPosition = new Vector3(startX[2], 7f, 0);
        Instantiate(bossPrefabs, spawnPosition, Quaternion.identity);
    }

    public void SetStopSpawn(bool _StopSpawn)
    {
        stopSpawn = _StopSpawn;
    }
}
