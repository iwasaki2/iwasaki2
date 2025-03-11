using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToneController : MonoBehaviour
{
    public ToneIndicator ti;

    void Start()
    {
        if (ti == null)
        {
            Debug.LogError("ToneIndicator がアタッチされていません！");
        }
    }

    void Update()
    {
        if (ti == null) return;

        if (OVRInput.Get(OVRInput.Button.One))
            ti.SetSex("Female"); // `char` → `string` に変更
        if (OVRInput.Get(OVRInput.Button.Two))
            ti.SetSex("Male");
    }
}
