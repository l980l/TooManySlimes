using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static readonly float MapHalfWidth = 2f;    // 맵 전체의 절반 가로 길이. 좌우 경계를 위해 사용
    
    [SerializeField] private float PlayerY;         // 플레이어 Y 좌표 
    [SerializeField] private float HorizontalSpeed;    // 가로 이동 속도

    // 컴포넌트
    private Rigidbody2D Rigidbody;
    private Animator animator;

    // 조이스틱 및 이동
    public FloatingJoystick joystick;
    private bool lastJoyActive;
    private float prevJoyValue;  // 이전 조이스틱 값
    private float JoyStartX;    // 조이스틱 활성화 시점의 X 좌표
    private bool OnLeftBoarder;
    private bool OnRightBoarder;


    // 애니메이션 
    readonly int AnimInputHash = Animator.StringToHash("Input");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.MovePosition(new Vector2(0, PlayerY));
    }

    private void FixedUpdate()
    {
        // 조이스틱 활성화 상태인 경우
        if(joystick.isUsing)
        {
            float xPos = joystick.Horizontal;

            // 원래 활성화 중인 경우
            if (lastJoyActive)
            {
                // 애니메이션
                if (xPos > prevJoyValue)
                    animator.SetInteger(AnimInputHash, 1);
                else if (xPos < prevJoyValue)
                    animator.SetInteger(AnimInputHash, -1);

                // 이동
                float newX = JoyStartX + xPos * HorizontalSpeed;
                // 맵을 나가지 못하도록
                if(newX < -MapHalfWidth)
                    newX = -MapHalfWidth;
                if(newX > MapHalfWidth)
                    newX = MapHalfWidth;
                Rigidbody.MovePosition(new Vector2(newX, PlayerY));
            }

            // 새롭게 활성화한 경우
            else
            {
                lastJoyActive = true;
                JoyStartX = transform.position.x;
            }

            prevJoyValue = joystick.Horizontal;
        }

        // 조이스틱 비활성화 상태
        else
        {
            animator.SetInteger(AnimInputHash, 0);
            lastJoyActive = false;
        }
    }

}
