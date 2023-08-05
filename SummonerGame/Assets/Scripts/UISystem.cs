using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISystem : MonoBehaviour
{
    private float diff = 0.0001f; //用於除法的/0例外預防

    [Header("其他系統")]
    [SerializeField] private SkillSystem skillSystem; //用於連接技能管理者
    [SerializeField] private BattleSystem battleSystem; //用於連結戰鬥主系統

    [Header("玩家屬性")]
    [SerializeField] private TextMeshProUGUI playerUnitName;  //名稱
    [SerializeField] private TextMeshProUGUI playerLevel; //等級
    [SerializeField] private Image playerAttribute;   //屬性圖案
    [SerializeField] private SpriteRenderer playerSprite;   //玩家腳色圖

    [SerializeField] private TextMeshProUGUI playerHp;    //血量
    [SerializeField] private Slider playerHpSlider;   //血量滑條

    [Header("敵人屬性")]
    [SerializeField] private TextMeshProUGUI enemyUnitName;
    [SerializeField] private TextMeshProUGUI enemyLevel;
    [SerializeField] private Image enemyAttribute;
    [SerializeField] private SpriteRenderer enemySprite;    //敵人腳色圖

    [SerializeField] private TextMeshProUGUI enemyHp;    //血量
    [SerializeField] private Slider enemyHpSlider;   //血量滑條

    [Header("按鍵操作系統")]
    [SerializeField] private TextMeshProUGUI[] skill = new TextMeshProUGUI[4];

    [Header("精靈背包")]
    [SerializeField] private TextMeshProUGUI[] unitName = new TextMeshProUGUI[6];   //名稱
    [SerializeField] private TextMeshProUGUI[] unitLevel = new TextMeshProUGUI[6];   //等級
    [SerializeField] private TextMeshProUGUI[] unitHp = new TextMeshProUGUI[6];   //生命值
    [SerializeField] private Image[] unitImage = new Image[6];   //腳色圖片

    /* 顯示單位的圖片
     * 玩家> 背面
     * 敵人> 正面
     */
    public void ShowPlayerSprite()
    {
        playerSprite.sprite = battleSystem.playerBattleData.unitImage[0];
    }
    public void ShowEnemySprite()
    {
        enemySprite.sprite = battleSystem.enemyBattleData.unitImage[1];
    }

    //顯示玩家技能的名字
    public void ShowSkillName()
    {
        int skillNum = battleSystem.playerBattleData.nowSkillID.Length;

        //skillNum=4代表傳進來的技能ID數量
        for(int i=0;i<skillNum;i++)
        {
            skill[i].text = skillSystem.CheckSkillName(battleSystem.playerBattleData.nowSkillID[i]);
        }
    } 

    //顯示玩家生命值
    public void ShowPlayerHp()
    {
        int nowHp = battleSystem.playerBattleData.nowAbilityValue[5];
        int maxHp = battleSystem.playerBattleData.initAbilityValue[5];

        playerHp.text = nowHp + "/" + maxHp;    //599/599 生命值顯示
        playerHpSlider.value = (nowHp+diff) / (maxHp+diff); //滑條展示
    }

    //顯示敵人生命值
    public void ShowEnemyHp()
    {
        int nowHp = battleSystem.enemyBattleData.nowAbilityValue[5];
        int maxHp = battleSystem.enemyBattleData.initAbilityValue[5];

        enemyHp.text = nowHp + "/" + maxHp;    //599/599 生命值顯示
        enemyHpSlider.value = (nowHp + diff) / (maxHp + diff); //滑條展示
    }

    //展示玩家以及敵人的名稱、等級、屬性資訊
    public void ShowPlayerData()
    {
        playerUnitName.text = battleSystem.playerBattleData.unitName;
        playerLevel.text = "lv." + battleSystem.playerBattleData.level.ToString();
        /*
         * 記得傳遞屬性的圖片
         * 等建好屬性圖庫
         * */
    }
    public void ShowEnemyData()
    {
        enemyUnitName.text = battleSystem.enemyBattleData.unitName;
        enemyLevel.text = "lv." + battleSystem.enemyBattleData.level.ToString();
        /*
         * 記得傳遞屬性的圖片
         * 等建好屬性圖庫
         * */
    }

    //顯示背包中的單位資料
    public void ShowUnitInPackage()
    {
        for(int i = 0;i<6;i++)
        {
            unitName[i].text = battleSystem.unitInPackage[i].unitName;  //設置名稱
            unitLevel[i].text = "lv." + battleSystem.unitInPackage[i].level.ToString();    //設置等級
            unitHp[i].text = battleSystem.unitInPackage[i].nowAbilityValue[5].ToString() + "/" + battleSystem.unitInPackage[i].initAbilityValue[5].ToString();  //設置生命值
            unitImage[i].sprite = battleSystem.unitInPackage[i].unitImage[1];  //顯示腳色正面
        }
    }
}
