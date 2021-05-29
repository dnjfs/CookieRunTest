using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid; //�̵��� ������ٵ�2D
    private Animator anim; //�ִϸ�����
    private SpriteRenderer rend; //��������Ʈ ������
    private BoxCollider2D boxColl;
    float collOffset, collSize;

    private AudioSource audioSource; //����� ��¿� ������Ʈ
    public AudioClip jumpAudio, slideAudio, collAudio; //����� Ŭ��

    public int jumpCount = 0; //���� Ƚ��

    enum Sound //���� Ÿ�� ����
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

        if (jumpCount == 0) //���� ���� �ƴ� ��
        {
            if (Input.GetKey(KeyCode.DownArrow)) //�����̵� ����
                Sliding(true);
            else //�����̵� ��
                Sliding(false);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < 2)
            {
                Sliding(false);
                rigid.velocity = new Vector2(rigid.velocity.x, 10.0f); //rigid.AddForce(Vector2.up * 500.0f);
                jumpCount++; //���� Ƚ�� ����
                anim.SetInteger("Jump", jumpCount);
                PlaySound(Sound.Jump); //���� ���� ���

                if (jumpCount == 2)
                    anim.SetTrigger("DoubleJump"); //�������� Ʈ���� ����
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
            if (anim.GetBool("Sliding")) //�̹� �����̵� ��
                return;

            anim.SetBool("Sliding", true);
            boxColl.size = new Vector2(boxColl.size.x, boxColl.size.y / 2);
            boxColl.offset = new Vector2(boxColl.offset.x, boxColl.offset.y - (boxColl.size.y / 2));
        }
        else
        {
            if (!anim.GetBool("Sliding")) //�����̵� ���� �ƴ�
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
            jumpCount = 0; //���� ī��Ʈ �ʱ�ȭ
            anim.SetInteger("Jump", jumpCount);
            anim.SetTrigger("Land"); //�ٴڿ� ���� ��
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Obstacle")
        {
            Debug.Log("Damaged!");
            anim.SetTrigger("Damaged"); //������ Ʈ���� ����
            rend.color = new Color(1, 1, 1, 0.4f); //���İ� ����
            this.gameObject.layer = LayerMask.NameToLayer("IgnoreCollision"); //���̾� ����
            PlaySound(Sound.Coll); //���� ���� ���

            Invoke("ResetDefaultLayer", 2);
        }
    }

    void ResetDefaultLayer()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        rend.color = new Color(1, 1, 1, 1f); //���İ� ����
    }
}
