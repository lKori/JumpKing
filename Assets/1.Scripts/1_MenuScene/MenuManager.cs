using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Image[] Cursors;

    private enum MenuSceneState { Idle, FadeIn, FadeOut };
    private MenuSceneState m_State;
    private bool isCursorChanged;
    private int cursorNum, previousCursorNum;
    

    private void Start()
    {
        m_State = MenuSceneState.FadeIn;
        isCursorChanged = false;
        cursorNum = 0;
    }

    private void Update()
    {
        switch(m_State)
        {
            case MenuSceneState.Idle:
                UserInput();
                CursorMove();
                break;
            case MenuSceneState.FadeIn:
                break;
            case MenuSceneState.FadeOut:
                break;
        }
    }

    private void UserInput()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
        {
            InputSelect();
            return;
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
            InputDown();
        if (Input.GetKeyDown(KeyCode.UpArrow))
            InputUp();
    }

    private void InputSelect()
    {
        switch (cursorNum)
        {
            case 0:
                SceneManager.LoadScene("2_GameScene");
                break;
            case 1:
                Debug.Log("Game Quit");
                break;
        }
        return;
    }

    private void InputDown()
    {
        previousCursorNum = cursorNum;
        cursorNum++;
        isCursorChanged = true;

        if (cursorNum >= Cursors.Length)
            cursorNum = 0;
    }

    private void InputUp()
    {
        previousCursorNum = cursorNum;
        cursorNum--;
        isCursorChanged = true;

        if (cursorNum < 0)
            cursorNum = Cursors.Length - 1;
    }

    private void CursorMove()
    {
        if (isCursorChanged == false)
            return;

        Cursors[previousCursorNum].gameObject.SetActive(false);
        Cursors[cursorNum].gameObject.SetActive(true);
        isCursorChanged = false;
    }

    public void SetIdle()
    {
        m_State = MenuSceneState.Idle;
    }
}
