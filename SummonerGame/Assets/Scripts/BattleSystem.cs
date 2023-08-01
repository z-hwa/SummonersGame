using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//系統可能的狀態
public enum BattleState {
    START = 0,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOSE
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleState battleState; //當前系統狀態

    [Header("單位資料")]
    [SerializeField] private UnitBattleData playerBattleData; //連結當下玩家單位的資料
    [SerializeField] private UnitBattleData enemyBattleData; //連結敵人單位的資料

    [Header("系統")]
    [SerializeField] private SkillSystem skillSystem; //連結技能庫
    [SerializeField] private UISystem uISystem; //連接UI系統(處理初始化的資料顯示)
    [SerializeField] private EnemyAISystem enemyAISystem;   //連結敵人AI系統

    [Header("UI相關")]
    [SerializeField] private Button[] skillButton = new Button[4]; //四個技能按鍵

    private int roundNum = 1;   //當前回合數
    private bool isUsingSkill = false;  //檢測回合中是否施放技能

    // Start is called before the first frame update
    void Start()
    {
        battleState = BattleState.START;    //狀態設為開始
        StartCoroutine(SetupBattle());  //設置戰鬥狀態
    }

    //加載戰鬥設置
    IEnumerator SetupBattle()
    {
        playerBattleData.LoadUnitData(); //載入玩家單位的資料
        enemyBattleData.LoadUnitData(); //載入敵人單位的資料

        //載入初始化的遊戲UI資訊
        uISystem.ShowPlayerData(playerBattleData.unitName, playerBattleData.level, playerBattleData.attribute);
        uISystem.ShowEnemyData(enemyBattleData.unitName, enemyBattleData.level, enemyBattleData.attribute);
        
        uISystem.ShowSkillName(4, playerBattleData.nowSkillID);
        
        uISystem.ShowPlayerHp(playerBattleData.nowAbilityValue[5], playerBattleData.initAbilityValue[5]);
        uISystem.ShowEnemyHp(enemyBattleData.nowAbilityValue[5], enemyBattleData.initAbilityValue[5]);

        uISystem.ShowPlayerSprite(playerBattleData.unitID);
        uISystem.ShowEnemySprite(enemyBattleData.unitID);

        //重設
        roundNum = 0;   //回合數

        Debug.Log("戰鬥場景加載中......");
        yield return new WaitForSeconds(2f);    //等待2秒 遊戲載入時間
        Debug.Log("場景加載完成 戰鬥開始!");

        PreemtiveTest(); //先進入先手pk
    }

