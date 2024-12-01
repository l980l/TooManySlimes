using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour 
{
    [SerializeField] private float speed;
    private int startIdx;
    private int endIdx;
    private float viewHeight;

    [SerializeField] private Transform[] sprites;

    private void Awake()
    {
        startIdx = 2;
        endIdx = 0;
        viewHeight = Camera.main.orthographicSize * 2;  
    }

    private void Update()
    {
        if(!DownManager.Instance.Stop)
        {
            Vector3 curPos = transform.position;
            Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
            transform.position = curPos + nextPos;

            if (sprites[endIdx].position.y < viewHeight * -1)
            {
                Vector3 backSpritePos = sprites[startIdx].localPosition;
                Vector3 frontSpritePos = sprites[endIdx].localPosition;
                sprites[endIdx].transform.localPosition = backSpritePos + Vector3.up * viewHeight;

                int startIndexSave = startIdx;
                startIdx = endIdx;
                endIdx = (startIndexSave - 1 == -1) ? sprites.Length - 1 : startIndexSave - 1;
            }
        }
    }
}
