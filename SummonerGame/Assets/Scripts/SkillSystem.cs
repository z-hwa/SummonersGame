﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem: MonoBehaviour
{
    [SerializeField]private AttributeSystem attributeSystem; //屬性系統(用於獲取屬性克制倍率)
    [SerializeField]private SkillData skillData; //技能資料(用於查詢)

    //根據ID施放技能
    public void UsingSkill(int skillID, UnitBattleData attaker, UnitBattleData defensor)
    {
        switch (skillID)
        {
            case 0:
                UseSkill_0(attaker, defensor);
                break;
            default:
                Debug.Log("查無檢索");
                break;
        }
    }

    //根據ID找到技能的名稱
    public string CheckSkillName(int skillID)
    {
        //確認當前資料庫長度
        //預設目標技能index為0
        int dataSize = skillData.data.Length;
        int targetSkill_Index = 0;

        //遍歷資料庫查詢技能
        for(int i=0;i<dataSize;i++)
        {
            if(skillID == skillData.data[i].ID)
            {
                //找到 設定目標index
                targetSkill_Index = i;
                break;
            }else
            {
                //沒有找到
                Debug.Log("do not find the " + skillID + " ID skill in data");
            }
        }

        return skillData.data[targetSkill_Index].skillName;
    }

    //幽影踢擊
    private void UseSkill_0(UnitBattleData player, UnitBattleData enemy)
    {
        //基數
        float fixeDmg = player.nowAbilityValue[0] * 0.5f; //固定傷害
        float physicalDmg = player.nowAbilityValue[0] * 0.3f; //物理傷害
        float shadowDmg = player.nowAbilityValue[1] * 0.3f; //暗影系傷害

        //攻防比值
        physicalDmg *= (player.nowAbilityValue[0] / enemy.nowAbilityValue[2]); //物理
        shadowDmg *= (player.nowAbilityValue[1] / enemy.nowAbilityValue[3]); //屬性

        //屬性克制
        shadowDmg *= attributeSystem.shadowEffect[(int)enemy.attribute];

        //扣血
        int totalDmg = (int)(physicalDmg + shadowDmg + fixeDmg);
        enemy.nowAbilityValue[5] -= totalDmg;

        //debug
        Debug.Log(player.unitName + " 對 " + enemy.unitName + " 發動幽影踢擊");
        Debug.Log("fixeDmg=" + fixeDmg + "\nphysicalDmg=" + physicalDmg + "\nshadowDmg=" + shadowDmg);
        return;
    }
}