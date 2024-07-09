using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour
{
    enum LogoState { FADE_IN, FADE_OUT };
    private LogoState logoState;

    [SerializeField]
    private TextMeshProUGUI LogoText;

    private void Awake()
    {
        LogoText.color = new Color(
            LogoText.color.r,
            LogoText.color.g,
            LogoText.color.b,
            0);
        logoState = LogoState.FADE_IN;
    }

    private void Update()
    {
        LogoAnimation();
    }

    private void LogoAnimation()
    {
        LogoStateCheck();

        switch (logoState)
        {
            case (LogoState.FADE_IN):
                LogoFadeIn();
                break;
            case (LogoState.FADE_OUT):
                LogoFadeOut();
                break;
            default:
                break;
        }
    }

    private void LogoStateCheck()
    {
        if (logoState == LogoState.FADE_IN && LogoText.color.a >= 0.95f)
            logoState = LogoState.FADE_OUT;
        else if (logoState == LogoState.FADE_OUT && LogoText.color.a <= 0.05f)
            LoadMenuScene();
    }

    private void LogoFadeIn()
    {
        LogoText.color += new Color(0, 0, 0, 0.4f * Time.deltaTime);
    }

    private void LogoFadeOut()
    {
        LogoText.color -= new Color(0, 0, 0, 0.4f * Time.deltaTime);
    }

    private void LoadMenuScene()
    {
        SceneManager.LoadScene("1_MenuScene");
    }
}