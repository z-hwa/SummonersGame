using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public UnitBattleData playerBattleData; //連結當下玩家單位的資料
    public UnitBattleData enemyBattleData; //連結敵人單位的資料

    public SkillManager skillManager; //連結技能庫
    public UISystem uISystem; //連接UI系統(處理初始化的資料顯示)

    // Start is called before the first frame update
    void Start()
    {
        uISystem.ShowPlayerData(playerBattleData.unitName, playerBattleData.level, playerBattleData.attribute);
        uISystem.ShowEnemyData(enemyBattleData.unitName, enemyBattleData.level, enemyBattleData.attribute);
        uISystem.ShowSkillName(4, playerBattleData.nowSkillID);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Skill_0() {
        //在unit data中找到技能ID
        //把技能ID傳進在unit battle skill
        //把敵人資料以及我方資料傳進unit battle skill
        skillManager.UsingSkill(playerBattleData.nowSkillID[0], playerBattleData, enemyBattleData);
    }

    public void Skill_1()
    {
        skillManager.UsingSkill(playerBattleData.nowSkillID[1], playerBattleData, enemyBattleData);
    }

    public void Skill_2()
    {
        skillManager.UsingSkill(playerBattleData.nowSkillID[2], playerBattleData, enemyBattleData);
    }

    public void Skill_3()
    {
        skillManager.UsingSkill(playerBattleData.nowSkillID[3], playerBattleData, enemyBattleData);
    }
}
