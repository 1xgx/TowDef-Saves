using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenuSubButtons : MonoBehaviour
{
    public GameObject[] BattleMenu;
    public void ShowSubMenu(bool active)
    {

        if (active)
        {
            for (int i = 0; i < BattleMenu.Length; i++)
            {
                BattleMenu[i].active = true;
            }
        }
        else
        {
            for (int i = 0; i < BattleMenu.Length; i++)
            {
                BattleMenu[i].active = false;
            }
        }

    }
}
