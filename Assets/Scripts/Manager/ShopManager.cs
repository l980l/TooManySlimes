using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [SerializeField] private SkillData[] skillDatas;
    [SerializeField] private GameObject shopObj;
    [SerializeField] private Text[] skillLvText;
    [SerializeField] private Text[] skillDescText;

    #region Singleton
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        shopObj.SetActive(false);
    }
    #endregion

    private void SetShopInfo()
    {
        int size = skillDescText.Length;
        for (int i = 0; i < size; i++)
        {
            int skillLV = EnhancementManager.Instance.SkillLV[i];

            if(skillLV < skillDatas[i].coolTime.Length)
            {
                skillLvText[i].text = "Level: " + skillLV.ToString();

                skillDescText[i].text = string.Format(skillDatas[i].skillDescription, skillDatas[i].counts[skillLV] * skillDatas[i].baseCount, skillDatas[i].coolTime[skillLV] * skillDatas[i].baseCoolTime, skillDatas[i].Damage);
            }
        }
    }

    public void ShowShop()
    {
        Time.timeScale = 0f;
        SetShopInfo();
        shopObj.SetActive(true);
    }

    public void HideShop()
    {
        SoundManager.Instance.PlaySFX(SFX.button, transform.position);

        Time.timeScale = 1f;
        shopObj.SetActive(false);
    }

    // 버튼에서 호출하기 위해 int를 파라미터로 사용
    public void PerchaseSkill(int Type)
    {
        SoundManager.Instance.PlaySFX(SFX.button, transform.position);

        // 일단 가격은 2로 고정
        if(CoinManager.Instance.CoinCount >= 2)
        {
            switch ((FloorType)Type)
            {
                case FloorType.NormalBulletEnhance:
                    EnhancementManager.Instance.EnhanceSkill(FloorType.NormalBulletEnhance);
                    break;
                case FloorType.LaserEnhance:
                    EnhancementManager.Instance.EnhanceSkill(FloorType.LaserEnhance);
                    break;
                case FloorType.ShieldEnhance:
                    EnhancementManager.Instance.EnhanceSkill(FloorType.ShieldEnhance);
                    break;
                default:
                    break;
            }

            CoinManager.Instance.CoinCount -= 2;
            SetShopInfo();
        }
    }
}
