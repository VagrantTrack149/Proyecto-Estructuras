using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicators : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        // Cambiar el color del SpriteRenderer al hacer clic
        if (spriteRenderer != null)
        {
            spriteRenderer.color= Color.red;
        }
    }
    void OnMouseUp() {
        if (spriteRenderer != null){
            spriteRenderer.color= Color.yellow;
        }
    }

}
