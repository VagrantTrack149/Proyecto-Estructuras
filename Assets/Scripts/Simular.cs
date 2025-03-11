using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simular : MonoBehaviour
{
    public void reproducir(){
        // 1. Leer los GameObjects con la etiqueta "AnchorPoint"
        GameObject[] anchorPoints = GameObject.FindGameObjectsWithTag("AnchorPoint");

        // 2. Leer los GameObjects con la etiqueta "Beam"
        GameObject[] beams = GameObject.FindGameObjectsWithTag("Beam");

        // 3. Quitarles los componentes Rigidbody2D
        QuitarRigidbody2D(anchorPoints);
        QuitarRigidbody2D(beams);

        // 4. Crear un nuevo objeto llamado "Estructuras"
        GameObject estructuras = new GameObject("Estructuras");

        // 5. Colocar como hijos a los GameObjects de AnchorPoint y Beam
        ColocarComoHijos(estructuras, anchorPoints);
        ColocarComoHijos(estructuras, beams);

        // 6. Colocarle a "Estructuras" el componente Rigidbody2D
        estructuras.AddComponent<Rigidbody2D>();
    }
void QuitarRigidbody2D(GameObject[] objetos)
    {
        foreach (GameObject obj in objetos)
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Destroy(rb);
            }
        }
    }

    void ColocarComoHijos(GameObject padre, GameObject[] hijos)
    {
        foreach (GameObject hijo in hijos)
        {
            hijo.transform.parent = padre.transform;
        }
    }
}
