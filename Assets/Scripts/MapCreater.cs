using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreater : MonoBehaviour
{
    public GameObject[] maps; //�� ����Ʈ

    int idx; //������ ���� ��ȣ
    Vector2 startPos = new Vector2(25.0f, -5.0f);

    public void NewMap()
    {
        Instantiate(maps[++idx % maps.Length], startPos, Quaternion.identity);
    }
}
