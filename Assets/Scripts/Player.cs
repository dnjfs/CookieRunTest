using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid; //이동용 리지드바디2D
    private Animator anim; //애니메이터
    private SpriteRenderer rend; //스프라이트 렌더러

    public int jumpCount = 0;

    void Start()
    {
        Debug.Log("Start");
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal"); //수평이동, 좌우 방향키입력

        if (hor != 0)
        {
            anim.SetBool("Run", true); //달리는 상태

            if (hor < 0)
                rend.flipX = true; //왼쪽으로 이동 시 좌우반전
            else
                rend.flipX = false;
        }
        else
            anim.SetBool("Run", false); //달리지 않는 상태

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < 2)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 10.0f); //rigid.AddForce(Vector2.up * 500.0f);
                jumpCount++; //점프 횟수 증가
                anim.SetInteger("Jump", jumpCount);

                if (jumpCount == 2)
                    anim.SetTrigger("DoubleJump"); //더블점프 트리거 설정
            }
        }
        rigid.velocity = new Vector2(hor * 5.0f, rigid.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            Debug.Log("Collision!");
            jumpCount = 0; //점프 카운트 초기화
            anim.SetInteger("Jump", jumpCount);
            anim.SetTrigger("Land"); //바닥에 닿을 때
        }
    }
}
