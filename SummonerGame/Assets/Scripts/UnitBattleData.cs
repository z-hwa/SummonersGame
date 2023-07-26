using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBattleData : MonoBehaviour
{
    public UnitObject unitData; //從來源獲取當前的出戰腳色object

    public string unitName; //名稱
    public int level;   //等級
    public Attribute attribute; //屬性
    public int[] nowAbilityValue = new int[6];  //當前能力值
    public string[] nowSkillName = new string[4];   //當前裝備的四個技能

    // Start is called before the first frame update
    void Start()
    {
        /*
         從背包系統載入
        UnitObject的腳色資料
         */

        //將從背包系統獲得的腳色資料 讀取進來
        unitName = unitData.unitName;
        level = unitData.level;
        attribute = unitData.attribute;
        unitData.nowAbilityValue.CopyTo(nowAbilityValue, 0);
        unitData.nowSkillID.CopyTo(nowSkillName, 0);
    }
}
