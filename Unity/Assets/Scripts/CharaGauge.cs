using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaGauge : MonoBehaviour
{
    [SerializeField] private Slider HpGaugeSlider;
    [SerializeField] private CharaBase charaBase;

    // Start is called before the first frame update
    void Start()
    {
        HpGaugeSlider.value = charaBase.Hp / charaBase.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        HpGaugeSlider.value = charaBase.Hp / charaBase.maxHp;
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
