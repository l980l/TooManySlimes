using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceManager : MonoBehaviour
{
    [SerializeField] private Text distanceText;
    [SerializeField] private Slider distanceSlider;

    [SerializeField] private float TimeTakenFor1m;
    [SerializeField] private int initDistance;
    [SerializeField] private int[] SpawnFloorDis;   // Floor를 생성할 위치. distance는 남은 거리니까 내림차순으로 세팅.
    private int nextFloorDisIdx;
    private int distance;
    private float timer;

    private void Awake()
    {
        distance = initDistance;
        distanceSlider.value = 0;
        nextFloorDisIdx = 0;
    }

    private void Update()
    {
        if(DownManager.Instance != null && !DownManager.Instance.Stop && distance > 0)
        {
            timer += Time.deltaTime;

            if (timer >= TimeTakenFor1m)
            {
                distance--;
                UpdateDistanceText();

                // SpawnFloorDis에 도달했는지 확인
                if (nextFloorDisIdx < SpawnFloorDis.Length)
                {
                    if (SpawnFloorDis[nextFloorDisIdx] == distance)
                    {
                        SpawnManager.Instance.SpawnFloor();
                        nextFloorDisIdx++;
                    }
                }

                // 목표지 도착
                if (distance == 0)
                {
                    distanceSlider.gameObject.SetActive(false);
                    SpawnManager.Instance.SpawnBoss();
                    SpawnManager.Instance.SetStopSpawn(true);
                }

                timer -= TimeTakenFor1m;
            }
        }
    }

    private void UpdateDistanceText()
    {
        distanceText.text = distance.ToString();
        distanceSlider.value = (float)(initDistance - distance) / initDistance;
    }
}
