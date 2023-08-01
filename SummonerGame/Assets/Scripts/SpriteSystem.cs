using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSystem : MonoBehaviour
{
    [SerializeField] private SpriteObject[] characterSprite = new SpriteObject[10];
    [SerializeField] private Sprite spriteDefault;

    //載入腳色圖
    //預設載入為 spriteDefault
    public Sprite GetSprite(int unitID, string aspect)
    {
        Sprite sprite = spriteDefault;  //預設載入

        if(aspect == "front")
        {
            //正面
            sprite = LoadFrontByID(unitID);
        }
        else if(aspect == "rear")
        {
            //背面
            sprite = LoadRearByID(unitID);
        }else
        {
            Debug.LogError("腳色圖像載入出現問題...");
        }

        return sprite;
    }
    
    //根據腳色ID 回傳正面圖
    private Sprite LoadFrontByID(int unitID)
    {
        switch (unitID)
        {
            case 0:
                return characterSprite[0].frontView;
            default:
                return default;
        }
    }

    //根據腳色ID 回傳背面圖
    private Sprite LoadRearByID(int unitID)
    {
        switch (unitID)
        {
            case 0:
                return characterSprite[0].rearView;
            default:
                return default;
        }
    }
}
