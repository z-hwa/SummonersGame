using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISystem : MonoBehaviour
{
    public SkillManager skillManager; //用於連接技能管理者

    [Header("玩家屬性")]
    public TextMeshProUGUI playerUnitName;
    public TextMeshProUGUI playerLevel;
    public Image playerAttribute;

    [Header("敵人屬性")]
    public TextMeshProUGUI enemyUnitName;
    public TextMeshProUGUI enemyLevel;
    public Image enemyAttribute;

    [Header("按鍵操作系統")]
    public TextMeshProUGUI[] skill = new TextMeshProUGUI[4];

    //顯示玩家技能的名字
    public void ShowSkillName(int skillNum, int[] skillID)
    {
        //skillNum=4代表傳進來的技能ID數量
        for(int i=0;i<skillNum;i++)
        {
            skill[i].text = skillManager.CheckSkillName(skillID[i]);
        }
    } 

    //展示玩家以及敵人的名稱、等級、屬性資訊
    public void ShowPlayerData(string unitName, int level, Attribute attribute)
    {
        playerUnitName.text = unitName;
        playerLevel.text = "lv." + level.ToString();
        /*
         * 記得傳遞屬性的圖片
         * 等建好屬性圖庫
         * */
    }

    public void ShowEnemyData(string unitName, int level, Attribute attribute)
    {
        enemyUnitName.text = unitName;
        enemyLevel.text = "lv." + level.ToString();
        /*
         * 記得傳遞屬性的圖片
         * 等建好屬性圖庫
         * */
    }
}
