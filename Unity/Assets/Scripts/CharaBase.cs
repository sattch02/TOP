using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラの継承元クラス
/// 味方や敵キャラに継承して使用予定
/// </summary>
public class CharaBase : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform trans;

    [SerializeField] private float speed = 10f;

    public virtual void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // キャラ独自動作やノックバックなどはここで継承して動作させること
    }

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        trans = transform;

        // TODO:キャラの読み込みや大きさ、当たり判定をデータによって仕込む。
    }

    /// <summary>
    /// 移動(ベクトルの向きで)
    /// </summary>
    /// <param name="vec"></param>
    public virtual void Move(Vector3 vec)
    {
        // 速度計算
        float speed_x = speed * Time.deltaTime * vec.x;

        MoveAnimation(Mathf.Abs(vec.x));

        if (speed_x != 0)
        {
            Vector3 tempPosition = transform.localPosition;
            tempPosition.x += speed_x;
            transform.localPosition = tempPosition;
        }

        SetDirection(vec);
    }

    /// <summary>
    /// 移動アニメーション(速度要素によって)
    /// </summary>
    /// <param name="_speed"></param>
    public virtual void MoveAnimation(float _speed)
    {
        anim.SetFloat("Speed", _speed);
    }

    /// <summary>
    /// 向き設定(ベクトルの向きで)
    /// </summary>
    /// <param name="vec"></param>
    public virtual void SetDirection(Vector3 vec)
    {
        // 向き
        float Direction_scale_x = 0;
        if (vec.x > 0)
        {
            Direction_scale_x = 1;
        }
        else if (vec.x < 0)
        {
            Direction_scale_x = -1;
        }

        if (Direction_scale_x != 0)
        {
            Vector3 tempScale = transform.localScale;
            tempScale.x = Direction_scale_x;
            transform.localScale = tempScale;
        }
    }
}
