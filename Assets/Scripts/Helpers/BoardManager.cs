using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[Serializable]
public class BoardManager
{
    public bool isGap;
    public bool isDark;
    public SpriteRenderer boardPrefab;
    public TMP_FontAsset boardFont;
    public Color32 systemColor;
    public Color32 userColor;
    public Color cameraColor;
    public bool isActive = false;
    public GameObject boardLayout;
}
