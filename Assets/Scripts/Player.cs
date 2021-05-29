using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid; //이동용 리지드바디2D
    private Animator anim; //애니메이터
    private SpriteRenderer rend; //스프라이트 렌더러
    private BoxCollider2D boxColl;
    float collOffset, collSize;

    private AudioSource audioSource; //오디오 출력용 컴포넌트
    public AudioClip jumpAudio, slideAudio, collAudio; //오디오 클립

    public int jumpCount = 0; //점프 횟수

    enum Sound //사운드 타입 열거
    {
        Jump, Slide, Coll
    };

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        audioSource = this.gameObject.AddComponent<AudioSource>();

        collOffset = boxColl.offset.y;
        collSize = boxColl.size.y;
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

        if (jumpCount == 0) //점프 중이 아닐 때
        {
            if (Input.GetKey(KeyCode.DownArrow)) //슬라이딩 시작
                Sliding(true);
            else //슬라이딩 끝
                Sliding(false);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < 2)
            {
                Sliding(false);
                rigid.velocity = new Vector2(rigid.velocity.x, 10.0f); //rigid.AddForce(Vector2.up * 500.0f);
                jumpCount++; //점프 횟수 증가
                anim.SetInteger("Jump", jumpCount);
                PlaySound(Sound.Jump); //점프 사운드 출력

                if (jumpCount == 2)
                    anim.SetTrigger("DoubleJump"); //더블점프 트리거 설정
            }
        }
        rigid.velocity = new Vector2(hor * 5.0f, rigid.velocity.y);
    }

    void PlaySound(Sound type)
    {
        switch (type)
        {
            case Sound.Jump:
                audioSource.clip = jumpAudio;
                break;
            case Sound.Slide:
                audioSource.clip = slideAudio;
                break;
            case Sound.Coll:
                audioSource.clip = collAudio;
                break;
        }
        audioSource.Play();
    }

    void Sliding(bool value)
    {
        if (value)
        {
            if (anim.GetBool("Sliding")) //이미 슬라이딩 중
                return;

            anim.SetBool("Sliding", true);
            boxColl.size = new Vector2(boxColl.size.x, boxColl.size.y / 2);
            boxColl.offset = new Vector2(boxColl.offset.x, boxColl.offset.y - (boxColl.size.y / 2));
        }
        else
        {
            if (!anim.GetBool("Sliding")) //슬라이딩 중이 아님
                return;

            anim.SetBool("Sliding", false);
            boxColl.offset = new Vector2(boxColl.offset.x, boxColl.offset.y + (boxColl.size.y / 2));
            boxColl.size = new Vector2(boxColl.size.x, boxColl.size.y * 2);
        }
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

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Obstacle")
        {
            Debug.Log("Damaged!");
            anim.SetTrigger("Damaged"); //데미지 트리거 설정
            rend.color = new Color(1, 1, 1, 0.4f); //알파값 설정
            this.gameObject.layer = LayerMask.NameToLayer("IgnoreCollision"); //레이어 변경
            PlaySound(Sound.Coll); //점프 사운드 출력

            Invoke("ResetDefaultLayer", 2);
        }
    }

    void ResetDefaultLayer()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        rend.color = new Color(1, 1, 1, 1f); //알파값 설정
    }
}
