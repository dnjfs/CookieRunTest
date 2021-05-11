using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid; //�̵��� ������ٵ�2D
    private Animator anim; //�ִϸ�����
    private SpriteRenderer rend; //��������Ʈ ������

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
        float hor = Input.GetAxis("Horizontal"); //�����̵�, �¿� ����Ű�Է�

        if (hor != 0)
        {
            anim.SetBool("Run", true); //�޸��� ����

            if (hor < 0)
                rend.flipX = true; //�������� �̵� �� �¿����
            else
                rend.flipX = false;
        }
        else
            anim.SetBool("Run", false); //�޸��� �ʴ� ����

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < 2)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 10.0f); //rigid.AddForce(Vector2.up * 500.0f);
                jumpCount++; //���� Ƚ�� ����
                anim.SetInteger("Jump", jumpCount);

                if (jumpCount == 2)
                    anim.SetTrigger("DoubleJump"); //�������� Ʈ���� ����
            }
        }
        rigid.velocity = new Vector2(hor * 5.0f, rigid.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            Debug.Log("Collision!");
            jumpCount = 0; //���� ī��Ʈ �ʱ�ȭ
            anim.SetInteger("Jump", jumpCount);
            anim.SetTrigger("Land"); //�ٴڿ� ���� ��
        }
    }
}
