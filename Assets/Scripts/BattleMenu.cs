using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    public GameObject[] _buttons;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _delayOfClick = 0.5f;
    public void HideOtherMenu(string ButtonClicked)
    {
        _audioSource.PlayOneShot(_audioClip);
        StartCoroutine(Delay());
        switch (ButtonClicked)
        {
            case "Ammo":
                GameObject[] newBut = _buttons[0].GetComponent<BattleMenuSubButtons>().BattleMenu;
                for (int i = 0; newBut.Length < i; i++)
                {
                    if (newBut[i].active == true) newBut[i].active = false;
                }
                _buttons[1].GetComponent<BattleMenuSubButtons>().ShowSubMenu(false);
                _buttons[2].GetComponent<BattleMenuSubButtons>().ShowSubMenu(false);
                break;
            case "Vammo":
                newBut = _buttons[1].GetComponent<BattleMenuSubButtons>().BattleMenu;
                for (int i = 0; newBut.Length < i; i++)
                {
                    if (newBut[i].active == true) newBut[i].active = false;
                }
                _buttons[0].GetComponent<BattleMenuSubButtons>().ShowSubMenu(false);
                _buttons[2].GetComponent<BattleMenuSubButtons>().ShowSubMenu(false);
                break;
            case "Setting":
                newBut = _buttons[2].GetComponent<BattleMenuSubButtons>().BattleMenu;
                for (int i = 0; newBut.Length < i; i++)
                {
                    if (newBut[i].active == true) newBut[i].active = false;
                }
                _buttons[0].GetComponent<BattleMenuSubButtons>().ShowSubMenu(false);
                _buttons[1].GetComponent<BattleMenuSubButtons>().ShowSubMenu(false);
                break;
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delayOfClick);
    }
}