    //玩家回合
    IEnumerator PlayerTurn()
    {
        bool isEnemyLose = false;   //標示敵人是否戰敗
        isUsingSkill = false;  //檢測是否施放技能

        OnInteractable_SkillButton();   //激活技能按鍵
        Debug.Log("你的回合 請選擇技能......");


        yield return new WaitUntil(() => isUsingSkill); //等待直到施放技能
        yield return new WaitForSeconds(2f);    //等待技能動畫或技能效果實現

        RenewUIData();    //更新UI

        isEnemyLose = CheckEnemyIsLose();   //檢測敵人是否輸了

        /* 兩種情況的檢測
         *      進入敵方回合
         *      勝利
         */
        if(isEnemyLose == true)
        {
            Debug.Log("你勝利了");
            battleState = BattleState.WON;
            //BattleWon();
        }else
        {
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());  //敵人回合
        }
    }

    //敵方回合
    IEnumerator EnemyTurn()
    {
        bool isPlayerLose = false;   //標示玩家是否戰敗
        int skillID = -1;   //將要進行的技能ID
        Debug.Log("敵方回合......");
        Debug.Log("敵方發起攻擊");
            
        skillID = enemyAISystem.AIModeUsing(enemyBattleData.enemyAIID);     //從AI系統 獲得敵人行為模式
        skillSystem.UsingSkill(skillID, enemyBattleData, playerBattleData);     //發動技能

        yield return new WaitForSeconds(2f);    //等待技能動畫或技能效果實現

        RenewUIData(); //更新UI

        isPlayerLose = CheckPlayerIsLose();   //檢測玩家是否輸了

        /* 兩種情況的檢測
         *      進入敵方回合
         *      勝利
         */
        if (isPlayerLose == true)
        {
            Debug.Log("你輸拉");
            battleState = BattleState.LOSE;
            //BattleLose();
        }
        else
        {    
            battleState = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());  //玩家回合
        }
    }

    //先手測試(加載完戰鬥設置後進入)
    private void PreemtiveTest()
    {
        //獲取雙方速度
        int playerSpeed = playerBattleData.nowAbilityValue[4];
        int enemySpeed = enemyBattleData.nowAbilityValue[4];

        //進行速度比較
        if(playerSpeed >= enemySpeed)
        {
            battleState = BattleState.PLAYERTURN;   //玩家回合
            StartCoroutine(PlayerTurn());
        }else if(playerSpeed < enemySpeed)
        {
            battleState = BattleState.ENEMYTURN;    //敵人回合
        }else
        {
            Debug.LogError("先手測試 出現速度比較錯誤");
        }
    }

    //更新每個回合後 場上的單位狀態
    private void RenewUIData()
    {
        uISystem.ShowPlayerHp(playerBattleData.nowAbilityValue[5], playerBattleData.initAbilityValue[5]);
        uISystem.ShowEnemyHp(enemyBattleData.nowAbilityValue[5], enemyBattleData.initAbilityValue[5]);
        roundNum++; //增加回合數
    }

    /* 檢測 敵人/玩家 是否輸掉
     * 全部的單位都輸了
     * other
     */
    private bool CheckEnemyIsLose()
    {
        bool result = false;    //預設否

        //檢測血量
        if(enemyBattleData.nowAbilityValue[5]<=0)
        {
            /* 待實作
             * 敵人背包中是否仍有其他精靈
             * 無 > 輸了
             */

            result = true;
        }
        else if(enemyBattleData.nowAbilityValue[5] > 0)
        {
            result = false;
        }
        else
        {
            Debug.LogError("檢測敵人輸掉狀態的血量判定 出現問題!");
        }

        return result;
    }
    private bool CheckPlayerIsLose()
    {
        bool result = true;     //預設否

        //檢測血量
        if (playerBattleData.nowAbilityValue[5] <= 0)
        {
            /* 待實作
             * 玩家背包中是否仍有其他精靈
             * 無 > 輸了
             */

            result = true;
        }
        else if (playerBattleData.nowAbilityValue[5] > 0)
        {
            result = false;
        }
        else
        {
            Debug.LogError("檢測玩家輸掉狀態的血量判定 出現問題!");
        }

        return result;
    }

    // 開啟/關閉 技能按鍵的互動
    private void OnInteractable_SkillButton()
    {
        //激活四個技能按鍵
        foreach (Button button in skillButton)
        {
            button.interactable = true;
        }
    }
    private void OffInteractable_SkillButton()
    {
        //關閉四個技能按鍵
        foreach (Button button in skillButton)
        {
            button.interactable = false;
        }
    }

    //四個玩家的技能鍵
    public void Skill_0(){
        //在unit data中找到技能ID
        //把技能ID傳進在unit battle skill
        //把敵人資料以及我方資料傳進unit battle skill
        skillSystem.UsingSkill(playerBattleData.nowSkillID[0], playerBattleData, enemyBattleData);
        OffInteractable_SkillButton();  //關閉技能按鍵的互動
        isUsingSkill = true;    //標示該回合已經使用技能
    }
    public void Skill_1()
    {
        skillSystem.UsingSkill(playerBattleData.nowSkillID[1], playerBattleData, enemyBattleData);
        OffInteractable_SkillButton();
        isUsingSkill = true;
    }
    public void Skill_2()
    {
        skillSystem.UsingSkill(playerBattleData.nowSkillID[2], playerBattleData, enemyBattleData);
        OffInteractable_SkillButton();
        isUsingSkill = true;
    }
    public void Skill_3()
    {
        skillSystem.UsingSkill(playerBattleData.nowSkillID[3], playerBattleData, enemyBattleData);
        OffInteractable_SkillButton();
        isUsingSkill = true;
    }
}
