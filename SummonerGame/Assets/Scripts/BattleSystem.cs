using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public UnitBattleData playerBattleData; //連結當下玩家單位的資料
    public UnitBattleData enemyBattleData; //連結敵人單位的資料
    public SkillManager skillManager; //連結技能庫

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Skill_1() {
        //在unit data中找到技能ID
        //把技能ID傳進在unit battle skill
        //把敵人資料以及我方資料傳進unit battle skill
        skillManager.UsingSkill("kick", playerBattleData, enemyBattleData);
    }
}
