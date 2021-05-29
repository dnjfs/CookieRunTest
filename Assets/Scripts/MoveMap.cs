using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    float endPosition = -25f;
    bool signal;

    void Update()
    {
        transform.Translate(-10f * Time.deltaTime, 0, 0); //맵 이동

        if (transform.position.x <= 0f && !signal) //맵이 일정 위치를 지나고 아직 신호를 보내지 않은 경우
        {
            GameObject.Find("MapCreater").GetComponent<MapCreater>().NewMap(); //맵 생성
            signal = true;
        }

        if (transform.position.x <= endPosition) //범위를 벗어나면
            Destroy(gameObject); //객체 제거
    }
}
