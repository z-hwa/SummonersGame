using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "characterSprite", menuName = "Unit/New Sprite")]
public class SpriteObject : ScriptableObject
{
    [Header("靜態素材")]
    public Sprite frontView;
    public Sprite rearView;
}
