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
    LOST
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleState battleState; //當前系統狀態

    [SerializeField] private UnitBattleData playerBattleData; //連結當下玩家單位的資料
    [SerializeField] private UnitBattleData enemyBattleData; //連結敵人單位的資料

    [SerializeField] private SkillManager skillManager; //連結技能庫
    [SerializeField] private UISystem uISystem; //連接UI系統(處理初始化的資料顯示)

    [SerializeField] private Button[] skillButton = new Button[4]; //四個技能按鍵

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

        Debug.Log("戰鬥場景加載中......");
        yield return new WaitForSeconds(2f);    //等待2秒 遊戲載入時間
        Debug.Log("場景加載完成 戰鬥開始!");

        PreemtiveTest(); //先進入先手pk
    }

    //玩家回合
    IEnumerator PlayerTurn()
    {
        bool isEnemyLose = false;   //檢測敵人是否輸了
        bool isUsingSkill = false;  //檢測是否施放技能
        OnInteractable_SkillButton();   //激活技能按鍵

        Debug.Log("你的回合 請選擇技能......");

        yield return new WaitUntil(() => isUsingSkill); //等待直到施放技能
        yield return new WaitForSeconds(2f);    //等待技能動畫或技能效果實現

        isEnemyLose = CheckEnemyIsLose();   //檢測敵人是否輸了

        if(isEnemyLose == true)
        {
            battleState = BattleState.WON;
            //BattleWon();
        }else
        {
            //StartCoroutine(EnemyTurn());  //敵人回合
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
    private void RenewUnitData()
    {

    }

    /* 檢測敵人是否輸掉
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
        skillManager.UsingSkill(playerBattleData.nowSkillID[0], playerBattleData, enemyBattleData);
        OffInteractable_SkillButton();
    }
    public void Skill_1()
    {
        skillManager.UsingSkill(playerBattleData.nowSkillID[1], playerBattleData, enemyBattleData);
        OffInteractable_SkillButton();
    }
    public void Skill_2()
    {
        skillManager.UsingSkill(playerBattleData.nowSkillID[2], playerBattleData, enemyBattleData);
        OffInteractable_SkillButton();
    }
    public void Skill_3()
    {
        skillManager.UsingSkill(playerBattleData.nowSkillID[3], playerBattleData, enemyBattleData);
        OffInteractable_SkillButton();
    }
}
