﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "unit", menuName ="Unit/New Unit")]
public class UnitObject : ScriptableObject
{
    /*ScriptObject
        作為腳色資料的模板
    */
    [Header("實際數值")]
    public int unitID;  //腳色ID
    public string unitName; //腳色名字
    public int level;   //當前等級
    public Attribute attribute; //屬性

    public Character character; //個性(影響成長值的分配)
    public float racialValue;  //單位值(影響每升一級獲得的成長值)
    public int[] potnetial = new int[6];    //六個潛能值(物攻、特攻、物防、特防、速度、生命)
    public float qualification;    //資質(影響每升一級獲得的成長值)

    public int[] usingAbilityValue = new int[6]; //現在的能力值
    public int[] defaultAbilityValue = new int[6]; //原先的基礎能力值區間

    public int[] skillTree = new int[20]; //紀錄該腳色所有的技能
    public int[] usingSkillID = new int[4];   //當前裝備的四個技能

    [Header("原生數值區間")]
    public int[] levelRange = new int[2];   //野生的等級區間
    public int[] abilityValueRange = new int[12];   //原生的基礎能力值區間

    [Header("圖片素材")]
    public Sprite[] unitImage = new Sprite[2];    /* 腳色正反照片
                                                 * 0是背面
                                                 * 1是正面
                                                 */
}
