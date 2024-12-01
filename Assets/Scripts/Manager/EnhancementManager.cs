using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 강화 상태를 갖고 있는 매니저
public class EnhancementManager : MonoBehaviour
{
    public static EnhancementManager Instance;

    // 강화 레벨
    public int[] SkillLV;
    public FloorType prveFloor;

    [SerializeField] private Text[] SkillLVText;

    #region Singleton
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        prveFloor = FloorType.Max;
        Instance = this;
    }
    #endregion

    public void EnhanceSkill(FloorType type)
    {
        switch (type)
        {
            case FloorType.NormalBulletEnhance:
                SkillLV[0]++;
                SkillLVText[0].text = SkillLV[0].ToString();
                break;
            case FloorType.LaserEnhance:
                SkillLV[1]++;
                SkillLVText[1].text = SkillLV[1].ToString();
                break;
            case FloorType.ShieldEnhance:
                SkillLV[2]++;
                SkillLVText[2].text = SkillLV[2].ToString();
                break;
            default:
                break;
        }
        SoundManager.Instance.PlaySFX(SFX.enhance, transform.position);
    }
}
