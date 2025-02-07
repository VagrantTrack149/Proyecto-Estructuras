using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public GameObject beamPrefab;
    private AnchorPoint selectedAnchor;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
    {
        Debug.Log("Clic detectado.");
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log("Objeto detectado: " + hit.collider.name);
            if (hit.collider.CompareTag("AnchorPoint"))
            {
                Debug.Log("Punto de anclaje detectado.");
                AnchorPoint anchor = hit.collider.GetComponent<AnchorPoint>();

                if (anchor == null)
                {
                    Debug.LogError("El objeto con tag 'AnchorPoint' no tiene el componente AnchorPoint.");
                    return;
                }

                if (selectedAnchor == null)
                {
                    Debug.Log("Primer punto de anclaje seleccionado.");
                    selectedAnchor = anchor;
                }
                else
                {
                    Debug.Log("Creando viga entre puntos de anclaje.");
                    if (beamPrefab == null)
                    {
                        Debug.LogError("beamPrefab no está asignado en el ConstructionManager.");
                        return;
                    }

                    CreateBeam(selectedAnchor, anchor);
                    selectedAnchor = null;
                }
            }
        }
        else
        {
            Debug.Log("No se detectó ningún objeto con el clic.");
        }
    }
    }

    void CreateBeam(AnchorPoint start, AnchorPoint end)
    {
        GameObject beam = Instantiate(beamPrefab);
        Beam beamScript = beam.GetComponent<Beam>();
        beamScript.startAnchor = start;
        beamScript.endAnchor = end;
    }
}