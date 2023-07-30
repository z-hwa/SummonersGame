using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    Serious = 1 //認真
}

public class CharacterSystem : MonoBehaviour
{
    //個性對於能力值的分配影響
    public float[] characterDefault = new float[6];
    public float[] seriousEffect = new float[6];

    public float[] CharacterEffect(int characterID)
    {
        switch(characterID)
        {
            case 1:
                return seriousEffect;
            default:
                return characterDefault;
        }
    }
}
