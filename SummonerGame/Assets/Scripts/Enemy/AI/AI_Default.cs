using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Default : MonoBehaviour
{
    //獲取當前狀態
    private UnitBattleData player;
    private UnitBattleData enemy;

    /*生命高於對手 發動技能0
     * 否則 發動技能1
     */

    public int AI(UnitBattleData player, UnitBattleData enemy)
    {
        int skillId = 0;

        if(enemy.nowAbilityValue[5] >= player.nowAbilityValue[5])
        {
            skillId = enemy.nowSkillID[0];
        }else
        {
            skillId = enemy.nowSkillID[1];
        }

        return skillId;
    }
}
