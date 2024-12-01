using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserProj : MonoBehaviour
{
    // Z축 회전량을 발사 방향으로 사용.
    public PlayerProjectileData projectileData;

    [SerializeField] private float speed;
    private Vector2 dir;

    private Rigidbody2D RB;
    private TrailRenderer TR;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        TR = GetComponent<TrailRenderer>();
    }

    private void OnDisable()
    {
        TR.Clear();
    }

    private void FixedUpdate()
    {
        // 현재 z축 회전량을 가져오고 라디안으로 변환.
        float radians = transform.eulerAngles.z * Mathf.Deg2Rad;

        // 방향 벡터를 계산
        dir = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        Vector2 newPosition = RB.position + dir * speed * Time.fixedDeltaTime;
        RB.MovePosition(newPosition);
        // 경계 체크
        CheckBoundaries(newPosition);
    }

    private void CheckBoundaries(Vector2 position)
    {
        // PlayerMovement.MapHalfWidth으로 좌우측 경계 X좌표 구하기
        float leftBoundary = -PlayerMovement.MapHalfWidth;
        float rightBoundary = PlayerMovement.MapHalfWidth;

        if (position.x <= leftBoundary)
        {
            // X축 방향 반전
            if (dir.x < 0)
            {
                SoundManager.Instance.PlaySFX(SFX.reflect, transform.position);
                dir.x = -dir.x;
            }
            UpdateLaserRot();
        }
        
        if(position.x >= rightBoundary)
        {
            // X축 방향 반전
            if(dir.x > 0)
            {
                SoundManager.Instance.PlaySFX(SFX.reflect, transform.position);
                dir.x = -dir.x;
            }
            UpdateLaserRot();
        }
    }

    private void UpdateLaserRot()
    {
        // 레이저의 방향을 업데이트
        float newAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletBoarder"))
        {
            gameObject.SetActive(false);
        }
    }
}
