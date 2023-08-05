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
    public UnitBattleData[] unitInPackage = new UnitBattleData[6];    //背包中的單位狀況
    public UnitBattleData playerBattleData; //連結當下背包中玩家單位的資料
    public UnitBattleData enemyBattleData; //連結敵人單位的資料

    [Header("系統")]
    [SerializeField] private PackageSystem packageSystem; //連結單位背包
    [SerializeField] private SkillSystem skillSystem; //連結技能庫
    [SerializeField] private UISystem uISystem; //連接UI系統(處理初始化的資料顯示)
    [SerializeField] private EnemyAISystem enemyAISystem;   //連結敵人AI系統

    [Header("UI相關")]
    [SerializeField] private GameObject unitDect;   //單位甲板
    [SerializeField] private GameObject skillDect;  //技能甲板
    [SerializeField] private GameObject otherDect;  //其他甲板

    [SerializeField] private GameObject rebackFromUnitButton; //從精靈面板返回
    [SerializeField] private GameObject changeButton;   //切換甲板按鍵
    [SerializeField] private Button[] changeUnitButtom = new Button[6]; //6個精靈的按鍵
    [SerializeField] private Button[] skillButton = new Button[4]; //四個技能按鍵

    private int roundNum = 1;   //當前回合數
    private bool isUsingSkill = false;  //檢測回合中是否施放技能
    private int nowUnitInPackage = 0;   //當前出戰精靈 在背包中的編號

    // Start is called before the first frame update
    void Start()
    {
        //前置
        packageSystem.SetUnitData();    //測試中需要初始化背包中的單位
        SetUnitDataInPackage(); //設置玩家背包中的精靈資料


        //戰鬥處理
        battleState = BattleState.START;    //狀態設為開始
        StartCoroutine(SetupBattle());  //設置戰鬥狀態
    }

    //加載戰鬥設置
    IEnumerator SetupBattle()
    {
        playerBattleData = unitInPackage[0]; //玩家單位 派出背包中順序為0的精靈
        enemyBattleData.LoadUnitData(); //載入敵人單位的資料

        LoadUIData();   //載入所有UI資訊

        //重設
        roundNum = 0;   //回合數
        nowUnitInPackage = 0;   //當前出戰為背包中首隻精靈

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
        Debug.Log("你的回合 請選擇......");


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
        int playerStatus = 0;   /* 標示玩家是否戰敗
                                 * 0 未戰敗 可繼續戰鬥
                                 * 1 戰敗
                                 * 2 當前單位戰敗 但可以更換單位繼續戰鬥
                                 */
        int skillID = -1;   //將要進行的技能ID
        Debug.Log("敵方回合......");
        Debug.Log("敵方發起攻擊");
            
        skillID = enemyAISystem.AIModeUsing(enemyBattleData.enemyAIID);     //從AI系統 獲得敵人行為模式
        skillSystem.UsingSkill(skillID, enemyBattleData, playerBattleData);     //發動技能

        yield return new WaitForSeconds(2f);    //等待技能動畫或技能效果實現

        RenewUIData(); //更新UI

        playerStatus = CheckPlayerIsLose();   //檢測玩家是否輸了

        /* 兩種情況的檢測
         *      進入敵方回合
         *      勝利
         */
        if (playerStatus == 1)
        {
            Debug.Log("你輸拉");
            battleState = BattleState.LOSE;
            //BattleLose();
        }
        else if(playerStatus == 0)
        {    
            battleState = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());  //玩家回合
        }
        else if(playerStatus == 2)
        {
            Debug.Log("當前單位戰敗 請更換單位!");
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

    private void LoadUIData()
    {
        //載入初始化的遊戲UI資訊
        uISystem.ShowPlayerData();
        uISystem.ShowEnemyData();

        uISystem.ShowSkillName();

        uISystem.ShowPlayerHp();
        uISystem.ShowEnemyHp();

        uISystem.ShowPlayerSprite();
        uISystem.ShowEnemySprite();
    }

    //更新每個回合後 場上的單位狀態
    private void RenewUIData()
    {
        //更新血量
        uISystem.ShowPlayerHp();
        uISystem.ShowEnemyHp();

        //更新背包血量
        uISystem.ShowUnitInPackage();

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
    private int CheckPlayerIsLose()
    {
        int result = 1;     //預設1 玩家輸了

        //檢測血量
        if (playerBattleData.nowAbilityValue[5] <= 0)
        {
            result = 1;

            //檢查背包中 是否仍有可戰鬥精靈
            foreach (UnitBattleData unitData in unitInPackage)
            {
                if(unitData.nowAbilityValue[5] > 0)
                {
                    result = 2; //沒有輸 但是需要更換精靈
                    break;
                }
            }
        }
        else if (playerBattleData.nowAbilityValue[5] > 0)
        {
            result = 0; //沒有輸 繼續戰鬥
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
        if(playerBattleData.nowAbilityValue[5] > 0)
        {
            foreach (Button button in skillButton)
            {
                button.interactable = true;
            }
        }

        //激活6個 精靈切換鍵
        foreach(Button button in changeUnitButtom)
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

        //關閉6個 精靈切換鍵
        foreach(Button button in changeUnitButtom)
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

    //面板切換鍵
    public void ChangeDeck()
    {
        // 如果技能面板激活、其他面板關閉 則切換
        if(skillDect.activeSelf == true && otherDect.activeSelf == false)
        {
            skillDect.SetActive(false);
            otherDect.SetActive(true);
        }else if(otherDect.activeSelf == true && skillDect.activeSelf == false)
        {
            otherDect.SetActive(false);
            skillDect.SetActive(true);
        }else
        {
            Debug.LogError("甲板更換出現錯誤......");
        }
    }

    //從精靈面板返回
    public void RebackDeck()
    {
        if(unitDect.activeSelf == true && otherDect.activeSelf == false)
        {
            unitDect.SetActive(false);  //關閉單位面板
            otherDect.SetActive(true);
            rebackFromUnitButton.SetActive(false);  //關閉技能返回介面
            changeButton.SetActive(true);    //開啟切換甲板按鍵
        }else
        {
            Debug.LogError("從精靈面板返回 出現問題......");
        }
    }

    //精靈鍵
    public void ChangeUnit()
    {
        if(unitDect.activeSelf == false && otherDect.activeSelf == true)
        {
            unitDect.SetActive(true);  //開啟單位面板
            otherDect.SetActive(false);
            rebackFromUnitButton.SetActive(true);  //開啟技能返回介面
            changeButton.SetActive(false);  //關閉切換甲板按鍵
            
            uISystem.ShowUnitInPackage();   //顯示背包UI
        }
    }

    //設定背包中單位的資料
    public void SetUnitDataInPackage()
    {
        for(int i=0;i<6;i++)
        {
            unitInPackage[i].SetUnitOB(packageSystem.unitPackage[i]);   //將背包中的精靈OB載入
            unitInPackage[i].LoadUnitData();    //載入背包中的精靈資料
        }
    }

    //選擇切換後的上場精靈
    public void ChooseUnit(int id)
    {
        //確認要更換的不是當前出戰的精靈
        if(id != nowUnitInPackage)
        {
            switch (id)
            {
                case 0:
                    playerBattleData = unitInPackage[0];
                    nowUnitInPackage = 0;
                    break;
                case 1:
                    playerBattleData = unitInPackage[1];
                    nowUnitInPackage = 1;
                    break;
                case 2:
                    playerBattleData = unitInPackage[2];
                    nowUnitInPackage = 2;
                    break;
                case 3:
                    playerBattleData = unitInPackage[3];
                    nowUnitInPackage = 3;
                    break;
                case 4:
                    playerBattleData = unitInPackage[4];
                    nowUnitInPackage = 4;
                    break;
                case 5:
                    playerBattleData = unitInPackage[5];
                    nowUnitInPackage = 5;
                    break;
                default:
                    break;
            }

            OffInteractable_SkillButton();  //關閉玩家回合的按鍵
            isUsingSkill = true;    //標示該回合已經使用技能
            LoadUIData();   //更新場上UI
        }
        else if (playerBattleData.nowAbilityValue[5] <= 0)
        {
            Debug.Log("當前精靈已戰敗...");
        }
        else if(id == nowUnitInPackage)
        {
            Debug.Log("當前出戰精靈 不可重複上場");
        }
        else
        {
            Debug.LogError("出現切換精靈時 的未知狀況");
        }
    }
}
