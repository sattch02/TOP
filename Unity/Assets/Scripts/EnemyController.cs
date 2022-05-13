using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private CharaBase charaBase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
