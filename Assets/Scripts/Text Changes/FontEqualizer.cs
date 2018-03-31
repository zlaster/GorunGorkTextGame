﻿using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Script que solo cambia el font de los TextMeshes
/// </summary>
[ExecuteInEditMode]
public class FontEqualizer : MonoBehaviour {

    public TMP_FontAsset font;

    //Enviado a FontReceiver
    public delegate void ChangeFont(TMP_FontAsset newFont);
    public static event ChangeFont OnFontChange;


    private void LateUpdate()
    {

        if (OnFontChange != null)
        {
            OnFontChange(font);
        }
        
    }
}