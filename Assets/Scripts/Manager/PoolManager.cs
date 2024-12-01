using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjType
{
    normalEnemy,
    rangerEnemy,
    MonsterEnd,

    coin,
    ItemEnd,
    floor,

    PlayerBullet,
    PlayerLaser,
    PlayerShieldProj,   // 방패가 아니라 방패에서 나가는 투사체임
    rangerBullet,
    BossBullet,
    Max,
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField] private GameObject normalEnemyPrefab;
    [SerializeField] private GameObject rangerEnemyPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject floorPrefab; 
    [SerializeField] private GameObject PlayerBulletPrefab;
    [SerializeField] private GameObject PlayerLaserProjPrefab;
    [SerializeField] private GameObject PlayerShieldProjPrefab;
    [SerializeField] private GameObject rangerBulletPrefab;
    [SerializeField] private GameObject BossBulletPrefab;

    private GameObject[] normalEnemyPool;
    private GameObject[] rangerEnemyPool;
    private GameObject[] coinPool;
    private GameObject[] floorPool;
    private GameObject[] PlayerBulletPool;
    private GameObject[] PlayerLaserProjPool;
    private GameObject[] PlayerShieldProjPool;
    private GameObject[] rangerBulletPool;
    private GameObject[] BossBulletPool;

    private int poolSize = 5; // 기본 풀 크기

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        normalEnemyPool = new GameObject[poolSize];
        rangerEnemyPool = new GameObject[poolSize];
        coinPool = new GameObject[poolSize];
        floorPool = new GameObject[poolSize];
        PlayerBulletPool = new GameObject[poolSize];
        PlayerLaserProjPool = new GameObject[poolSize];
        rangerBulletPool = new GameObject[poolSize];
        PlayerShieldProjPool = new GameObject[poolSize];
        BossBulletPool = new GameObject[poolSize];

        Generate();
    }

    private void Generate()
    {
        // Normal Enemy Pool 생성
        for (int i = 0; i < normalEnemyPool.Length; ++i)
        {
            normalEnemyPool[i] = Instantiate(normalEnemyPrefab);
            normalEnemyPool[i].SetActive(false); 
        }

        // Ranger Enemy Pool 생성
        for (int i = 0; i < rangerEnemyPool.Length; ++i)
        {
            rangerEnemyPool[i] = Instantiate(rangerEnemyPrefab);
            rangerEnemyPool[i].SetActive(false); 
        }

        // Coin Pool 생성
        for (int i = 0; i < coinPool.Length; ++i)
        {
            coinPool[i] = Instantiate(coinPrefab);
            coinPool[i].SetActive(false);
        }

        // Floor Pool 생성
        for (int i = 0; i < floorPool.Length; ++i)
        {
            floorPool[i] = Instantiate(floorPrefab);
            floorPool[i].SetActive(false);
        }

        // Player Bullet Pool 생성
        for (int i = 0; i < PlayerBulletPool.Length; ++i)
        {
            PlayerBulletPool[i] = Instantiate(PlayerBulletPrefab);
            PlayerBulletPool[i].SetActive(false); 
        }

        // Player LaserProj Pool 생성
        for (int i = 0; i < PlayerLaserProjPool.Length; ++i)
        {
            PlayerLaserProjPool[i] = Instantiate(PlayerLaserProjPrefab);
            PlayerLaserProjPool[i].SetActive(false); 
        }

        // Player ShieldProj Pool 생성
        for (int i = 0; i < PlayerShieldProjPool.Length; ++i)
        {
            PlayerShieldProjPool[i] = Instantiate(PlayerShieldProjPrefab);
            PlayerShieldProjPool[i].SetActive(false); 
        }

        // Ranger Bullet Pool 생성
        for (int i = 0; i < rangerBulletPool.Length; ++i)
        {
            rangerBulletPool[i] = Instantiate(rangerBulletPrefab);
            rangerBulletPool[i].SetActive(false);
        }

        // Boss Bullet Pool 생성
        for (int i = 0; i < BossBulletPool.Length; ++i)
        {
            BossBulletPool[i] = Instantiate(BossBulletPrefab);
            BossBulletPool[i].SetActive(false);
        }
    }

    public GameObject MakeObj(PoolObjType type, Vector3 pos)
    {
        switch (type)
        {
            case PoolObjType.normalEnemy:
                return GetObjectFromPool(ref normalEnemyPool, normalEnemyPrefab, pos);
            case PoolObjType.rangerEnemy:
                return GetObjectFromPool(ref rangerEnemyPool, rangerEnemyPrefab, pos);
            case PoolObjType.coin:
                return GetObjectFromPool(ref coinPool, coinPrefab, pos);
            case PoolObjType.floor:
                return GetObjectFromPool(ref floorPool, floorPrefab, pos);
            case PoolObjType.PlayerBullet:
                return GetObjectFromPool(ref PlayerBulletPool, PlayerBulletPrefab, pos);
            case PoolObjType.PlayerLaser:
                return GetObjectFromPool(ref PlayerLaserProjPool, PlayerLaserProjPrefab, pos);
            case PoolObjType.PlayerShieldProj: 
                return GetObjectFromPool(ref PlayerShieldProjPool, PlayerShieldProjPrefab, pos);
            case PoolObjType.rangerBullet:
                return GetObjectFromPool(ref rangerBulletPool, rangerBulletPrefab, pos);
            case PoolObjType.BossBullet: 
                return GetObjectFromPool(ref BossBulletPool, BossBulletPrefab, pos);
            default:
                return null;
        }
    }

    private GameObject GetObjectFromPool(ref GameObject[] pool, GameObject prefab, Vector3 pos)
    {
        for (int i = 0; i < pool.Length; ++i)
        {
            if (!pool[i].activeSelf)
            {
                pool[i].transform.position = pos;
                pool[i].SetActive(true);
                return pool[i];
            }
        }

        // 오브젝트 풀이 가득 찼을 경우 새로운 오브젝트를 추가
        ExpandPool(ref pool, prefab);
        return GetObjectFromPool(ref pool, prefab, pos);
    }

    private void ExpandPool(ref GameObject[] pool, GameObject prefab)
    {
        int newSize = pool.Length + poolSize; // 기존 크기에 기본 크기를 더함
        GameObject[] newPool = new GameObject[newSize];

        for (int i = 0; i < pool.Length; ++i)
        {
            newPool[i] = pool[i];
        }

        for (int i = pool.Length; i < newSize; ++i)
        {
            newPool[i] = Instantiate(prefab);
            newPool[i].SetActive(false); // 비활성화
        }

        pool = newPool; // 기존 풀을 새로운 풀로 교체
    }
}
