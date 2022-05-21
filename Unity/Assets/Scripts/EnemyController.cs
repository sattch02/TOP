using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private CharaBase charaBase;

    [SerializeField] private float counterTime = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input_vec = Vector3.zero;

        counterTime += Time.deltaTime;

        /*
        if (Mathf.Floor(counterTime) % 2 == 0)
        {
            input_vec.x -= 1;
        }
        else
        {
            input_vec.x += 1;
        }

        if (charaBase != null)
        {
            charaBase.Move(input_vec);
        }*/
    }

    /// <summary>
    /// 敵設定
    /// </summary>
    /// <param name="_charaBase"></param>
    public void SetChara(CharaBase _charaBase)
    {
        charaBase = _charaBase;
    }
}
