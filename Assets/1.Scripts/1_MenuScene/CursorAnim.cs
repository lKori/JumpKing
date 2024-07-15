using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorAnim : MonoBehaviour
{
    [SerializeField]
    private float ANIM_SPEED;

    private Image image;
    private bool FadeDirection;

    void Start()
    {
        image = GetComponent<Image>();
        FadeDirection = false;
    }

    private void FixedUpdate()
    {
        if(FadeDirection ==  false)
        {
            image.color -= new Color(0, 0, 0, ANIM_SPEED);

            if (image.color.a <= 0)
                FadeDirection = true;
        }
        else
        {
            image.color += new Color(0, 0, 0, ANIM_SPEED);

            if (image.color.a >= 1)
                FadeDirection = false;
        }
    }
}
