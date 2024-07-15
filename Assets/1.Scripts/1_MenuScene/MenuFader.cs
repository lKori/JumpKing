using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFader : MonoBehaviour
{
    [SerializeField]
    private Image TitleFader, MenusFader;
    [SerializeField]
    private float TitleFadeSpeed, MenusFadeSpeed;

    private enum FaderState { TitleFadeIn, MenusFadeIn, Idle };
    private FaderState f_State;

    private void Start()
    {
        f_State = FaderState.TitleFadeIn;
        TitleFader.color = new Color(0f, 0f, 0f, 1f);
        MenusFader.color = new Color(0f, 0f, 0f, 1f);
    }

    private void FixedUpdate()
    {
        switch(f_State)
        {
            case FaderState.TitleFadeIn:
                TitleFadeIn();
                break;
            case FaderState.MenusFadeIn:
                MenusFadeIn();
                break;
            case FaderState.Idle:
                break;
        }
    }

    private void TitleFadeIn()
    {
        TitleFader.color -= new Color(0f, 0f, 0f, TitleFadeSpeed);

        if (TitleFader.color.a <= 0f)
            f_State = FaderState.MenusFadeIn;
    }

    private void MenusFadeIn()
    {
        MenusFader.color -= new Color(0f, 0f, 0f, MenusFadeSpeed);

        if (MenusFader.color.a <= 0f)
        {
            this.GetComponent<MenuManager>().SetIdle();
            f_State = FaderState.Idle;
        }
    }
}
