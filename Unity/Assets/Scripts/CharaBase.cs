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
    [SerializeField] private bool guardFlg = false;

    [SerializeField] private List<AnimatorControllerParameter> animatorControllerParameterList = null;

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
        GuardAnimation(guardFlg);

        guardFlg = false;
    }

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        trans = transform;

        // TODO:キャラの読み込みや大きさ、当たり判定をデータによって仕込む。

        // Animatorに登録されているパラメータを保持
        animatorControllerParameterList = new List<AnimatorControllerParameter>(anim.parameters);
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
    /// 攻撃
    /// </summary>
    /// <param name="vec"></param>
    public virtual void Attack(Vector3 vec)
    {
        if (guardFlg) return;

        AttackAnimation();

        SetDirection(vec);
    }

    /// <summary>
    /// 防御
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="_guard_flg"></param>
    public virtual void Guard(Vector3 vec)
    {
        guardFlg = true;
        SetDirection(vec);
    }

    /// <summary>
    /// 移動アニメーション(速度要素によって)
    /// </summary>
    /// <param name="_speed"></param>
    public virtual void MoveAnimation(float _speed)
    {
        const string animeName = "Speed";
        if (!animatorControllerParameterList.Exists(x => x.name.Equals(animeName)))
        {
            return;
        }

        if (guardFlg)
        {
            anim.SetFloat(animeName, 0);
        }
        else
        {
            anim.SetFloat(animeName, _speed);
        }
    }

    /// <summary>
    /// 攻撃アニメーション
    /// </summary>
    public virtual void AttackAnimation()
    {
        const string animeName = "Attack";
        if (!animatorControllerParameterList.Exists(x => x.name.Equals(animeName)))
        {
            return;
        }
        anim.SetTrigger(animeName);
    }

    /// <summary>
    /// 防御アニメーション
    /// </summary>
    /// <param name="_guard_flg"></param>
    public virtual void GuardAnimation(bool _guard_flg)
    {
        const string animeName = "Guard";
        if (!animatorControllerParameterList.Exists(x => x.name.Equals(animeName)))
        {
            return;
        }
        anim.SetBool(animeName, _guard_flg);
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

    /// <summary>
    /// キャラのポジション取得
    /// </summary>
    public virtual Vector3 GetCharaPosition()
    {
        return transform.position;
    }
}
