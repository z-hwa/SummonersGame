using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager: MonoBehaviour
{
    //根據ID施放技能
    public void UsingSkill(string skillID, UnitBattleData player, UnitBattleData enemy)
    {
        switch (skillID)
        {
            case "kick":
                UseSkill_kick(player, enemy);
                break;
            default:
                Debug.Log("查無檢索");
                break;
        }
    }

    /*根據ID查詢技能
    //要丟入一個查詢包的指針
    //並在函數中將值賦回給查詢包
    //回傳空
    public void CheckSkill(int skillID)
    {
        switch (skillID)
        {
            case 0:
                CheckSkill_0();
                break;
            default:
                Debug.Log("查無檢索");
                break;
        }
    }
    */

    public void UseSkill_kick(UnitBattleData player, UnitBattleData enemy)
    {
        Debug.Log(player.unitName + " kick " + enemy.unitName);
        Debug.Log("skill_0!!!");
        return;
    }

    /*
    public void CheckSkill_0()
    {
        return;
    }

    */
}