using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Singleton
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 120;  // 부드러운 녹화를 위해
        Instance = this;
    }
    #endregion

    // 플레이어 사망 시 호출할 함수. 아마 로비로 넘어가지게 하고 끝일듯?
    public void PlayerDie()
    {
        SoundManager.Instance.PlaySFX(SFX.death, transform.position);
    }

    public void BossDie()
    {
        SoundManager.Instance.PlaySFX(SFX.clear, transform.position);
        SoundManager.Instance.PlaySFX(SFX.clear2, transform.position);
    }
}
