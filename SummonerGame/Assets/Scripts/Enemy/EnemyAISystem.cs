using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 根據場上情況
 * 回傳該使用的技能ID
 */
public class EnemyAISystem : MonoBehaviour
{
    [Header("當前場上狀態")]
    [SerializeField] private UnitBattleData player;
    [SerializeField] private UnitBattleData enemy;

    [Header("AI腳本")]
    [SerializeField] private AI_Default aI_Default;     //預設模式腳本

    public int AIModeUsing(int aiID)
    {
        int skillId = 0;

        switch(aiID)
        {
            default:
                skillId = aI_Default.AI(player, enemy);     //預設模式
                break;
        }

        return skillId; //回傳決定好的技能
    }
}
