using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreater : MonoBehaviour
{
    public GameObject[] maps; //맵 리스트

    int idx; //생성된 맵의 번호
    Vector2 startPos = new Vector2(25.0f, -5.0f);

    public void NewMap()
    {
        Instantiate(maps[++idx % maps.Length], startPos, Quaternion.identity);
    }
}
