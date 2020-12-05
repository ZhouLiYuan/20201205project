using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicController
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] public GameObject graphic;

    void HorizontalMovement()
    {
        if (graphic)
        {
            if (move.x > 0.01f)
            {
                if (graphic.transform.localScale.x == -1)
                {
                    graphic.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

                }
            }
            else if (move.x < -0.01f)
            {
                if (graphic.transform.localScale.x == 1)
                {
                    graphic.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                }
            }

        }

    }
}
