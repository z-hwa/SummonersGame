using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSystem : MonoBehaviour
{
    [SerializeField] private UnitObject test;   //測試單位
    public UnitObject[] unitPackage = new UnitObject[6];   //單位背包

    //載入測試中 預設的單位
    public void SetUnitData()
    {
        for(int i = 0;i < 6;i++)
        {
            unitPackage[i] = test;
        }
    }
}
