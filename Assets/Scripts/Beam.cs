using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] // Para que pueda ser guardado o mostrado en el Inspector
public class BeamData
{
    public int id;  //Identificador de viga
    public Vector2 startAnchorPosition; // Posición del punto de anclaje inicial
    public Vector2 endAnchorPosition;   // Posición del punto de anclaje final
    public float angle;                 // Ángulo de la viga
    public float scaleX;               // Escala en X de la viga
}

public class Beam : MonoBehaviour
{
    public AnchorPoint startAnchor;
    public AnchorPoint endAnchor;

    private bool isScaleFrozen = false;
     private SpriteRenderer spriteRenderer;

    public BeamData GetBeamData()
    {
        BeamData data = new BeamData();
        if (startAnchor != null && endAnchor != null)
        {
            data.startAnchorPosition = startAnchor.transform.position;
            data.endAnchorPosition = endAnchor.transform.position;
            data.angle = GetAngle();
            data.scaleX = transform.localScale.x;
        }
        return data;
    }

    public float GetAngle()
    {
        if (startAnchor != null && endAnchor != null)
        {
            Vector2 direction = endAnchor.transform.position - startAnchor.transform.position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        return 0f;
    }

    public float GetScaleX()
    {
        return transform.localScale.x;
    }

     void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (startAnchor != null && endAnchor != null)
        {
            // Actualizar la posición y rotación de la viga
            Vector2 midpoint = (startAnchor.transform.position + endAnchor.transform.position) / 2;
            transform.position = midpoint;

            Vector2 direction = endAnchor.transform.position - startAnchor.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (!isScaleFrozen)
            {
                float distance = Vector2.Distance(startAnchor.transform.position, endAnchor.transform.position);
                transform.localScale = new Vector3(distance, transform.localScale.y, transform.localScale.z);
            }

            // Calcular la tensión y actualizar el color
            float tension = CalculateTension();
            UpdateColorBasedOnTension(tension);
        }
    }

    private float CalculateTension()
    {
        // Fórmula simplificada: tensión = longitud de la viga * fuerza aplicada
        float length = Vector2.Distance(startAnchor.transform.position, endAnchor.transform.position);
        float force = CalculateAppliedForce(); // Método para calcular la fuerza aplicada
        return length * force;
    }

    private float CalculateAppliedForce()
    {
        return 2.0f; 
    }

    private void UpdateColorBasedOnTension(float tension)
    {
        // Cambiar el color según la tensión
        if (tension < 10f)
        {
            spriteRenderer.color = Color.green; // Baja tensión
        }
        else if (tension < 20f)
        {
            spriteRenderer.color = Color.yellow; // Tensión media
        }
        else
        {
            spriteRenderer.color = Color.red; // Alta tensión
        }
    }

    public void FreezeScale()
    {
        isScaleFrozen = true;
    }
}