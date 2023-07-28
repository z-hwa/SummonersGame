using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//屬性的Enum
public enum Attribute
{
    Shadow = 1, //暗影
    Fire = 2,   //火
    Water = 3,      //水
    Grass = 4       //草
}

public class AttributeSystem : MonoBehaviour
{
    //暗影屬性的克制表
    public double[] shadowEffect = {0, 1f,  0.5f, 2f, 2f};
}
