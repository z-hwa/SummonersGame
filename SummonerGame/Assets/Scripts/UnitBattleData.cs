using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBattleData : MonoBehaviour
{
    [SerializeField] private UnitObject unitData; //從來源獲取當前的出戰腳色object

    [Header("腳色資料")]
    public int unitID; //ID
    public string unitName; //名稱
    public int level;   //等級
    public Attribute attribute; //屬性
    public int[] initAbilityValue = new int[6]; //原始能力值
    public int[] nowAbilityValue = new int[6];  //當前能力值
    public int[] nowSkillID = new int[4];   //當前裝備的四個技能

    [Header("AI資料")]
    public int enemyAIID; //野生狀態下的AI模式

    // Start is called before the first frame update
    //遊戲資料初始化
    public void LoadUnitData()
    {
        /*
         從背包系統載入
        UnitObject的腳色資料
         */

        //將從背包系統獲得的腳色資料 讀取進來
        unitID = unitData.unitID;
        unitName = unitData.unitName;
        level = unitData.level;
        attribute = unitData.attribute;
        unitData.usingAbilityValue.CopyTo(initAbilityValue, 0);   //複製原始資料
        unitData.usingAbilityValue.CopyTo(nowAbilityValue, 0);    //複製使用的當前資料
        unitData.usingSkillID.CopyTo(nowSkillID, 0);

        //設定AI
        enemyAIID = unitID;   //預設為當前腳色編號
    }
}
