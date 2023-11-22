using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Gaming");
    }
    // 브랜치 수정 내용
}
