using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Camera cam;
    PlayerMove playerMove;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        cam = GetComponent<Camera>();
        playerMove = GetComponent<PlayerMove>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        GetPlayerPositionY();   // Player의 Y축 높이 가져오기
    }

    private void GetPlayerPositionY()
    {
        //Debug.Log(player.transform.position);

        float playerPos = player.transform.position.y;  // Player의 Y축 높이

        // 만약 7보다 높을 경우 카메라 위로 이동
        if(playerPos > 7.0f)
        {
            cam.transform.position = new Vector3(0, 14.0f, -10.0f);
        }
        else
        {
            // 만약 7보다 낮을 경우 카메라 아래로 이동
            cam.transform.position = new Vector3(0, 0, -10.0f);
        }
    }
}
