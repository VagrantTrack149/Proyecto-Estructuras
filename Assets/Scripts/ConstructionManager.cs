using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance; // Instancia para usarse en el punto de anclaje

    public GameObject beamPrefab; // Prefab de la viga
    public GameObject anchorPointPrefab; // Prefab del punto de anclaje

    private AnchorPoint startAnchor; // Punto de anclaje inicial
    private GameObject tempBeam; // Viga temporal
    private AnchorPoint tempAnchor; // Punto de anclaje temporal(Propio)

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (startAnchor != null)
        {
            UpdateTempBeam();
        }

        if (Input.GetMouseButtonUp(0)) // Al soltar el clic
        {
            if (startAnchor != null)
            {
                FinishCreatingBeam();
            }
        }
    }

    public void StartCreatingBeam(AnchorPoint anchor)
    {
        startAnchor = anchor;

        // Crear viga temporal
        tempBeam = Instantiate(beamPrefab);
        Beam beamScript = tempBeam.GetComponent<Beam>();
        beamScript.startAnchor = startAnchor;

        // Crear punto de anclaje temporal
        tempAnchor = Instantiate(anchorPointPrefab).GetComponent<AnchorPoint>();
        tempAnchor.isFixed = false; // El punto de anclaje es propio de la viga
    }

    private void UpdateTempBeam()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Mover el punto de anclaje temporal al ratón
        tempAnchor.transform.position = mousePosition;

        // Actualizar la viga temporal
        Beam beamScript = tempBeam.GetComponent<Beam>();
        beamScript.endAnchor = tempAnchor;
    }

    private void FinishCreatingBeam()
{
    // Buscar el punto de anclaje más cercano
    AnchorPoint endAnchor = FindClosestAnchor(tempAnchor.transform.position);

    if (endAnchor != null)
    {
        // Mover el punto de anclaje temporal a la posición del punto de anclaje más cercano
        tempAnchor.transform.position = endAnchor.transform.position;

        // Conectar la viga al punto de anclaje existente
        Beam beamScript = tempBeam.GetComponent<Beam>();
        beamScript.endAnchor = endAnchor;

        // Destruir el punto de anclaje temporal
        Destroy(tempAnchor.gameObject);
    }
    else
    {
        // Fijar el punto de anclaje temporal en su posición actual
        tempAnchor.isFixed = true;
    }

    startAnchor = null;
    tempBeam = null;
    tempAnchor = null;
}

    private AnchorPoint FindClosestAnchor(Vector2 position)
    {
        float minDistance = 1f; // Distancia máxima para conectar a un punto de anclaje
        AnchorPoint closestAnchor = null;

        foreach (AnchorPoint anchor in FindObjectsOfType<AnchorPoint>())
        {
            if (anchor != startAnchor && anchor.isFixed)
            {
                float distance = Vector2.Distance(position, anchor.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestAnchor = anchor;
                }
            }
        }

        return closestAnchor;
    }
}