using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownMovement : MonoBehaviour
{
    public static readonly float DownSpeed = 1.5f;

    private float XPos;     // X축 좌표. 생성기에서 정해줌.
    private float prevY;    // 이전 Y 좌표
    private bool stopUse;    // 객체 개인의 Down 미사용 여부. ShieldProj로 밀칠 때, 꺼야 Down을 꺼야 돼서 사용
    [SerializeField] private bool passPlayer;   // 플레이어 통과여부

    private Rigidbody2D Rigidbody;

    public void SetStopUse(bool _stopUse) { stopUse = _stopUse; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        XPos = transform.position.x;
        prevY = 7f;
        stopUse = false;
    }

    private void FixedUpdate()
    {
        if(!DownManager.Instance.Stop && !stopUse)
        {
            float newY = prevY - DownSpeed * Time.fixedDeltaTime;
            prevY = newY;
            Rigidbody.MovePosition(new Vector2(XPos, newY));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DownBoarder"))
            gameObject.SetActive(false);
    }

    // 플레이어가 충돌 중인 몬스터 개수를 Enter와 Exit에서 ++, -- 하는 식으로 최적화 가능할듯
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !passPlayer)
            DownManager.Instance.Stop = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !passPlayer)
            DownManager.Instance.Stop = false;
    }
}
