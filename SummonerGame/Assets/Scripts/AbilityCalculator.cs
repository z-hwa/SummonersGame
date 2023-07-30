using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCalculator : MonoBehaviour
{
    [SerializeField] private CharacterSystem characterSystem;   //個性系統

    [SerializeField] private UnitObject unitObject;     //要計算的單位
    [SerializeField] public int[] abilityValue = new int[6]; //現在的能力值

    //獲得總成長值
    private float totalGrowth()
    {
        float value = 0;

        //每級的成長值
        for (int level = 1; level <= unitObject.level; level++)
        {
            value += Mathf.Log10((float)(level * (0.1 * unitObject.qualification + 0.3 * unitObject.racialValue)));
        }

        return value;
    }

    public void Count()
    {
        unitObject.defaultAbilityValue.CopyTo(abilityValue, 0); //複製基礎能力值

        float growthValue;      //成長值
        float[] characterEffect = new float[6];     //個性影響分配

        growthValue = totalGrowth();    //獲得總成長值
        characterEffect = characterSystem.CharacterEffect((int)unitObject.character);   //根據個性 找到分配表

        for(int i = 0; i<6; i++)
        {
            abilityValue[i] += (int)(growthValue * characterEffect[i]);   //加上成長值
            abilityValue[i] += unitObject.potnetial[i]; //加上潛能
        }
    }
}
