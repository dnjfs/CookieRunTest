using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    float endPosition = -25f;
    bool signal;

    void Update()
    {
        transform.Translate(-10f * Time.deltaTime, 0, 0); //�� �̵�

        if (transform.position.x <= 0f && !signal) //���� ���� ��ġ�� ������ ���� ��ȣ�� ������ ���� ���
        {
            GameObject.Find("MapCreater").GetComponent<MapCreater>().NewMap(); //�� ����
            signal = true;
        }

        if (transform.position.x <= endPosition) //������ �����
            Destroy(gameObject); //��ü ����
    }
}
