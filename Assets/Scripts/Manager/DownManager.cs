using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownManager : MonoBehaviour
{
    public static DownManager Instance;
    public bool Stop;

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

}
