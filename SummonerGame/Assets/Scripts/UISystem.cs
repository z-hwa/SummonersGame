using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISystem : MonoBehaviour
{
    [SerializeField]private SkillSystem skillManager; //用於連接技能管理者
    private float diff = 0.0001f; //用於除法的/0例外預防

    [Header("玩家屬性")]
    [SerializeField] private TextMeshProUGUI playerUnitName;  //名稱
    [SerializeField] private TextMeshProUGUI playerLevel; //等級
    [SerializeField] private Image playerAttribute;   //屬性圖案

    [SerializeField] private TextMeshProUGUI playerHp;    //血量
    [SerializeField] private Slider playerHpSlider;   //血量滑條

    [Header("敵人屬性")]
    [SerializeField] private TextMeshProUGUI enemyUnitName;
    [SerializeField] private TextMeshProUGUI enemyLevel;
    [SerializeField] private Image enemyAttribute;

    [SerializeField] private TextMeshProUGUI enemyHp;    //血量
    [SerializeField] private Slider enemyHpSlider;   //血量滑條

    [Header("按鍵操作系統")]
    [SerializeField] private TextMeshProUGUI[] skill = new TextMeshProUGUI[4];

    //顯示玩家技能的名字
    public void ShowSkillName(int skillNum, int[] skillID)
    {
        //skillNum=4代表傳進來的技能ID數量
        for(int i=0;i<skillNum;i++)
        {
            skill[i].text = skillManager.CheckSkillName(skillID[i]);
        }
    } 

    //顯示玩家生命值
    public void ShowPlayerHp(int nowHp, int maxHp)
    {
        playerHp.text = nowHp + "/" + maxHp;    //599/599 生命值顯示
        playerHpSlider.value = (nowHp+diff) / (maxHp+diff); //滑條展示
    }

    //顯示敵人生命值
    public void ShowEnemyHp(int nowHp, int maxHp)
    {
        enemyHp.text = nowHp + "/" + maxHp;    //599/599 生命值顯示
        enemyHpSlider.value = (nowHp + diff) / (maxHp + diff); //滑條展示
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
