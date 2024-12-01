using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorType
{
    NormalBulletEnhance,
    LaserEnhance,
    ShieldEnhance,
    Shop,
    Max
}

public class Floor : MonoBehaviour
{
    // 충돌시 강화를 하거나 상점을 열을 판.
    // 활성화 시 타입 정하기
    // 이미지 만들 시간이 아까워서 이미지만 띄워줄 자식 3개를 붙여서 만듦. 
    private FloorType floorType;
    [SerializeField] private SkillData[] skillDatas;
    [SerializeField] private Sprite shopIcon;

    private SpriteRenderer spriteRenderer;
    private ParticleSystem PS;
    private bool isUsed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PS = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        isUsed = false;
        PS.Stop();

        // 랜덤으로 FloorType 설정
        while (true)
        {
            floorType = (FloorType)UnityEngine.Random.Range(0, (int)FloorType.Max);
            // 같은 바닥이 동시에 나오지 않도록.
            if (floorType != EnhancementManager.Instance.prveFloor)
            {
                EnhancementManager.Instance.prveFloor = floorType;
                break;
            }
        }

        InitializeFloor(floorType);
    }

    private void InitializeFloor(FloorType type)
    {
        // 각 타입에 따라 초기화 로직을 추가할 수 있습니다.
        switch (type)
        {
            case FloorType.NormalBulletEnhance:
                spriteRenderer.sprite = skillDatas[(int)FloorType.NormalBulletEnhance].Icon;
                break;
            case FloorType.LaserEnhance:
                spriteRenderer.sprite = skillDatas[(int)FloorType.LaserEnhance].Icon;
                break;
            case FloorType.ShieldEnhance:
                spriteRenderer.sprite = skillDatas[(int)FloorType.ShieldEnhance].Icon;
                break;
            case FloorType.Shop:
                spriteRenderer.sprite = shopIcon;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isUsed)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if(floorType!=FloorType.Shop) 
                    EnhancementManager.Instance.EnhanceSkill(floorType);
                else
                    ShopManager.Instance.ShowShop();
                isUsed = true;
                PS.Play();
            }
        }

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Item"))
        {
            collision.gameObject.SetActive(false);  
        }
    }
}
