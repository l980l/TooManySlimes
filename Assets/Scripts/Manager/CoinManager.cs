using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
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

    [SerializeField] private Text CoinText;
    
    private int cointCount;
    public int CoinCount { get { return cointCount; } set { cointCount = value;  UpdateCoinText(); } }

    private void UpdateCoinText()
    { 
        if(CoinText != null) 
            CoinText.text = cointCount.ToString();   
    }
}
