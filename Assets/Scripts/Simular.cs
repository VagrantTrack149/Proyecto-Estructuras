using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simular : MonoBehaviour
{
    public void reproducir(){
        Debug.Log("Hacer click en simular");
        // Busca todos los objetos con la etiqueta "AnchorPoint"
        GameObject[] anchorPoints = GameObject.FindGameObjectsWithTag("AnchorPoint");

        // Recorre cada objeto encontrado
        foreach (GameObject anchorPoint in anchorPoints)
        {
            // Obtiene el componente AnchorPoint
            AnchorPoint anchorComponent = anchorPoint.GetComponent<AnchorPoint>();

            // Verifica si el componente existe y si isFixed es false
            if (anchorComponent != null && anchorComponent.isMoving)
            {
                // Obtiene el componente Rigidbody2D
                Rigidbody2D rb = anchorPoint.GetComponent<Rigidbody2D>();

                // Si el Rigidbody2D existe, cambia su Body Type a Dynamic
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    Debug.Log($"Changed {anchorPoint.name} Rigidbody2D to Dynamic.");
                }
            }
        }
    }
}
