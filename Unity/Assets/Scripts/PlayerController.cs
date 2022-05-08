using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの操作周り
/// </summary>
public class PlayerController : MonoBehaviour
{
    // 操作しているキャラのターゲット
    [SerializeField] private CharaBase charaBase;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input_vec = Vector3.zero;

        if (Input.GetKey("right"))
        {
            input_vec.x += 1;
        }

        if (Input.GetKey("left"))
        {
            input_vec.x -= 1;
        }

        if (charaBase != null)
        {
            charaBase.Move(input_vec);
        }

    }

    /// <summary>
    /// キャラ設定
    /// </summary>
    /// <param name="_charaBase"></param>
    public void SetChara(CharaBase _charaBase)
    {
        charaBase = _charaBase;
    }
}
