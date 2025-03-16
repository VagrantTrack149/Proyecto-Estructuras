using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{
    public float forceMagnitude = 10f; // Magnitud de la fuerza aplicada

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.down * forceMagnitude, ForceMode2D.Impulse);
        }
    }
}
