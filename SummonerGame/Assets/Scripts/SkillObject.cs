using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "Unit/New Skill")]
public class SkillObject : ScriptableObject
{
    [Header("技能敘述")]
    public string skillName;
    public int ID;
    public int learnLevel;
    public int power;
    public int pp;

    [TextArea]  public string skillEffect;  //技能效果
    [TextArea]  public string skillIntro;   //技能敘述
}
