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
        bool attack_flg = false;
        bool guard_flg = false;
        bool strong_attack_flg = false;


        if (Input.GetKey("right"))
        {
            input_vec.x += 1;
        }

        if (Input.GetKey("left"))
        {
            input_vec.x -= 1;
        }

        if (Input.GetKey("w"))
        {
            guard_flg = true;
        }

        if (Input.GetKeyDown("space"))
        {
            attack_flg = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            strong_attack_flg = true;
        }

        if (charaBase != null)
        {
            charaBase.Move(input_vec);

            if (guard_flg)
            {
                charaBase.Guard(input_vec);
            }
            else if (attack_flg)
            {
                charaBase.Attack(input_vec);
            }
            else if (strong_attack_flg)
            {
                charaBase.StrongAttack(input_vec);
            }
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
