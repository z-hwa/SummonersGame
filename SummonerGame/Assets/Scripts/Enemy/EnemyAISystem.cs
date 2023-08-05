using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 根據場上情況
 * 回傳該使用的技能ID
 */
public class EnemyAISystem : MonoBehaviour
{
    [Header("連結系統")]
    [SerializeField] private BattleSystem battleSystem;  //戰鬥主系統

    [Header("當前場上狀態")]
    [SerializeField] private UnitBattleData player;
    [SerializeField] private UnitBattleData enemy;

    [Header("AI腳本")]
    [SerializeField] private AI_Default aI_Default;     //預設模式腳本

    public int AIModeUsing(int aiID)
    {
        int skillId = 0;    //預設AI為編號0
        SetUnitData();  //確認連結的精靈狀態

        switch(aiID)
        {
            default:
                skillId = aI_Default.AI(player, enemy);     //預設模式
                break;
        }

        return skillId; //回傳決定好的技能
    }

    //設定單位狀態(確保更換精靈以後 有連結到)
    private void SetUnitData()
    {
        player = battleSystem.playerBattleData;
        enemy = battleSystem.enemyBattleData;
    }
}
