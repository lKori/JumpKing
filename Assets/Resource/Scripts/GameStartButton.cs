using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour
{
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickStartButton()
    {
        Debug.Log("click start button");
        SceneManager.LoadScene("SampleScene");
    }
}
