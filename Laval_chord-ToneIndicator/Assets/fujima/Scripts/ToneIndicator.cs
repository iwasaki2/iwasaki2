using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToneIndicator : MonoBehaviour
{
    float targetFreq;
    float range;
    private string sex; // 性別を格納する変数

    void Start()
    {
        targetFreq = 440.0f;
        range = 25.0f;
    }

    public void SetTargetFreq(float f)
    {
        targetFreq = f;
    }

    public void SetFreqRange(float r)
    {
        range = r;
    }

    public void SetCurrentFreq(float f)
    {
        float v;
        v = (f - targetFreq) / range;  // v is from -1.0 to 1.0
        v = (v + 1) / 2;               // v is from 0.0 to 1.0
        v = Mathf.Clamp(v, 0, 1);      // 0〜1 の範囲に制限

        gameObject.GetComponent<Slider>().value = v;
    }
}
