using UnityEngine;

public class Beam : MonoBehaviour
{
    public AnchorPoint startAnchor;
    public AnchorPoint endAnchor;

    private bool isScaleFrozen = false; // Bandera para congelar la escala

    public void FreezeScale()
    {
        isScaleFrozen = true; // Congela la escala 
    }

    void Update()
    {
        if (startAnchor != null && endAnchor != null)
        {
            // Actualiza la posición y rotación de la viga
            Vector2 midpoint = (startAnchor.transform.position + endAnchor.transform.position) / 2;
            transform.position = midpoint;

            Vector2 direction = endAnchor.transform.position - startAnchor.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Solo actualiza la escala si no está congelada
            if (!isScaleFrozen)
            {
                float distance = Vector2.Distance(startAnchor.transform.position, endAnchor.transform.position);
                transform.localScale = new Vector3(distance, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}